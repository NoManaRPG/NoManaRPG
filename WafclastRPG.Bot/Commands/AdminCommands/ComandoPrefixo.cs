using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Entidades;

namespace WafclastRPG.Bot.Comandos.Acao
{
    public class ComandoPrefixo : BaseCommandModule
    {

        public Banco banco;

        [Command("prefixo")]
        [Description("Define um prefixo customizado para o servidor atual.")]
        [Cooldown(1, 5, CooldownBucketType.Guild)]
        [RequireUserPermissions(Permissions.Administrator)]
        [Example("prefixo !", "Define o prefixo do servidor para *!*.")]
        [Example("prefixo", "Volta ao prefixo original do bot.")]
        [Usage("prefixo [ prefix ]")]
        public async Task ComandoPrefixoAsync(CommandContext ctx, string prefixo = "")
        {
            if (string.IsNullOrWhiteSpace(prefixo))
            {
                await banco.Servidores.DeleteOneAsync(x => x.Id == ctx.Guild.Id);
                await ctx.RespondAsync("Feito!");
                return;
            }

            if (prefixo.Length > 3)
            {
                await ctx.RespondAsync("Prefixo não pode ter mais que 3 letras!");
                return;
            }

            var server = new BotServidor(ctx.Guild.Id);
            server.SetPrefix(prefixo);
            await banco.Servidores.ReplaceOneAsync(x => x.Id == server.Id, server, new ReplaceOptions { IsUpsert = true });
            await ctx.RespondAsync("Feito!");
        }
    }
}
