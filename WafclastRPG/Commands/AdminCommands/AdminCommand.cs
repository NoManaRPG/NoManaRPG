using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;

namespace WafclastRPG.Commands.AdminCommands {
  class AdminCommand : BaseCommandModule {
    [Command("admin")]
    [Description("Exibe todos os comandos que o bot reconhece de administrador.")]
    [Usage("admin")]
    [RequireOwner]
    public async Task CommandsAsync(CommandContext ctx) {
      var str = new StringBuilder();
      str.AppendLine("[Admin]");

      str.AppendLine();
      str.AppendLine("[Monstros]");
      str.Append("vermonstro, ");

      str.AppendLine();
      str.AppendLine("[Itens]");
      str.Append("atualizar-itens, ");
      str.Append("criarfabricacao, ");
      str.Append("itemEC, ");
      str.Append("itensV, ");

      str.AppendLine();
      str.AppendLine("[Jogadores]");
      str.Append("atualizar-jogadores, ");
      str.Append("deletarU, ");
      str.Append("additem, ");
      await ctx.RespondAsync(Formatter.BlockCode(str.ToString(), "css"));
    }
  }
}
