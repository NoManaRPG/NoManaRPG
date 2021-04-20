using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using WafclastRPG.Entities;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Entities.Monsters;
using MongoDB.Bson;
using System.Text;
using WafclastRPG.Entities.MercadoGeral;

namespace WafclastRPG.Commands.AdminCommands
{
    public class DatabaseCommands : BaseCommandModule
    {
        public DataBase database;
        public TimeSpan timeoutoverride = TimeSpan.FromMinutes(2);

        [Command("deletarU")]
        [Description("Permite deletar o usuario informado.")]
        [Usage("deletarU <usuario>")]
        [RequireOwner]
        public async Task DeletarUserAsync(CommandContext ctx, DiscordUser user)
        {
            await ctx.TriggerTypingAsync();
            var result = await database.CollectionPlayers.DeleteOneAsync(x => x.Id == user.Id);
            if (result.DeletedCount >= 1)
                await ctx.ResponderAsync("usuario deletado!");
            else
                await ctx.ResponderAsync("não foi possível deletar! Tente novamente");
        }

        [Command("monstroV")]
        [Description("Permite ver informações de um monstro.")]
        [Usage("monstroV <id>")]
        [RequireOwner]
        public async Task VerMonstro(CommandContext ctx, string idString)
        {
            await ctx.TriggerTypingAsync();

            if (!ObjectId.TryParse(idString, out var id))
            {
                await ctx.ResponderAsync("o formato do ID está inválido!");
                return;
            }

            var monster = await database.CollectionMonsters.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (monster == null)
            {
                await ctx.ResponderAsync("não encontrei este monstro, você informou o ID correto?");
                return;
            }

            var embed = new DiscordEmbedBuilder();
            embed.WithTitle(monster.Name);
            embed.WithDescription($"ID `{monster.Id}`" +
                $"\nAndar {monster.FloorLevel}");
            embed.AddField("Vida", monster.Life.MaxValue.ToString("N2"), true);
            embed.AddField("Armadura", monster.Armour.MaxValue.ToString("N2"), true);
            embed.AddField("Dano físico", monster.PhysicalDamage.MaxValue.ToString("N2"), true);
            embed.AddField("Evasão", monster.Evasion.MaxValue.ToString("N2"), true);
            embed.AddField("Precisão", monster.Accuracy.MaxValue.ToString("N2"), true);

            var str = new StringBuilder();
            foreach (var item in monster.DropChances)
            {
                var drop = await database.CollectionItems.Find(x => x.Id == item.Id).FirstOrDefaultAsync();
                str.AppendLine($"{item.Chance * 100}% de cair {item.MinQuantity} ~ {item.MaxQuantity} {drop.Name}.");
            }

            embed.AddField("Drops", str.ToString());
            await ctx.ResponderAsync(embed.Build());
        }

        [Command("andarV")]
        [Description("Permite ver os monstros de um andar.")]
        [Usage("andarV <andar>")]
        [RequireOwner]
        public async Task VerAndar(CommandContext ctx, int andar)
        {
            await ctx.TriggerTypingAsync();

            var monsters = await database.CollectionMonsters.Find(x => x.FloorLevel == andar).ToListAsync();
            if (monsters.Count == 0)
            {
                await ctx.ResponderAsync("não foi encontrado monstros neste andar.");
                return;
            }

            var str = new StringBuilder();
            var embed = new DiscordEmbedBuilder();
            embed.WithTitle($"Andar {andar}.");

            foreach (var item in monsters)
                str.AppendLine($"{item.Name} - `{item.Id}`");

            embed.WithDescription(str.ToString());
            await ctx.ResponderAsync(embed.Build());
        }

        [Command("itensV")]
        [Description("Permite ver todos os itens já criados.")]
        [Usage("itensV <pagina>")]
        [RequireOwner]
        public async Task VerItens(CommandContext ctx, int pagina = 1)
        {
            await ctx.TriggerTypingAsync();

            var itens = await database.CollectionItems.Find(x => x.PlayerId == ctx.Client.CurrentUser.Id)
               .Skip((pagina - 1) * 10)
               .Limit(10)
               .ToListAsync();

            var embed = new DiscordEmbedBuilder();
            var str = new StringBuilder();

            foreach (var item in itens)
                str.AppendLine($"{item.Name} - `{item.Id}`");

            embed.WithTitle($"Página {pagina}");
            embed.WithDescription(str.ToString());
            await ctx.ResponderAsync(embed.Build());
        }

        [Command("monstroEC")]
        [Description("Permite editar/criar monstros.")]
        [Usage("monstroEC [id]")]
        [RequireOwner]
        public async Task MonsterEditCreateAsync(CommandContext ctx, string idString = null)
        {
            await ctx.TriggerTypingAsync();

            WafclastMonster monster = null;
            if (!string.IsNullOrWhiteSpace(idString))
            {
                if (!ObjectId.TryParse(idString, out var id))
                {
                    await ctx.ResponderAsync("o formato do ID está inválido!");
                    return;
                }

                monster = await database.CollectionMonsters.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (monster == null)
                {
                    await ctx.ResponderAsync("não encontrei este monstro, você informou o ID correto?");
                    return;
                }
            }
            else
                monster = new WafclastMonster();

            var name = await ctx.WaitForStringAsync("Nome:", database, timeoutoverride);
            if (name.TimedOut)
                return;
            monster.Name = name.Value;

            var floor = await ctx.WaitForIntAsync("Andar:", database, timeoutoverride);
            if (floor.TimedOut)
                return;
            monster.FloorLevel = floor.Value;

            var physicalDamage = await ctx.WaitForDoubleAsync($"Dano máximo:", database, timeoutoverride);
            if (physicalDamage.TimedOut)
                return;
            monster.PhysicalDamage = new WafclastStatePoints(physicalDamage.Value);

            var evasion = await ctx.WaitForDoubleAsync($"Evasão:", database, timeoutoverride);
            if (evasion.TimedOut)
                return;
            monster.Evasion = new WafclastStatePoints(evasion.Value);

            var accuracy = await ctx.WaitForDoubleAsync($"Precisão:", database, timeoutoverride);
            if (accuracy.TimedOut)
                return;
            monster.Accuracy = new WafclastStatePoints(accuracy.Value);

            var armour = await ctx.WaitForDoubleAsync($"Armadura:", database, timeoutoverride);
            if (armour.TimedOut)
                return;
            monster.Armour = new WafclastStatePoints(armour.Value);

            var life = await ctx.WaitForDoubleAsync($"Vida:", database, timeoutoverride);
            if (life.TimedOut)
                return;
            monster.Life = new WafclastStatePoints(life.Value);

            await database.CollectionMonsters.ReplaceOneAsync(x => x.Id == monster.Id, monster, new ReplaceOptions { IsUpsert = true });

            var embed = new DiscordEmbedBuilder();
            embed.WithTitle(monster.Name.Titulo());
            embed.AddField("Vida".Titulo(), $"{monster.Life.MaxValue}", true);
            embed.AddField("Andar".Titulo(), $"{monster.FloorLevel}", true);
            embed.AddField("Ataque físico".Titulo(), $"{monster.PhysicalDamage.MaxValue}", true);
            embed.AddField("Evasão".Titulo(), $"{monster.Evasion.MaxValue}", true);
            embed.AddField("Precisão".Titulo(), $"{monster.Accuracy.MaxValue}", true);
            embed.AddField("Armadura".Titulo(), $"{monster.Armour.MaxValue}", true);

            await ctx.ResponderAsync($"monstro **{monster.Name}** pode ser encontrado com o ID `{monster.Id}` e no andar {monster.FloorLevel}.", embed.Build());
        }

        [Command("itemEC")]
        [Description("Permite criar um item.")]
        [Usage("itemEC [id]")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task ItemCriarAsync(CommandContext ctx, string idString = null)
        {
            await ctx.TriggerTypingAsync();

            WafclastBaseItem item = null;
            if (!string.IsNullOrWhiteSpace(idString))
            {
                if (!ObjectId.TryParse(idString, out var id))
                {
                    await ctx.ResponderAsync("o formato do ID está inválido!");
                    return;
                }

                item = await database.CollectionItems.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (item == null)
                {
                    await ctx.ResponderAsync("não encontrei este item, você informou o ID correto?");
                    return;
                }
            }
            else
            {
                item = new WafclastBaseItem();
                item.PlayerId = ctx.Client.CurrentUser.Id;
            }

            var name = await ctx.WaitForStringAsync("Nome:", database, timeoutoverride);
            if (name.TimedOut)
                return;
            item.Name = name.Value;

            var embed = new DiscordEmbedBuilder();
            embed.WithDescription($"É possível vender o item?");
            embed.WithFooter("Sim ou Não?");
            bool canSell = await ctx.WaitForBoolAsync(database, embed.Build());
            item.CanSell = canSell;

            var description = await ctx.WaitForStringAsync("Descrição:", database, timeoutoverride);
            if (name.TimedOut)
                return;
            item.Description = description.Value;

            embed = ItemBuilder(item);

            var type = await ctx.WaitForStringAsync("Informe um tipo: comida cozida, comida cru, picareta, nucleo.", database, timeoutoverride);
            if (type.TimedOut)
                return;

            switch (type.Value.ToLower())
            {
                case "comida cozida":
                    var lifeGain = await ctx.WaitForIntAsync("Recupera quantos de vida:", database, timeoutoverride);
                    if (lifeGain.TimedOut)
                        return;
                    var comidaCozinhada = new WafclastCookedFoodItem(item);
                    comidaCozinhada.LifeGain = lifeGain.Value;
                    await database.CollectionItems.ReplaceOneAsync(x => x.Id == comidaCozinhada.Id, comidaCozinhada, new ReplaceOptions { IsUpsert = true });
                    embed.AddField("Recupera de vida", comidaCozinhada.LifeGain.ToString(), true);
                    break;

                case "picareta":
                    var hardness = await ctx.WaitForDoubleAsync("Dureza da picareta:", database, timeoutoverride);
                    if (hardness.TimedOut)
                        return;

                    var chanceBonus = await ctx.WaitForDoubleAsync("Chance extra de drop:", database, timeoutoverride);
                    if (chanceBonus.TimedOut)
                        return;

                    var levelUp = await ctx.WaitForDoubleAsync("Level Up Bonus:", database, timeoutoverride);
                    if (levelUp.TimedOut)
                        return;

                    var strength = await ctx.WaitForDoubleAsync("Força para equipar:", database, timeoutoverride);
                    if (strength.TimedOut)
                        return;

                    var picareta = new WafclastPickaxeItem(item);
                    picareta.Hardness = hardness.Value;
                    picareta.DropChanceBonus = chanceBonus.Value;
                    picareta.LevelUpBonus = levelUp.Value;
                    picareta.Strength = strength.Value;

                    await database.CollectionItems.ReplaceOneAsync(x => x.Id == picareta.Id, picareta, new ReplaceOptions { IsUpsert = true });

                    embed.AddField("Dureza", picareta.Hardness.ToString("N2"), true);
                    embed.AddField("Chance bonus", picareta.DropChanceBonus.ToString("N2"), true);
                    embed.AddField("Melhora em ao evoluir", picareta.LevelUpBonus.ToString("N2"), true);
                    embed.AddField("Força", picareta.Strength.ToString("N2"), true);

                    break;

                case "comida cru":
                    var idTransformString = await ctx.WaitForStringAsync("ID do item após cozido:", database, timeoutoverride);
                    if (idTransformString.TimedOut)
                        return;

                    var cookingLevel = await ctx.WaitForIntAsync("Nível para cozinhar:", database, timeoutoverride);
                    if (cookingLevel.TimedOut)
                        return;

                    var chanceCozinhar = await ctx.WaitForDoubleAsync("Chance para cozinhar:", database, timeoutoverride);
                    if (chanceCozinhar.TimedOut)
                        return;

                    var experience = await ctx.WaitForDoubleAsync("Experiencia ganha ao cozinhar:", database, timeoutoverride);
                    if (experience.TimedOut)
                        return;

                    var comidaCru = new WafclastRawFoodItem(item);
                    comidaCru.CookedItemId = ObjectId.Parse(idTransformString.Value);
                    comidaCru.CookingLevel = cookingLevel.Value;
                    comidaCru.Chance = chanceCozinhar.Value / 100;
                    comidaCru.ExperienceGain = experience.Value;
                    await database.CollectionItems.ReplaceOneAsync(x => x.Id == comidaCru.Id, comidaCru, new ReplaceOptions { IsUpsert = true });
                    var itemCozinhado = await database.CollectionItems.Find(x => x.Id == comidaCru.CookedItemId).FirstOrDefaultAsync();
                    embed.AddField("Item cozinhado", itemCozinhado.Name, true);
                    embed.AddField("Nível para cozinhar", comidaCru.CookingLevel.ToString(), true);
                    embed.AddField("Chance", $"{comidaCru.Chance * 100}%", true);
                    break;

                case "nucleo":
                    var experienceGain = await ctx.WaitForDoubleAsync("Ganha quantos de experiencia:", database, timeoutoverride);
                    if (experienceGain.TimedOut)
                        return;
                    WafclastMonsterCoreItem core = new WafclastMonsterCoreItem(item);
                    core.ExperienceGain = Convert.ToDouble(experienceGain.Value);
                    await database.CollectionItems.ReplaceOneAsync(x => x.Id == core.Id, core, new ReplaceOptions { IsUpsert = true });
                    embed.AddField("Ganha de experiencia".Titulo(), core.ExperienceGain.ToString());
                    break;
                default:
                    await database.CollectionItems.ReplaceOneAsync(x => x.Id == item.Id, item, new ReplaceOptions { IsUpsert = true });
                    break;
            }
            await ctx.RespondAsync("Item editado/criado!", embed.Build());
        }

        private DiscordEmbedBuilder ItemBuilder(WafclastBaseItem item)
        {
            var embed = new DiscordEmbedBuilder();
            embed.WithTitle($"{item.Name.Titulo()}");
            embed.WithDescription(Formatter.BlockCode(item.Description));
            embed.WithThumbnail(item.ImageURL);
            embed.WithColor(DiscordColor.Blue);
            embed.AddField("ID", $"`{item.Id}`", true);
            embed.AddField("Pode vender", item.CanSell ? "Sim" : "Não", true);
            return embed;
        }

        [Command("loja-adicionar-item")]
        [Description("Permite adicionar um item na loja.")]
        [Usage("loja-adicionar-item")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task LojaAdicionarItemAsync(CommandContext ctx)
        {
            //await ctx.TriggerTypingAsync();

            //var map = await database.FindAsync(ctx.Channel);
            //if (map == null || map.Tipo != MapType.Cidade)
            //{
            //    await ctx.ResponderAsync("você precisa usar este comando em uma cidade!");
            //    return;
            //}

            //var itemId = await ctx.WaitForUlongAsync("Informe o numero correspondente ao id do item desejado", database, timeoutoverride);
            //if (itemId.TimedOut)
            //    return;

            //var item = await database.FindAsync(itemId.Value, ctx.Guild);
            //if (item == null)
            //{
            //    await ctx.ResponderAsync("o item com este id não foi encontrado!");
            //    return;
            //}

            //map.ShopItens.Add(item);
            //await database.CollectionMaps.ReplaceOneAsync(x => x.Id == map.Id, map);

            //await ctx.RespondAsync($"Item {item.Name.Titulo()} foi adicionado a loja!");
        }

        [Command("monstroEDROP")]
        [Description("Permite adicionar uma recompensa ao monstro após ser abatido.")]
        [Usage("monstroEDROP <Monstro ID> <Item ID> [Posição]}")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task MonstroDropAsync(CommandContext ctx, string monsterIdString, string itemIdString, int? pos = null)
        {
            await ctx.TriggerTypingAsync();

            WafclastMonster monster = null;

            if (!ObjectId.TryParse(monsterIdString, out var monsterId))
            {
                await ctx.ResponderAsync("o ID do monstro está inválido!");
                return;
            }
            monster = await database.CollectionMonsters.Find(x => x.Id == monsterId).FirstOrDefaultAsync();
            if (monster == null)
            {
                await ctx.ResponderAsync("não encontrei este monstro, você informou o ID correto?");
                return;
            }

            if (!ObjectId.TryParse(itemIdString, out var itemId))
            {
                await ctx.ResponderAsync("o ID do item está inválido!");
                return;
            }

            var item = await database.CollectionItems.Find(x => x.Id == itemId && x.PlayerId == ctx.Client.CurrentUser.Id).FirstOrDefaultAsync();
            if (item == null)
            {
                await ctx.ResponderAsync("não encontrei este item, você informou o ID correto?");
                return;
            }
            var dropChance = new DropChance();
            dropChance.Id = item.Id;

            var chance = await ctx.WaitForDoubleAsync("Chance do item cair:", database, timeoutoverride, 0);
            if (chance.TimedOut)
                return;
            dropChance.Chance = chance.Value / 100;

            var minQuantity = await ctx.WaitForIntAsync("Minimo de quantidade que pode cair:", database, timeoutoverride, 1);
            if (minQuantity.TimedOut)
                return;
            dropChance.MinQuantity = minQuantity.Value;

            var maxQuantity = await ctx.WaitForIntAsync("Máximo de quantidade que pode cair:", database, timeoutoverride, minQuantity.Value);
            if (maxQuantity.TimedOut)
                return;
            dropChance.MaxQuantity = maxQuantity.Value;

            if (pos == null)
                monster.DropChances.Add(dropChance);
            else if (monster.DropChances.Count > pos)
                monster.DropChances[(int)pos] = dropChance;
            else
                monster.DropChances.Add(dropChance);

            await database.CollectionMonsters.ReplaceOneAsync(x => x.Id == monster.Id, monster);

            var embed = new DiscordEmbedBuilder();
            embed.AddField("Item".Titulo(), item.Name, true);
            embed.AddField("Monstro".Titulo(), monster.Name, true);
            embed.AddField("Chance".Titulo(), (dropChance.Chance * 100).ToString(), true);
            embed.AddField("Quantia".Titulo(), $"{dropChance.MinQuantity} ~ {dropChance.MaxQuantity}", true);

            await ctx.ResponderAsync($"monstro {Formatter.Bold(monster.Name)} editado! Agora pode cair:", embed.Build());
        }

        [Command("atualizar-jogadores")]
        [RequireOwner]
        public async Task AtualizarAsync(CommandContext ctx)
        {
            FilterDefinition<WafclastPlayer> filter = FilterDefinition<WafclastPlayer>.Empty;
            FindOptions<WafclastPlayer> options = new FindOptions<WafclastPlayer>
            {
                BatchSize = 8,
                NoCursorTimeout = false
            };

            using (IAsyncCursor<WafclastPlayer> cursor = await database.CollectionPlayers.FindAsync(filter, options))
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<WafclastPlayer> list = cursor.Current;

                    foreach (WafclastPlayer item in list)
                    {

                        //item.Character.Strength = new WafclastStatePoints(20);
                        //item.Character.Intelligence = new WafclastStatePoints(20);
                        //item.Character.Dexterity = new WafclastStatePoints(20);

                        //item.Character.PhysicalDamage = new WafclastStatePoints(8);
                        //item.Character.PhysicalDamage.MultValue += item.Character.Strength.CurrentValue * 0.2;
                        //item.Character.PhysicalDamage.Restart();

                        //item.Character.Evasion = new WafclastStatePoints(53);
                        //item.Character.Evasion.MultValue += item.Character.Dexterity.CurrentValue * 0.2;
                        //item.Character.Evasion.Restart();

                        //item.Character.Accuracy = new WafclastStatePoints(item.Character.Dexterity.CurrentValue * 2);

                        //item.Character.Armour = new WafclastStatePoints(0);

                        //item.Character.EnergyShield = new WafclastStatePoints(0);
                        //item.Character.EnergyShield.MultValue += item.Character.Intelligence.CurrentValue * 0.2;

                        //item.Character.Life = new WafclastStatePoints(50);
                        //item.Character.Life.BaseValue += item.Character.Strength.CurrentValue * 0.5;

                        //item.Character.Life.Restart();

                        //item.Character.Mana = new WafclastStatePoints(40);
                        //item.Character.Mana.BaseValue += item.Character.Intelligence.CurrentValue * 0.5;
                        //item.Character.Mana.Restart();

                        //item.Character.LifeRegen = new WafclastStatePoints(0);
                        //item.Character.ManaRegen = new WafclastStatePoints(item.Character.Mana.MaxValue * 0.08);
                        //item.Character.AttributePoints = (item.Character.Level - 1) * 10;

                        //item.Character.MineSkill = new WafclastLevel(1);

                        //if (item.Character.CookingSkill == null)
                        //    item.Character.CookingSkill = new WafclastLevel(1);

                        //if (item.Character.Level > 1)
                        //{
                        //    item.Character.Accuracy.BaseValue += 2 * (item.Character.Level - 1);
                        //    item.Character.Life.BaseValue += 12 * (item.Character.Level - 1);
                        //}
                        item.Character.ExperienceForNextLevel = item.Character.ExperienceTotalLevel(2);
                        item.Character.MineSkill.ExperienceForNextLevel = item.Character.MineSkill.ExperienceTotalLevel(2);
                        item.Character.CookingSkill.ExperienceForNextLevel = item.Character.CookingSkill.ExperienceTotalLevel(2);

                        item.Character.Level = 1;
                        item.Character.MineSkill.Level = 1;
                        item.Character.CookingSkill.Level = 1;


                        await database.CollectionPlayers.ReplaceOneAsync(x => x.Id == item.Id, item);
                    }
                }

            await ctx.RespondAsync("Banco foi atualizado!");
        }

        [Command("novaAcao")]
        [RequireOwner]
        public async Task NovaAcao(CommandContext ctx, ulong preco, [RemainingText] string nome)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var acao = await session.FindAcaoAsync(nome);
                    if (acao != null)
                        return new Response("essa ação já existe!");

                    acao = new Acao();
                    acao.Id = nome;
                    acao.PrecoMedio = preco;
                    await session.InsertAsync(acao);

                    return new Response("ação criada!");
                });

            if (!string.IsNullOrWhiteSpace(response.Message))
            {
                await ctx.ResponderAsync(response.Message);
                return;
            }

            await ctx.ResponderAsync(response.Embed.Build());
        }

        [Command("atualizar-itens")]
        [RequireOwner]
        public async Task AtualizarItensAsync(CommandContext ctx)
        {
            FilterDefinition<WafclastBaseItem> filter = FilterDefinition<WafclastBaseItem>.Empty;
            FindOptions<WafclastBaseItem> options = new FindOptions<WafclastBaseItem>
            {
                BatchSize = 8,
                NoCursorTimeout = false
            };

            using (IAsyncCursor<WafclastBaseItem> cursor = await database.CollectionItems.FindAsync(filter, options))
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<WafclastBaseItem> list = cursor.Current;

                    foreach (WafclastBaseItem item in list)
                    {
                        switch (item.Name)
                        {
                            case "Galinha Crú":
                                item.Name = "Galinha Cru";
                                break;
                            case "Carne de Coelho Crú":
                                item.Name = "Carne de Coelho Cru";
                                break;
                        }
                        await database.CollectionItems.ReplaceOneAsync(x => x.Id == item.Id, item);
                    }
                }

            await ctx.RespondAsync("Banco foi atualizado!");
        }

        [Command("everyone-role")]
        [Description("Permite dar a todos os membros um cargo especifico.")]
        [Usage("everyone-role [ cargo ]")]
        [RequireOwner]
        public async Task EveryoneRoleAsync(CommandContext ctx, DiscordRole role)
        {
            var members = await ctx.Guild.GetAllMembersAsync();
            try
            {
                foreach (var member in members)
                    await member.GrantRoleAsync(role);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            await ctx.ResponderAsync($"{members.Count} ganharam a badge!");
        }

        [Command("sudo")]
        [RequireOwner]
        public async Task Sudo(CommandContext ctx, DiscordUser member, [RemainingText] string command)
        {
            await ctx.TriggerTypingAsync();
            var cmd = ctx.CommandsNext.FindCommand(command, out var args);
            if (cmd == null)
            {
                await ctx.RespondAsync("Comando não encontrado");
                return;
            }

            var cfx = ctx.CommandsNext.CreateFakeContext(member, ctx.Channel, "", "w.", cmd, args);
            await ctx.CommandsNext.ExecuteCommandAsync(cfx);
        }
    }
}
