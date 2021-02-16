using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Bot.Entidades
{
    public class BotJogador : WafclastPlayer
    {
        private readonly Banco banco;
        private readonly DiscordUser user;
        private DiscordEmbedBuilder embed;

        public BotJogador(WafclastPlayer jogador, Banco banco, DiscordUser user) : base(jogador)
        {
            this.banco = banco;
            this.user = user;
        }

        public DiscordEmbedBuilder NewEmbed()
        {
            embed.WithAuthor(user.Username, iconUrl: user.AvatarUrl);
            return embed;
        }

        public Task Salvar() => this.banco.ReplaceJogadorAsync(this.Id, new WafclastPlayer(this));
    }
}
