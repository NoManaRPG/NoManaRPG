using System.Globalization;
using System.Threading.Tasks;
using WafclastRPG.Bot.Database;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Bot.Entidades
{
    public class BotJogador : WafclastPlayer
    {
        private readonly BotDatabaseSession banco;

        public BotJogador(WafclastPlayer jogador, BotDatabaseSession banco) : base(jogador)
        {
            this.banco = banco;
        }

        public Task SaveAsync() => this.banco.ReplacePlayerAsync(new WafclastPlayer(this));
        public string Mention()
            => $"<@{Id.ToString(CultureInfo.InvariantCulture)}>";

        public static string UserMention(ulong id)
            => $"<@{id.ToString(CultureInfo.InvariantCulture)}>";
    }
}
