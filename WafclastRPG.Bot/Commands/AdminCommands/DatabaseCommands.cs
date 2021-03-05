using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Database;
using WafclastRPG.Bot.Extensions;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Bot.Commands.AdminCommands
{
    public class DatabaseCommands : BaseCommandModule
    {
        public BotDatabase banco;

        [Command("deletar-user")]
        [Description("Permite deletar o usuario informado.")]
        [Usage("deletar-user [ @menção | id ]")]
        [Example("deletar-user @talion", "Deleta o usuario mencionado.")]
        [Example("deletar-user 1239485", "Deleta o usuario informado.")]
        [RequireOwner]
        public async Task DeletarUserAsync(CommandContext ctx, DiscordUser user)
        {
            await ctx.TriggerTypingAsync();
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
    }
}
