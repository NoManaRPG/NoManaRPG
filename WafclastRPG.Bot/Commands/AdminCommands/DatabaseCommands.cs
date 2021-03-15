using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WafclastRPG.Bot.Attributes;
using WafclastRPG.Bot.Extensions;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Bot.Commands.AdminCommands
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
            if (Enum.TryParse<WafclastMapaType>(tipo, true, out var mapType))
            {
                var mapa = new WafclastMapa(ctx.Channel.Id, mapType);
                mapa.Coordinates = new WafclastCoordinates(x, y);
                await banco.CollectionMaps.InsertOneAsync(mapa);
                await ctx.ResponderAsync($"mapa {Formatter.InlineCode(ctx.Channel.Name)} criado! Nas coordenadas {Formatter.Bold($"({x} | {y})")} com o tipo {Formatter.Bold(mapType.ToString())}.");
            }
            else
                await ctx.RespondAsync($"{ctx.User.Mention}, você informou um tipo inexistente!");
        }

        [Command("criar-monstro")]
        [Description("Permite criar um monstro para o mapa atual.")]
        [Usage("criar-monstro [ id ] [ nome ] [ defesa ] [ ataque ] [ vida ] [ exp ] [ spawn time ]")]
        [Example("criar-monstro 1 Zombiee 5 1 20 3 4", "Cria um monstro com as informações fornecidas..")]
        [RequireOwner]
        public async Task CriarMonstroAsync(CommandContext ctx, ulong id = 0, string nome = "", decimal defesa = 0, decimal ataque = 0, decimal vidaMaxima = 0, decimal exp = 0, int tempo = 0)
        {
            await ctx.TriggerTypingAsync();
            var result = await banco.CollectionMaps.Find(x => x.Id == ctx.Channel.Id).FirstOrDefaultAsync();
            if (result == null)
            {
                await ctx.ResponderAsync("não existe um mapa no canal atual!");
                return;
            }

            var monstro = new WafclastMonster(ctx.Channel.Id, id, nome, defesa, ataque, vidaMaxima, exp, TimeSpan.FromMinutes(tempo));
            await banco.CollectionMonsters.InsertOneAsync(monstro);

            await ctx.ResponderAsync($"monstro criado!");
        }

        [Command("monstro-reduzir-vida")]
        [RequireOwner]
        public async Task MonstroReduzirVidaAsync(CommandContext ctx, ulong id, decimal quantidade)
        {
            await ctx.TriggerTypingAsync();
            var monster = await banco.CollectionMonsters.Find(x => x.Id == ctx.Channel.Id + id).FirstOrDefaultAsync();

            monster.SetVida(quantidade);

            await banco.CollectionMonsters.ReplaceOneAsync(x => x.Id == ctx.Channel.Id + id, monster);

            await ctx.ResponderAsync($"{monster.Nome} está com {quantidade} de vida!");
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
                        if (player.Character.ExperienciaAtual >= player.Character.ExperienciaProximoNivel)
                            player.Character.ExperienciaAtual = 0;

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
