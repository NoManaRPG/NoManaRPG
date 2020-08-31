using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Eventos
{
    public static class VoiceStateUpdated
    {
        public static Task EventAsync(VoiceStateUpdateEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
