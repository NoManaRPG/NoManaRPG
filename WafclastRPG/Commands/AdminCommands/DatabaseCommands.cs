﻿using DSharpPlus;
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
        public Database banco;

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
            var result = await banco.CollectionJogadores.DeleteOneAsync(x => x.Id == user.Id);
            if (result.DeletedCount >= 1)
                await ctx.ResponderAsync("usuario deletado!");
            else
                await ctx.ResponderAsync("não foi possível deletar! Tente novamente");
        }

        [Command("criar-mapa")]
        [Description("Permite transforma o canal atual em um mapa.")]
        [Usage("criar-mapa [ coordenada x ] [ coordenada y ] [ tipo: < Cidade, Vila, Aldeia, Floresta > ]")]
        [Example("criar-mapa 5 10 Cidade", "Transforma o canal atual em um mapa com as informações fornecidas..")]
        [RequireOwner]
        public async Task CriarMapaAsync(CommandContext ctx, double x = 0, double y = 0, string tipo = "")
        {
            await ctx.TriggerTypingAsync();
            var result = await banco.CollectionMaps.Find(x => x.Id == ctx.Channel.Id).FirstOrDefaultAsync();
            if (result != null)
            {
                await ctx.ResponderAsync("já existe um mapa neste canal!");
                return;
            }

            tipo = tipo.RemoverAcentos();
            tipo = Regex.Replace(tipo, @"\s+", "");
            if (Enum.TryParse<MapType>(tipo, true, out var mapType))
            {
                var mapa = new WafclastMapa(ctx.Channel.Id, mapType);
                mapa.Coordinates = new WafclastCoordinates(x, y);
                await banco.CollectionMaps.InsertOneAsync(mapa);
                await ctx.ResponderAsync($"mapa {Formatter.InlineCode(ctx.Channel.Name)} criado! Nas coordenadas {Formatter.Bold($"({x} | {y})")} com o tipo {Formatter.Bold(mapType.ToString())}.");
            }
            else
                await ctx.RespondAsync($"{ctx.User.Mention}, você informou um tipo inexistente!");
        }

        [Command("monstro-criar")]
        [Description("Permite criar um monstro para o mapa atual.")]
        [Usage("monstro-criar [ nome ]")]
        [Example("monstro-criar Zombiie Grandioso", "Cria um monstro com o nome fornecido..")]
        [RequireOwner]
        public async Task MonstroCriarAsync(CommandContext ctx, [RemainingText] string nome = "")
        {
            await ctx.TriggerTypingAsync();
            var map = await banco.CollectionMaps.Find(x => x.Id == ctx.Channel.Id).FirstOrDefaultAsync();
            if (map == null)
            {
                await ctx.ResponderAsync("não existe um mapa no canal atual!");
                return;
            }

            var monstro = new WafclastMonster(ctx.Channel.Id, (ulong)map.QuantidadeMonstros + 1) { Nome = nome };
            await banco.CollectionMonsters.InsertOneAsync(monstro);

            map.QuantidadeMonstros++;
            await banco.CollectionMaps.ReplaceOneAsync(x => x.Id == map.Id, map);

            await ctx.ResponderAsync($"monstro {Formatter.Bold(nome)} criado! Pode ser encontrado com o #ID {monstro.MonsterId}.");
        }

        [Command("item-criar")]
        [Description("Permite criar um item.")]
        [Usage("item-criar")]
        [RequireOwner]
        public async Task ItemEditarAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            banco.StartExecutingInteractivity(ctx.User.Id);

            var time = TimeSpan.FromMinutes(10);

            await ctx.ResponderNegritoAsync("Ok! Vamos criar um item...");

            var str = new StringBuilder();
            var item = new WafclastBaseItem();

            foreach (var itens in Enum.GetValues(typeof(ItemType)).Cast<ItemType>())
                str.Append($"`{itens.GetEnumDescription()}[{(int)itens}]` ");

            item.Type = await ctx.WaitForEnumAsync<ItemType>($"Informe um TIPO: {str.ToString()}", time);
            item.ItemID = await banco.FindLastItem(ctx);
            item.Name = await ctx.WaitForStringAsync("Informe um NOME:", time);
            item.Level = await ctx.WaitForIntAsync("Informe o NÍVEL minimo:", time);
            item.Price = await ctx.WaitForIntAsync("Informe um PREÇO de compra, o valor de venda será a metade:", time);

            if ((await ctx.WaitForStringAsync("É possível vender?: Sim ou Não", time)).ToLower() == "nao")
                item.CanSell = false;

            if ((await ctx.WaitForStringAsync("É possível empilhar?: Sim ou Não", time)).ToLower() == "nao")
                item.CanStack = false;

            item.ImageURL = await ctx.WaitForStringAsync("Informe uma URL para imagem:", time);
            item.Description = await ctx.WaitForStringAsync("Informe uma descrição:", time);

            var embed = ItemBuilder(item);
            switch (item.Type)
            {
                case ItemType.Food:
                    WafclastFood comida = new WafclastFood(item);
                    comida.LifeGain = await ctx.WaitForIntAsync("Valor de cura:", time);
                    await banco.InsertItemAsync(comida);

                    embed.AddField("Cura".Titulo(), comida.LifeGain.ToString());
                    break;
                default:

                    await banco.InsertItemAsync(item);
                    break;
            }
            banco.StopExecutingInteractivity(ctx.User.Id);

            await ctx.RespondAsync("Feito!", embed.Build());
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

        [Command("monstro-drop")]
        [Description("Permite editar um drop.")]
        [Usage("monstro-criar [ posicao ] [ item id ] [ chance ] [ quant. min ] [ quant. max] [ monstro id]")]
        [Example("monstro-drop 1 1 0.5 1 1 1", "Adiciona um drop com as informações fornecidas..")]
        [RequireOwner]
        public async Task MonstroDropAsync(CommandContext ctx, int posicao, ulong itemId, double chance, int quantMin, int quantMax, ulong monstroId)
        {
            await ctx.TriggerTypingAsync();
            var map = await banco.CollectionMaps.Find(x => x.Id == ctx.Channel.Id).FirstOrDefaultAsync();
            if (map == null)
            {
                await ctx.ResponderAsync("não existe um mapa no canal atual!");
                return;
            }

            var monster = await banco.CollectionMonsters.Find(x => x.Id == $"{ctx.Channel.Id}:{monstroId}").FirstOrDefaultAsync();
            if (monster == null)
            {
                await ctx.ResponderAsync("não existe um monstro com o #ID informado!");
                return;
            }

            var item = await banco.FindItemByItemIdAsync(itemId, 0);
            if (item == null)
            {
                await ctx.ResponderAsync("não existe um item com o #ID informado!");
                return;
            }

            var embed = new DiscordEmbedBuilder();
            embed.AddField("Item".Titulo(), item.Name);
            embed.AddField("Monstro".Titulo(), monster.Nome);
            embed.AddField("Chance".Titulo(), (chance * 100).ToString());
            embed.AddField("Quantia".Titulo(), $"{quantMin} ~ {quantMax}");

            if (monster.Drops.Count > posicao)
                monster.Drops[posicao] = new ItemChance(item.Id, chance, quantMin, quantMax);
            else
                monster.Drops.Add(new ItemChance(item.Id, chance, quantMin, quantMax));
            await banco.CollectionMonsters.ReplaceOneAsync(x => x.Id == monster.Id, monster);

            await ctx.ResponderAsync($"monstro {Formatter.Bold(monster.Nome)} editado! Agora pode cair {Formatter.Bold(item.Name)}.");
        }


        [Command("monstro-atributos")]
        [Description("Permite editar os atributos de um monstro já criado no mapa atual.")]
        [Usage("monstro-atributos [ forca min ] [ forca max ] [ resistencia min ] [ resistencia max ] [ agilidade min ] [ agilidade max ] [ exp min] [ exp max ] [ tempo ] [ s | m | h | d ] [ id ]")]
        [Example("monstro-atributos 4 5 6 7 2 3 2 3 1 m 1", "Edita o monstro de #ID 1 com os parâmetros fornecidos.")]
        [RequireOwner]
        public async Task MonstrosAtributosAsync(CommandContext ctx, int forcaMin = 0, int forcaMax = 0,
                                                 int resistenciaMin = 0, int resistenciaMax = 0, int agilidadeMin = 0,
                                                 int agilidadeMax = 0, decimal expMin = 0, decimal expMax = 0,
                                                 int tempo = 1, string duracao = "m", ulong id = 0)
        {
            await ctx.TriggerTypingAsync();
            var monster = await banco.CollectionMonsters.Find(x => x.Id == $"{ctx.Channel.Id}:{id}").FirstOrDefaultAsync();
            if (monster == null)
            {
                await ctx.ResponderAsync("não existe um monstro com o #ID informado!");
                return;
            }

            monster.Atributos = new WafclastMonsterAtributos(forcaMin, forcaMax, resistenciaMin, resistenciaMax, agilidadeMin, agilidadeMax, expMin, expMax);
            monster.CalcAtributos();
            switch (duracao)
            {
                case "s":
                    monster.RespawnTime = TimeSpan.FromSeconds(tempo);
                    break;
                case "m":
                    monster.RespawnTime = TimeSpan.FromMinutes(tempo);
                    break;
                case "h":
                    monster.RespawnTime = TimeSpan.FromHours(tempo);
                    break;
                case "d":
                    monster.RespawnTime = TimeSpan.FromDays(tempo);
                    break;
                default:
                    monster.RespawnTime = TimeSpan.FromMinutes(tempo);
                    break;
            }

            await banco.CollectionMonsters.ReplaceOneAsync(x => x.Id == monster.Id, monster);
            var embed = new DiscordEmbedBuilder();
            embed.WithTitle(monster.Nome.Titulo());
            embed.AddField("Força".Titulo(), $"{monster.Atributos.ForcaMin} ~ {monster.Atributos.ForcaMax}");
            embed.AddField("Resistencia".Titulo(), $"{monster.Atributos.ResistenciaMin} ~ {monster.Atributos.ResistenciaMax}");
            embed.AddField("Agilidade".Titulo(), $"{monster.Atributos.AgilidadeMin} ~ {monster.Atributos.AgilidadeMax}");
            embed.AddField("Experiencia".Titulo(), $"{monster.Atributos.ExpMin} ~ {monster.Atributos.ExpMax}");
            embed.AddField("Respawn a cada".Titulo(), $"{monster.RespawnTime}");
            await ctx.ResponderAsync($"monstro editado!", embed.Build());
        }

        [Command("atualizar")]
        [RequireOwner]
        public async Task AtualizarAsync(CommandContext ctx)
        {
            FilterDefinition<WafclastPlayer> filter = FilterDefinition<WafclastPlayer>.Empty;
            FindOptions<WafclastPlayer> options = new FindOptions<WafclastPlayer>
            {
                BatchSize = 8,
                NoCursorTimeout = false
            };

            using (IAsyncCursor<WafclastPlayer> cursor = await banco.CollectionJogadores.FindAsync(filter, options))
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<WafclastPlayer> usuarios = cursor.Current;

                    foreach (WafclastPlayer player in usuarios)
                    {
                        player.Deaths = 0;

                        await banco.CollectionJogadores.ReplaceOneAsync(x => x.Id == player.Id, player);
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
    }
}
