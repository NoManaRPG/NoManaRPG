using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace TorreRPG.Eventos
{
    public static class MessageCreated
    {
        public static Task EventAsync(MessageCreateEventArgs e)
        {
            if (e.Channel.Id == 737727073828995123)
                return null;
            return Task.CompletedTask;
        }
    }
}
