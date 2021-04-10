using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using WafclastRPG.Entities;
using WafclastRPG.Enums;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Monsters;
using WafclastRPG.Entities.Itens;
using DSharpPlus.Interactivity.Extensions;
using System.Text;
using System.Linq;
using WafclastRPG.Entities.Maps;

namespace WafclastRPG.Commands.AdminCommands
{
    public class DatabaseCommands : BaseCommandModule
    {
        public DataBase database;
        public TimeSpan timeoutoverride = TimeSpan.FromMinutes(2);

        [Command("deletar-user")]
        [Description("Permite deletar o usuario informado.")]
        [Usage("deletar-user [ @menção | id ]")]
        [Example("deletar-user @talion", "Deleta o usuario mencionado.")]
        [Example("deletar-user 1239485", "Deleta o usuario informado.")]
        [RequireOwner]
        public async Task DeletarUserAsync(CommandContext ctx, DiscordUser user = null)
        {
            await ctx.TriggerTypingAsync();
            if (user == null) user = ctx.User;
            var result = await database.CollectionPlayers.DeleteOneAsync(x => x.Id == user.Id);
            if (result.DeletedCount >= 1)
                await ctx.ResponderAsync("usuario deletado!");
            else
                await ctx.ResponderAsync("não foi possível deletar! Tente novamente");
        }

        [Command("mapa-criar")]
        [Description("Permite transforma o canal de texto atual em um mapa.")]
        [Usage("mapa-criar")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task CriarMapaAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var result = await database.FindAsync(ctx.Channel);
            if (result != null)
            {
                await ctx.ResponderAsync("já existe um mapa neste canal! Você não queria editar?");
                return;
            }

            var mapa = new WafclastMap(ctx.Channel.Id, ctx.Guild.Id);

            var x = await ctx.WaitForIntAsync("Informe um valor para a coordenada X. Ela será usada futuramente.", database, timeoutoverride);
            if (x.TimedOut)
                return;

            var y = await ctx.WaitForIntAsync("Informe um valor para a coordenada Y. Ela será usada futuramente.", database, timeoutoverride);
            if (y.TimedOut)
                return;
            mapa.Coordinates = new WafclastCoordinates(x.Value, y.Value);


            var str = new StringBuilder();
            int ind = 0;
            foreach (var item in Enum.GetValues(typeof(MapType)).Cast<MapType>())
            {
                str.AppendLine($"{ind} -> {item.GetEnumDescription()}");
                ind++;
            }
            var tipo = await ctx.WaitForEnumAsync<MapType>("Informe o numero correspondente ao tipo desejado do seu mapa\n" + str.ToString(), database, timeoutoverride);
            if (tipo.TimedOut)
                return;
            mapa.Tipo = tipo.Value;


            await database.CollectionMaps.InsertOneAsync(mapa);
            await ctx.ResponderAsync($"mapa {Formatter.InlineCode(ctx.Channel.Name)} criado! Nas coordenadas {Formatter.Bold($"({x.Value} | {y.Value})")} com o tipo {Formatter.Bold(mapa.Tipo.ToString())}.");
        }

        [Command("mapa-editar")]
        [Description("Permite editar o mapa  atual.")]
        [Usage("mapa-editar")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task EditarMapaAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var mapaAnterior = await database.FindAsync(ctx.Channel);
            if (mapaAnterior == null)
            {
                await ctx.ResponderAsync("não existe um mapa neste canal! Você não queria criar?");
                return;
            }

            var mapa = new WafclastMap(ctx.Channel.Id, ctx.Guild.Id);

            var x = await ctx.WaitForIntAsync("Informe um valor para a coordenada X. Ela será usada futuramente.", database, timeoutoverride);
            if (x.TimedOut)
                return;

            var y = await ctx.WaitForIntAsync("Informe um valor para a coordenada Y. Ela será usada futuramente.", database, timeoutoverride);
            if (y.TimedOut)
                return;
            mapa.Coordinates = new WafclastCoordinates(x.Value, y.Value);

            var str = new StringBuilder();
            int ind = 0;
            foreach (var item in Enum.GetValues(typeof(MapType)).Cast<MapType>())
            {
                str.AppendLine($"{ind} -> {item.GetEnumDescription()}");
                ind++;
            }

            var tipo = await ctx.WaitForEnumAsync<MapType>("Informe o numero correspondente ao tipo desejado do seu mapa\n" + str.ToString(), database, timeoutoverride);
            if (tipo.TimedOut)
                return;
            mapa.Tipo = tipo.Value;
            mapa.QuantidadeMonstros = mapaAnterior.QuantidadeMonstros;

            await database.CollectionMaps.ReplaceOneAsync(x => x.Id == mapa.Id, mapa);
            await ctx.ResponderAsync($"mapa {Formatter.InlineCode(ctx.Channel.Name)} editado! Novas coordenadas {Formatter.Bold($"({x.Value} | {y.Value})")} com o tipo {Formatter.Bold(mapa.Tipo.ToString())}.");
        }

        [Command("monstro")]
        [Description("Permite editar ou criar um monstro.")]
        [Usage("monstro [ ID ]")]
        [Example("monstro 3", "Começa a editar o monstro de ID 3")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task MonstroEditarAsync(CommandContext ctx, int id = 0)
        {
            await ctx.TriggerTypingAsync();
            var map = await database.FindAsync(ctx.Channel);
            if (map == null)
            {
                await ctx.ResponderAsync("não existe um mapa no canal atual! Crie um antes!");
                return;
            }

            if (map.Tipo == MapType.Cidade)
            {
                await ctx.ResponderAsync("você não pode criar monstros na cidade!");
                return;
            }

            var monster = await database.FindAsync(new WafclastMonsterBase(ctx.Channel.Id, id));
            if (monster == null)
            {
                monster = new WafclastMonster(ctx.Channel.Id, map.QuantidadeMonstros + 1);
                map.QuantidadeMonstros++;
                await ctx.ResponderAsync($"como não encontrei este monstro, vou criar um novo  para você com o ID **{monster.MonsterId}**!");
            }

            var name = await ctx.WaitForStringAsync("Por favor, informe um nome para o seu monstro!", database, timeoutoverride);
            if (name.TimedOut)
                return;
            monster.Name = name.Value;

            var respawnTime = await ctx.WaitForIntAsync("Quantos segundos precisa esperar para voltar a vida?", database, timeoutoverride);
            if (respawnTime.TimedOut)
                return;
            monster.RespawnTime = TimeSpan.FromSeconds(respawnTime.Value);

            var physicalDamage = await ctx.WaitForIntAsync($"Qual o dano máximo?", database, timeoutoverride);
            if (physicalDamage.TimedOut)
                return;
            monster.PhysicalDamage = new WafclastStatePoints(physicalDamage.Value);

            var evasion = await ctx.WaitForIntAsync($"Quantos pontos de evasão terá?", database, timeoutoverride);
            if (evasion.TimedOut)
                return;
            monster.Evasion = new WafclastStatePoints(evasion.Value);

            var accuracy = await ctx.WaitForIntAsync($"Quantos pontos de precisão terá?", database, timeoutoverride);
            if (accuracy.TimedOut)
                return;
            monster.Accuracy = new WafclastStatePoints(accuracy.Value);

            var armour = await ctx.WaitForIntAsync($"Quantos pontos de armadura terá?", database, timeoutoverride);
            if (armour.TimedOut)
                return;
            monster.Armour = new WafclastStatePoints(armour.Value);

            var life = await ctx.WaitForIntAsync($"Quantos pontos de vida terá?", database, timeoutoverride);
            if (life.TimedOut)
                return;
            monster.Life = new WafclastStatePoints(life.Value);

            await database.ReplaceAsync(map);
            await database.ReplaceAsync(monster);

            var embed = new DiscordEmbedBuilder();
            embed.WithTitle(monster.Name.Titulo());
            embed.AddField("Vida".Titulo(), $"{monster.Life.MaxValue}");
            embed.AddField("Ataque físico".Titulo(), $"{monster.PhysicalDamage.MaxValue}");
            embed.AddField("Evasão".Titulo(), $"{monster.Evasion.MaxValue}");
            embed.AddField("Precisão".Titulo(), $"{monster.Accuracy.MaxValue}");
            embed.AddField("Armadura".Titulo(), $"{monster.Armour.MaxValue}");
            embed.AddField("Respawn a cada".Titulo(), $"{monster.RespawnTime}");

            await ctx.ResponderAsync($"monstro {Formatter.Bold(monster.Name)} pode ser encontrado com o #ID {monster.MonsterId}.", embed.Build());
        }

        [Command("item-criar")]
        [Description("Permite criar um item.")]
        [Usage("item-criar")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task ItemCriarAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var str = new StringBuilder();
            int ind = 0;
            foreach (var i in Enum.GetValues(typeof(ItemType)).Cast<ItemType>())
            {
                str.AppendLine($"{ind} -> {i.GetEnumDescription()}");
                ind++;
            }

            var type = await ctx.WaitForEnumAsync<ItemType>($"Informe o numero correspondente ao tipo desejado do seu item\n" + str.ToString(), database, timeoutoverride);
            if (type.TimedOut)
                return;

            var name = await ctx.WaitForStringAsync("Informe um nome.", database, timeoutoverride);
            if (name.TimedOut)
                return;

            var level = await ctx.WaitForIntAsync("Informe o nível minimo que permite o item ser usado.", database, timeoutoverride);
            if (level.TimedOut)
                return;

            var price = await ctx.WaitForIntAsync("Informe um valor de compra, o valor de venda será a metade.", database, timeoutoverride);
            if (price.TimedOut)
                return;

            var embed = new DiscordEmbedBuilder();
            embed.WithDescription($"É possível vender o item?");
            embed.WithFooter("Sim ou Não?");
            bool canSell = await ctx.WaitForBoolAsync(database, embed.Build());

            embed = new DiscordEmbedBuilder();
            embed.WithDescription($"É possível fazer pilhas do mesmo item?");
            embed.WithFooter("Sim ou Não?");
            bool canStack = await ctx.WaitForBoolAsync(database, embed.Build());

            var description = await ctx.WaitForStringAsync("Informe uma descrição para o item.", database, timeoutoverride);
            if (name.TimedOut)
                return;

            var itemId = await database.GetLastIDAsync(ctx.Guild);

            var item = new WafclastBaseItem();
            item.PlayerId = ctx.Guild.Id;
            item.ItemID = itemId;
            item.Type = type.Value;
            item.Name = name.Value;
            item.Level = level.Value;
            item.Price = price.Value;
            item.CanSell = canSell;
            item.CanStack = canStack;
            item.Description = description.Value;

            embed = ItemBuilder(item);
            switch (item.Type)
            {
                case ItemType.Food:

                    var lifeGain = await ctx.WaitForIntAsync("Por ser do tipo Comida, é preciso informar quantos de vida recupera ao comer.", database, timeoutoverride);
                    if (lifeGain.TimedOut)
                        return;

                    WafclastFood comida = new WafclastFood(item);
                    comida.LifeGain = lifeGain.Value;
                    await database.InsertAsync(comida);

                    embed.AddField("Cura".Titulo(), comida.LifeGain.ToString());
                    break;
                default:
                    await database.InsertAsync(item);
                    break;
            }
            await ctx.RespondAsync("Item criado!", embed.Build());
        }

        [Command("item-editar")]
        [Description("Permite editar um item.")]
        [Usage("item-editar")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task ItemEditarAsync(CommandContext ctx, ulong id = 1)
        {
            await ctx.TriggerTypingAsync();


            var oldItem = await database.FindAsync(id, ctx.Guild);
            if (oldItem == null)
            {
                await ctx.ResponderAsync("item não encontrado. Você não queria criar?");
                return;
            }


            var str = new StringBuilder();
            int ind = 0;
            foreach (var i in Enum.GetValues(typeof(ItemType)).Cast<ItemType>())
            {
                str.AppendLine($"{ind} -> {i.GetEnumDescription()}");
                ind++;
            }

            var type = await ctx.WaitForEnumAsync<ItemType>($"Informe o numero correspondente ao tipo desejado do seu item\n" + str.ToString(), database, timeoutoverride);
            if (type.TimedOut)
                return;

            var name = await ctx.WaitForStringAsync("Informe um nome.", database, timeoutoverride);
            if (name.TimedOut)
                return;

            var level = await ctx.WaitForIntAsync("Informe o nível minimo que permite o item ser usado.", database, timeoutoverride);
            if (level.TimedOut)
                return;

            var price = await ctx.WaitForIntAsync("Informe um valor de compra, o valor de venda será a metade.", database, timeoutoverride);
            if (price.TimedOut)
                return;

            var embed = new DiscordEmbedBuilder();
            embed.WithDescription($"É possível vender o item?");
            embed.WithFooter("Sim ou Não?");
            bool canSell = await ctx.WaitForBoolAsync(database, embed.Build());

            embed = new DiscordEmbedBuilder();
            embed.WithDescription($"É possível fazer pilhas do mesmo item?");
            embed.WithFooter("Sim ou Não?");
            bool canStack = await ctx.WaitForBoolAsync(database, embed.Build());

            var description = await ctx.WaitForStringAsync("Informe uma descrição para o item.", database, timeoutoverride);
            if (name.TimedOut)
                return;

            var itemId = await database.GetLastIDAsync(ctx.Guild);

            var item = new WafclastBaseItem();
            item.Id = oldItem.Id;
            item.PlayerId = ctx.Guild.Id;
            item.ItemID = itemId;
            item.Type = type.Value;
            item.Name = name.Value;
            item.Level = level.Value;
            item.Price = price.Value;
            item.CanSell = canSell;
            item.CanStack = canStack;
            item.Description = description.Value;

            embed = ItemBuilder(item);
            switch (item.Type)
            {
                case ItemType.Food:

                    var lifeGain = await ctx.WaitForIntAsync("Por ser do tipo Comida, é preciso informar quantos de vida recupera ao comer.", database, timeoutoverride);
                    if (lifeGain.TimedOut)
                        return;

                    WafclastFood comida = new WafclastFood(item);
                    comida.LifeGain = lifeGain.Value;
                    await database.CollectionItems.ReplaceOneAsync(x => x.Id == oldItem.Id, comida);

                    embed.AddField("Cura".Titulo(), comida.LifeGain.ToString());
                    break;
                default:
                    await database.CollectionItems.ReplaceOneAsync(x => x.Id == oldItem.Id, item);
                    break;
            }
            await ctx.RespondAsync("Item editado!", embed.Build());
        }

        private DiscordEmbedBuilder ItemBuilder(WafclastBaseItem item)
        {
            var embed = new DiscordEmbedBuilder();
            embed.WithTitle($"[{item.ItemID}] {item.Name.Titulo()}");
            embed.WithDescription(item.Description);
            embed.WithThumbnail(item.ImageURL);
            embed.WithColor(DiscordColor.Blue);
            embed.AddField("Tipo".Titulo(), item.Type.GetEnumDescription(), true);
            embed.AddField("Level".Titulo(), item.Level.ToString(), true);
            embed.AddField("Preço compra".Titulo(), item.Price.ToString(), true);
            embed.AddField("Preço venda".Titulo(), (item.Price / 2).ToString(), true);
            embed.AddField("Pode vender".Titulo(), item.CanSell ? "Sim" : "Não", true);
            embed.AddField("Pode empilhar".Titulo(), item.CanStack ? "Sim" : "Não", true);
            return embed;
        }

        [Command("loja-adicionar-item")]
        [Description("Permite adicionar um item na loja.")]
        [Usage("loja-adicionar-item")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task LojaAdicionarItemAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var map = await database.FindAsync(ctx.Channel);
            if (map == null || map.Tipo != MapType.Cidade)
            {
                await ctx.ResponderAsync("você precisa usar este comando em uma cidade!");
                return;
            }

            var itemId = await ctx.WaitForUlongAsync("Informe o numero correspondente ao id do item desejado", database, timeoutoverride);
            if (itemId.TimedOut)
                return;

            var item = await database.FindAsync(itemId.Value, ctx.Guild);
            if (item == null)
            {
                await ctx.ResponderAsync("o item com este id não foi encontrado!");
                return;
            }

            map.ShopItens.Add(item);
            await database.CollectionMaps.ReplaceOneAsync(x => x.Id == map.Id, map);

            await ctx.RespondAsync($"Item {item.Name.Titulo()} foi adicionado a loja!");
        }

        [Command("monstro-drop")]
        [Description("Permite adicionar uma recompensa ao monstro.")]
        [Usage("monstro-drop [ Monstro ID ] [ Item ID ] [ Posição }")]
        [Example("monstro-drop 1 1 0", "Adiciona uma nova recompensa para quando alguem eliminar o monstro de ID 1")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task MonstroDropAsync(CommandContext ctx, int monsterId = 0, ulong itemId = 0, int pos = 0)
        {
            await ctx.TriggerTypingAsync();
            var map = await database.FindAsync(ctx.Channel);
            if (map == null)
            {
                await ctx.ResponderAsync("não existe um mapa no canal atual!");
                return;
            }

            var monster = await database.FindAsync(new WafclastMonsterBase(ctx.Channel.Id, monsterId));
            if (monster == null)
            {
                await ctx.ResponderAsync("não existe um monstro com o #ID informado!");
                return;
            }

            var item = await database.FindAsync(itemId, ctx.Guild);
            if (item == null)
            {
                await ctx.ResponderAsync("não existe um item com o ID informado!");
                return;
            }

            var chance = await ctx.WaitForDoubleAsync("Qual a chance do item cair?", database, timeoutoverride, 0);
            if (chance.TimedOut)
                return;

            var minQuantity = await ctx.WaitForIntAsync("Qual o minimo de quantidade que pode cair?", database, timeoutoverride, 1);
            if (minQuantity.TimedOut)
                return;

            var maxQuantity = await ctx.WaitForIntAsync("Qual o máximo de quantidade que pode cair?", database, timeoutoverride, minQuantity.Value);
            if (maxQuantity.TimedOut)
                return;

            var drop = new ItemChance(item.Id, chance.Value / 100, minQuantity.Value, maxQuantity.Value);


            var embed = new DiscordEmbedBuilder();
            embed.AddField("Item".Titulo(), item.Name);
            embed.AddField("Monstro".Titulo(), monster.Name);
            embed.AddField("Chance".Titulo(), (drop.Chance * 100).ToString());
            embed.AddField("Quantia".Titulo(), $"{drop.MinQuantity} ~ {drop.MaxQuantity}");

            if (monster.ChanceDrops.Count > pos)
                monster.ChanceDrops[pos] = drop;
            else
                monster.ChanceDrops.Add(drop);
            await database.CollectionMonsters.ReplaceOneAsync(x => x.Id == monster.Id, monster);

            await ctx.ResponderAsync($"monstro {Formatter.Bold(monster.Name)} editado! Agora pode cair {Formatter.Bold(item.Name)}.");
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
                        item.Language = "pt-BR";

                        if (item.Character != null)
                        {
                            item.Character.AttributePoints = item.Character.Level * 10;

                            item.Character.Strength = new WafclastStatePoints(20);
                            item.Character.Intelligence = new WafclastStatePoints(20);
                            item.Character.Dexterity = new WafclastStatePoints(20);

                            item.Character.PhysicalDamage = new WafclastStatePoints(8);
                            item.Character.PhysicalDamage.MultValue += item.Character.Strength.CurrentValue * 0.2M;
                            item.Character.PhysicalDamage.Restart();

                            item.Character.Evasion = new WafclastStatePoints(53);
                            item.Character.Evasion.MultValue += item.Character.Dexterity.CurrentValue * 0.2M;
                            item.Character.Evasion.Restart();

                            item.Character.Accuracy = new WafclastStatePoints(item.Character.Dexterity.CurrentValue * 2);
                            item.Character.Accuracy.BaseValue += 2 * item.Character.Level;

                            item.Character.Armour = new WafclastStatePoints(0);

                            item.Character.EnergyShield = new WafclastStatePoints(0);
                            item.Character.EnergyShield.MultValue += item.Character.Intelligence.CurrentValue * 0.2M;

                            item.Character.Life = new WafclastStatePoints(50);
                            item.Character.Life.BaseValue += item.Character.Strength.CurrentValue * 0.5M;
                            item.Character.Life.BaseValue += 12 * item.Character.Level;
                            item.Character.Life.Restart();

                            item.Character.Mana = new WafclastStatePoints(40);
                            item.Character.Mana.BaseValue += item.Character.Intelligence.CurrentValue * 0.5M;
                            item.Character.Mana.Restart();

                            item.Character.LifeRegen = new WafclastStatePoints(0);
                            item.Character.ManaRegen = new WafclastStatePoints(item.Character.Mana.MaxValue * 0.08M);
                        }

                        await database.CollectionPlayers.ReplaceOneAsync(x => x.Id == item.Id, item);
                    }
                }

            await ctx.RespondAsync("Banco foi atualizado!");
        }

        [Command("atualizar-mapa")]
        [RequireOwner]
        public async Task AtualizarMapAsync(CommandContext ctx)
        {
            FilterDefinition<WafclastMap> filter = FilterDefinition<WafclastMap>.Empty;
            FindOptions<WafclastMap> options = new FindOptions<WafclastMap>
            {
                BatchSize = 8,
                NoCursorTimeout = false
            };

            using (IAsyncCursor<WafclastMap> cursor = await database.CollectionMaps.FindAsync(filter, options))
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<WafclastMap> list = cursor.Current;

                    foreach (WafclastMap item in list)
                    {
                        item.ShopItens = new List<WafclastBaseItem>();

                        await database.CollectionMaps.ReplaceOneAsync(x => x.Id == item.Id, item);
                    }
                }

            await ctx.RespondAsync("Banco foi atualizado!");
        }

        [Command("everyone-role")]
        [Description("Permite dar a todos os membros um cargo especifico.")]
        [Usage("everyone-role [ cargo ]")]
        [Example("everyone-role @Admin", "Dá a todos os membros o cargo de @admin")]
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
            var invocation = command.Substring(2);
            var cmd = ctx.CommandsNext.FindCommand(invocation, out var args);
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
