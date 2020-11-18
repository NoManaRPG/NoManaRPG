using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Game;

namespace WafclastRPG.Bot.Commands.UserCommands
{
    public class ComandoSaquear : BaseCommandModule
    {
        public Banco banco;

        [Command("saquear")]
        [Description("Permite saquear os itens de um monstro morto.")]
        [Usage("saquear")]
        public async Task ComandoSaquearAsync(CommandContext ctx)
        {
            await Task.CompletedTask;
        }
    }
}
