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

namespace WafclastRPG.Commands.AdminCommands
{
    public class DatabaseCommands : BaseCommandModule
    {
        public Database database;
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
            var result = await database.CollectionJogadores.DeleteOneAsync(x => x.Id == user.Id);
            if (result.DeletedCount >= 1)
                await ctx.ResponderAsync("usuario deletado!");
            else
                await ctx.ResponderAsync("não foi possível deletar! Tente novamente");
        }

        [Command("mapa-criar")]
        [Description("Permite transforma o canal atual em um mapa para se tornar jogável.")]
        [Usage("mapa-criar")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task CriarMapaAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var result = await database.FindMapAsync(ctx.Channel.Id);
            if (result != null)
            {
                await ctx.ResponderAsync("já existe um mapa neste canal! Você não queria editar?");
                return;
            }

            var mapa = new WafclastMapa(ctx.Channel.Id, ctx.Guild.Id);

            var x = await ctx.WaitForIntAsync("Informe um valor para a coordenada X. Ela será usada futuramente.", database, timeoutoverride);
            if (x.TimedOut)
                return;

            var y = await ctx.WaitForIntAsync("Informe um valor para a coordenada Y. Ela será usada futuramente.", database, timeoutoverride);
            if (y.TimedOut)
                return;
            mapa.Coordinates = new WafclastCoordinates(x.Result, y.Result);

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
            mapa.Tipo = tipo.Result;

            await database.CollectionMaps.InsertOneAsync(mapa);
            await ctx.ResponderAsync($"mapa {Formatter.InlineCode(ctx.Channel.Name)} criado! Nas coordenadas {Formatter.Bold($"({x.Result} | {y.Result})")} com o tipo {Formatter.Bold(mapa.Tipo.ToString())}.");
        }

        [Command("mapa-editar")]
        [Description("Permite editar o mapa  atual.")]
        [Usage("mapa-editar")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task EditarMapaAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var mapaAnterior = await database.FindMapAsync(ctx.Channel.Id);
            if (mapaAnterior == null)
            {
                await ctx.ResponderAsync("não existe um mapa neste canal! Você não queria criar?");
                return;
            }

            var mapa = new WafclastMapa(ctx.Channel.Id, ctx.Guild.Id);

            var x = await ctx.WaitForIntAsync("Informe um valor para a coordenada X. Ela será usada futuramente.", database, timeoutoverride);
            if (x.TimedOut)
                return;

            var y = await ctx.WaitForIntAsync("Informe um valor para a coordenada Y. Ela será usada futuramente.", database, timeoutoverride);
            if (y.TimedOut)
                return;
            mapa.Coordinates = new WafclastCoordinates(x.Result, y.Result);

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
            mapa.Tipo = tipo.Result;
            mapa.QuantidadeMonstros = mapaAnterior.QuantidadeMonstros;

            await database.CollectionMaps.ReplaceOneAsync(x => x.Id == mapa.Id, mapa);
            await ctx.ResponderAsync($"mapa {Formatter.InlineCode(ctx.Channel.Name)} editado! Novas coordenadas {Formatter.Bold($"({x.Result} | {y.Result})")} com o tipo {Formatter.Bold(mapa.Tipo.ToString())}.");
        }

        [Command("monstro-criar")]
        [Description("Permite criar um monstro para o mapa atual.")]
        [Usage("monstro-criar")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task MonstroCriarAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var map = await database.FindMapAsync(ctx);
            if (map == null)
            {
                await ctx.ResponderAsync("não existe um mapa no canal atual! Crie um antes!");
                return;
            }

            var monster = new WafclastMonster(ctx.Channel.Id, (ulong)map.QuantidadeMonstros + 1);

            var name = await ctx.WaitForStringAsync("Informe um nome.", database, timeoutoverride);
            if (name.TimedOut)
                return;

            var respawnTime = await ctx.WaitForIntAsync("Quanto tempo para voltar a vida. (em segundos)", database, timeoutoverride);
            if (respawnTime.TimedOut)
                return;

            var forcaMin = await ctx.WaitForIntAsync($"Quantos de força minima {name.Result} pode ter?", database, timeoutoverride);
            if (forcaMin.TimedOut)
                return;

            var forcaMax = await ctx.WaitForIntAsync($"Quantos de força máxima {name.Result} pode ter? Deve ser maior que a força minima.", database, timeoutoverride, forcaMin.Result);
            if (forcaMax.TimedOut)
                return;

            var resistenciaMin = await ctx.WaitForIntAsync($"Quantos de resistencia minima {name.Result} pode ter?", database, timeoutoverride);
            if (resistenciaMin.TimedOut)
                return;

            var resistenciaMax = await ctx.WaitForIntAsync($"Quantos de resistencia máxima {name.Result} pode ter? Deve ser maior que a resistencia minima.", database, timeoutoverride, resistenciaMin.Result);
            if (resistenciaMax.TimedOut)
                return;

            var agilidadeMin = await ctx.WaitForIntAsync($"Quantos de agilidade minima {name.Result} pode ter?", database, timeoutoverride);
            if (agilidadeMin.TimedOut)
                return;

            var agilidadeMax = await ctx.WaitForIntAsync($"Quantos de agilidade máxima {name.Result} pode ter? Deve ser maior que a agilidade minima.", database, timeoutoverride, agilidadeMin.Result);
            if (agilidadeMax.TimedOut)
                return;

            var experienciaMin = await ctx.WaitForIntAsync($"Quantos de experiencia minima os jogadores recebem ao eliminar {name.Result}?", database, timeoutoverride);
            if (experienciaMin.TimedOut)
                return;

            var experienciaMax = await ctx.WaitForIntAsync($"Quantos de experiencia máxima os jogadores recebem ao eliminar {name.Result}? Deve ser maior que a experiencia minima.", database, timeoutoverride, experienciaMin.Result);
            if (experienciaMax.TimedOut)
                return;

            monster.Atributos = new WafclastMonsterAtributos(forcaMin.Result, forcaMax.Result, resistenciaMin.Result,
                                                             resistenciaMax.Result, agilidadeMin.Result,
                                                             agilidadeMax.Result, experienciaMin.Result,
                                                             experienciaMax.Result);

            monster.Nome = name.Result;
            monster.RespawnTime = TimeSpan.FromSeconds(respawnTime.Result);
            monster.CalcAtributos();

            await database.CollectionMonsters.InsertOneAsync(monster);

            map.QuantidadeMonstros++;
            await database.CollectionMaps.ReplaceOneAsync(x => x.Id == map.Id, map);

            var embed = new DiscordEmbedBuilder();
            embed.WithTitle(monster.Nome.Titulo());
            embed.AddField("Força".Titulo(), $"{monster.Atributos.ForcaMin} ~ {monster.Atributos.ForcaMax}");
            embed.AddField("Resistencia".Titulo(), $"{monster.Atributos.ResistenciaMin} ~ {monster.Atributos.ResistenciaMax}");
            embed.AddField("Agilidade".Titulo(), $"{monster.Atributos.AgilidadeMin} ~ {monster.Atributos.AgilidadeMax}");
            embed.AddField("Experiencia".Titulo(), $"{monster.Atributos.ExpMin} ~ {monster.Atributos.ExpMax}");
            embed.AddField("Respawn a cada".Titulo(), $"{monster.RespawnTime}");

            await ctx.ResponderAsync($"monstro {Formatter.Bold(monster.Nome)} criado! Pode ser encontrado com o #ID {monster.MonsterId}.", embed.Build());
        }

        [Command("monstro-editar")]
        [Description("Permite editar um monstro que percente ao mapa atual.")]
        [Usage("monstro-editar [ id ]")]
        [Example("monstro-editar 3", "Começa a editar o monstro de ID 3")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task MonstroEditarAsync(CommandContext ctx, string stringId = "")
        {
            await ctx.TriggerTypingAsync();
            var map = await database.FindMapAsync(ctx);
            if (map == null)
            {
                await ctx.ResponderAsync("não existe um mapa no canal atual! Crie um antes!");
                return;
            }

            if (string.IsNullOrEmpty(stringId))
            {
                await ctx.ResponderAsync("informe um ID para editar!");
                return;
            }

            if (!int.TryParse(stringId, out int id))
            {
                await ctx.ResponderAsync("o ID precisa ser um número inteiro!");
                return;
            }

            var monsterOld = await database.FindMonsterAsync(ctx.Channel.Id, id);
            if (monsterOld == null)
            {
                await ctx.ResponderAsync("monstro não encontrado. Você não queria criar?");
                return;
            }

            var monster = new WafclastMonster(ctx.Channel.Id, monsterOld.MonsterId);

            var name = await ctx.WaitForStringAsync("Informe um novo nome.", database, timeoutoverride);
            if (name.TimedOut)
                return;

            var respawnTime = await ctx.WaitForIntAsync("Quanto tempo para voltar a vida. (em segundos)", database, timeoutoverride);
            if (respawnTime.TimedOut)
                return;

            var forcaMin = await ctx.WaitForIntAsync($"Quantos de força minima {name.Result} pode ter?", database, timeoutoverride);
            if (forcaMin.TimedOut)
                return;

            var forcaMax = await ctx.WaitForIntAsync($"Quantos de força máxima {name.Result} pode ter? Deve ser maior que a força minima.", database, timeoutoverride, forcaMin.Result);
            if (forcaMax.TimedOut)
                return;

            var resistenciaMin = await ctx.WaitForIntAsync($"Quantos de resistencia minima {name.Result} pode ter?", database, timeoutoverride);
            if (resistenciaMin.TimedOut)
                return;

            var resistenciaMax = await ctx.WaitForIntAsync($"Quantos de resistencia máxima {name.Result} pode ter? Deve ser maior que a resistencia minima.", database, timeoutoverride, resistenciaMin.Result);
            if (resistenciaMax.TimedOut)
                return;

            var agilidadeMin = await ctx.WaitForIntAsync($"Quantos de agilidade minima {name.Result} pode ter?", database, timeoutoverride);
            if (agilidadeMin.TimedOut)
                return;

            var agilidadeMax = await ctx.WaitForIntAsync($"Quantos de agilidade máxima {name.Result} pode ter? Deve ser maior que a agilidade minima.", database, timeoutoverride, agilidadeMin.Result);
            if (agilidadeMax.TimedOut)
                return;

            var experienciaMin = await ctx.WaitForIntAsync($"Quantos de experiencia minima os jogadores recebem ao eliminar {name.Result}?", database, timeoutoverride);
            if (experienciaMin.TimedOut)
                return;

            var experienciaMax = await ctx.WaitForIntAsync($"Quantos de experiencia máxima os jogadores recebem ao eliminar {name.Result}? Deve ser maior que a experiencia minima.", database, timeoutoverride, experienciaMin.Result);
            if (experienciaMax.TimedOut)
                return;

            monster.Atributos = new WafclastMonsterAtributos(forcaMin.Result, forcaMax.Result, resistenciaMin.Result,
                                                             resistenciaMax.Result, agilidadeMin.Result,
                                                             agilidadeMax.Result, experienciaMin.Result,
                                                             experienciaMax.Result);

            monster.Nome = name.Result;
            monster.RespawnTime = TimeSpan.FromSeconds(respawnTime.Result);
            monster.CalcAtributos();

            await database.CollectionMonsters.ReplaceOneAsync(x => x.Id == monster.Id, monster);

            var embed = new DiscordEmbedBuilder();
            embed.WithTitle(monster.Nome.Titulo());
            embed.AddField("Força".Titulo(), $"{monster.Atributos.ForcaMin} ~ {monster.Atributos.ForcaMax}");
            embed.AddField("Resistencia".Titulo(), $"{monster.Atributos.ResistenciaMin} ~ {monster.Atributos.ResistenciaMax}");
            embed.AddField("Agilidade".Titulo(), $"{monster.Atributos.AgilidadeMin} ~ {monster.Atributos.AgilidadeMax}");
            embed.AddField("Experiencia".Titulo(), $"{monster.Atributos.ExpMin} ~ {monster.Atributos.ExpMax}");
            embed.AddField("Respawn a cada".Titulo(), $"{monster.RespawnTime}");

            await ctx.ResponderAsync($"monstro {Formatter.Bold(monster.Nome)} editado! Pode ser encontrado com o #ID {monster.MonsterId}.", embed.Build());
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

            var itemId = await database.FindLastItem(ctx);

            var item = new WafclastBaseItem();
            item.PlayerId = ctx.Guild.Id;
            item.ItemID = itemId;
            item.Type = type.Result;
            item.Name = name.Result;
            item.Level = level.Result;
            item.Price = price.Result;
            item.CanSell = canSell;
            item.CanStack = canStack;
            item.Description = description.Result;

            embed = ItemBuilder(item);
            switch (item.Type)
            {
                case ItemType.Food:

                    var lifeGain = await ctx.WaitForIntAsync("Por ser do tipo Comida, é preciso informar quantos de vida recupera ao comer.", database, timeoutoverride);
                    if (lifeGain.TimedOut)
                        return;

                    WafclastFood comida = new WafclastFood(item);
                    comida.LifeGain = lifeGain.Result;
                    await database.InsertItemAsync(comida);

                    embed.AddField("Cura".Titulo(), comida.LifeGain.ToString());
                    break;
                default:
                    await database.InsertItemAsync(item);
                    break;
            }
            await ctx.RespondAsync("Item criado!", embed.Build());
        }

        [Command("item-editar")]
        [Description("Permite editar um item.")]
        [Usage("item-editar")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task ItemEditarAsync(CommandContext ctx, string stringId = "")
        {
            await ctx.TriggerTypingAsync();

            if (string.IsNullOrEmpty(stringId))
            {
                await ctx.ResponderAsync("informe um ID para editar!");
                return;
            }

            if (!ulong.TryParse(stringId, out ulong id))
            {
                await ctx.ResponderAsync("o ID precisa ser um número inteiro!");
                return;
            }

            var oldItem = await database.FindItemByItemIdAsync(id, ctx.Guild.Id);
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

            var itemId = await database.FindLastItem(ctx);

            var item = new WafclastBaseItem();
            item.Id = oldItem.Id;
            item.PlayerId = ctx.Guild.Id;
            item.ItemID = itemId;
            item.Type = type.Result;
            item.Name = name.Result;
            item.Level = level.Result;
            item.Price = price.Result;
            item.CanSell = canSell;
            item.CanStack = canStack;
            item.Description = description.Result;

            embed = ItemBuilder(item);
            switch (item.Type)
            {
                case ItemType.Food:

                    var lifeGain = await ctx.WaitForIntAsync("Por ser do tipo Comida, é preciso informar quantos de vida recupera ao comer.", database, timeoutoverride);
                    if (lifeGain.TimedOut)
                        return;

                    WafclastFood comida = new WafclastFood(item);
                    comida.LifeGain = lifeGain.Result;
                    await database.CollectionItens.ReplaceOneAsync(x => x.Id == oldItem.Id, comida);

                    embed.AddField("Cura".Titulo(), comida.LifeGain.ToString());
                    break;
                default:
                    await database.CollectionItens.ReplaceOneAsync(x => x.Id == oldItem.Id, item);
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

            var map = await database.FindMapAsync(ctx);
            if (map == null || map.Tipo != MapType.Cidade)
            {
                await ctx.ResponderAsync("você precisa usar este comando em uma cidade!");
                return;
            }

            var itemId = await ctx.WaitForIntAsync("Informe o numero correspondente ao id do item desejado", database, timeoutoverride);
            if (itemId.TimedOut)
                return;

            var item = await database.FindItemByItemIdAsync((ulong)itemId.Result, ctx.Guild.Id);
            if (item == null)
            {
                await ctx.ResponderAsync("o item com este id não foi encontrado!");
                return;
            }

            map.ShopItens.Add(item);
            await database.CollectionMaps.ReplaceOneAsync(x => x.Id == map.Id, map);

            await ctx.RespondAsync($"Item {item.Name.Titulo()} foi adicionado a loja!");
        }

        //[Command("monstro-recompensa")]
        //[Description("Permite adicionar uma recompensa ao monstro informado.")]
        //[Usage("monstro-recompensa [ id ]")]
        //[Example("monstro-recompensa 1", "Adiciona uma nova recompensa para quando alguem eliminar o monstro de ID 1")]
        //[RequireUserPermissions(Permissions.Administrator)]
        //public async Task MonstroDropAsync(CommandContext ctx, string stringId)
        //{
        //    await ctx.TriggerTypingAsync();
        //    var map = await database.CollectionMaps.Find(x => x.Id == ctx.Channel.Id).FirstOrDefaultAsync();
        //    if (map == null)
        //    {
        //        await ctx.ResponderAsync("não existe um mapa no canal atual!");
        //        return;
        //    }

        //    var monster = await database.CollectionMonsters.Find(x => x.Id == $"{ctx.Channel.Id}:{stringId}").FirstOrDefaultAsync();
        //    if (monster == null)
        //    {
        //        await ctx.ResponderAsync("não existe um monstro com o #ID informado!");
        //        return;
        //    }

        //    var item = await database.FindItemByItemIdAsync(itemId, 0);
        //    if (item == null)
        //    {
        //        await ctx.ResponderAsync("não existe um item com o #ID informado!");
        //        return;
        //    }

        //    var embed = new DiscordEmbedBuilder();
        //    embed.AddField("Item".Titulo(), item.Name);
        //    embed.AddField("Monstro".Titulo(), monster.Nome);
        //    embed.AddField("Chance".Titulo(), (chance * 100).ToString());
        //    embed.AddField("Quantia".Titulo(), $"{quantMin} ~ {quantMax}");

        //    if (monster.Drops.Count > posicao)
        //        monster.Drops[posicao] = new ItemChance(item.Id, chance, quantMin, quantMax);
        //    else
        //        monster.Drops.Add(new ItemChance(item.Id, chance, quantMin, quantMax));
        //    await database.CollectionMonsters.ReplaceOneAsync(x => x.Id == monster.Id, monster);

        //    await ctx.ResponderAsync($"monstro {Formatter.Bold(monster.Nome)} editado! Agora pode cair {Formatter.Bold(item.Name)}.");
        //}

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

            using (IAsyncCursor<WafclastPlayer> cursor = await database.CollectionJogadores.FindAsync(filter, options))
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<WafclastPlayer> list = cursor.Current;

                    foreach (WafclastPlayer item in list)
                    {
                        item.Character.LevelBloqueado = 0;
                        item.Character.Atributos.PontosLivreAtributo = item.Character.Level * 4;

                        await database.CollectionJogadores.ReplaceOneAsync(x => x.Id == item.Id, item);
                    }
                }

            await ctx.RespondAsync("Banco foi atualizado!");
        }

        [Command("atualizar-mapa")]
        [RequireOwner]
        public async Task AtualizarMapAsync(CommandContext ctx)
        {
            FilterDefinition<WafclastMapa> filter = FilterDefinition<WafclastMapa>.Empty;
            FindOptions<WafclastMapa> options = new FindOptions<WafclastMapa>
            {
                BatchSize = 8,
                NoCursorTimeout = false
            };

            using (IAsyncCursor<WafclastMapa> cursor = await database.CollectionMaps.FindAsync(filter, options))
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<WafclastMapa> list = cursor.Current;

                    foreach (WafclastMapa item in list)
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
