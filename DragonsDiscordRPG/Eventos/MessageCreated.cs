using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Eventos
{
    public static class MessageCreated
    {
        public static Task EventAsync(MessageCreateEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
