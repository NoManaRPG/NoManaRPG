using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class CommandsCommand : BaseCommandModule
    {
        [Command("comandos")]
        [Aliases("commands")]
        [Description("Exibe todos os comandos que o bot reconhece.")]
        [Usage("comandos")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task CommandsAsync(CommandContext ctx)
        {
            var str = new StringBuilder();
            str.AppendLine();
            str.AppendLine("[Mercado Geral]");
            str.Append("mgcriarvenda, ");
            str.Append("mgvervendas, ");
            str.Append("mgcomprar, ");
            str.Append("mglista, ");
            str.Append("mgparar, ");

            str.AppendLine();
            str.AppendLine("[Geral]");
            str.Append("comandos, ");
            str.Append("ajuda, ");
            str.Append("info, ");
            str.Append("admin, ");

            str.AppendLine();
            str.AppendLine("[Habilidades]");
            str.Append("minerar, ");
            str.Append("cozinhar, ");

            str.AppendLine();
            str.AppendLine("[Usuário]");
            str.Append("explorar, ");
            str.Append("atacar, ");
            str.Append("status, ");
            str.Append("inventario, ");
            str.Append("habilidades, ");
            str.Append("subir, ");
            str.Append("descer, ");
            str.Append("dar, ");
            str.Append("despertador, ");
            str.Append("atributos, ");
            str.Append("atribuir, ");
            str.Append("absorver, ");
            str.Append("examinar, ");

            await ctx.RespondAsync(Formatter.BlockCode(str.ToString(), "css"));
        }
    }
}
