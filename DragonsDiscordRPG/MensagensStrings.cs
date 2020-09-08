using DSharpPlus.CommandsNext;
using System.Threading.Tasks;

namespace DragonsDiscordRPG
{
    public static class MensagensStrings
    {
        public static Task ComandoSendoProcessado(CommandContext ctx) => ctx.RespondAsync($"{ctx.User.Mention}, o ultimo comando ainda não foi processado! Aguarde!");
        public static Task PersonagemJaExiste(CommandContext ctx) => ctx.RespondAsync($"{ctx.User.Mention}, você já criou um personagem e por isso não pode usar este comando novamente!");
    }
}
