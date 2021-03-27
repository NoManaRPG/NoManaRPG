using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;

namespace WafclastRPG.Comandos.Acao
{
    public class ComandoPrefixo : BaseCommandModule
    {

        public Database banco;

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
                await banco.DeleteServerAsync(ctx.Guild.Id);
                await ctx.RespondAsync("Feito!");
                return;
            }

            if (prefixo.Length > 3)
            {
                await ctx.RespondAsync("Prefixo não pode ter mais que 3 letras!");
                return;
            }

            var server = new WafclastServer(ctx.Guild.Id);
            server.SetPrefix(prefixo);
            await banco.ReplaceServerAsync(server.Id, server);
            await ctx.RespondAsync("Feito!");
        }
    }
}
