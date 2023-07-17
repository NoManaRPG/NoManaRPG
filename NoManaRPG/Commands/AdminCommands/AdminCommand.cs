// This file is part of NoManaRPG project.

using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using NoManaRPG.Attributes;

namespace NoManaRPG.Commands.AdminCommands;

public class AdminCommand : BaseCommandModule
{
    [Command("admin")]
    [Description("Exibe todos os comandos que o bot reconhece de administrador.")]
    [Usage("admin")]
    [RequireOwner]
    [Hidden]
    public async Task CommandsAsync(CommandContext ctx)
    {
        var str = new StringBuilder();
        str.AppendLine("[Admin]");

        str.AppendLine();
        str.AppendLine("[Monstros]");
        str.Append("vermonstro, ");

        str.AppendLine();
        str.AppendLine("[Room]");
        str.Append("setdescription, ");
        str.Append("new, ");

        str.AppendLine();
        str.AppendLine("[Itens]");
        str.Append("atualizar-itens, ");
        str.Append("criarfabricacao, ");
        await ctx.RespondAsync(Formatter.BlockCode(str.ToString(), "css"));
    }
}
