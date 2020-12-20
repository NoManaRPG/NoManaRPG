using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Game.Entidades;

namespace WafclastRPG.Bot.Entidades
{
    public class BotJogador : WafclastJogador
    {
        private readonly Banco banco;
        private readonly DiscordUser user;

        public BotJogador(WafclastJogador jogador, Banco banco, DiscordUser user) : base(jogador)
        {
            this.banco = banco;
            this.user = user;
        }

        public DiscordEmbedBuilder CriarEmbed()
        {
            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{this.user.Username} [Nv.{this.Personagem.NivelCombate}]", this.user.AvatarUrl);
            embed.WithFooter("Se estiver perdido, utilize o comando ajuda.");
            return embed;
        }

        public Task<WafclastRegiao> GetRegiaoAsync() => banco.GetRegiaoAsync(Personagem.RegiaoId);
        public Task Salvar() => this.banco.ReplaceJogadorAsync(this.Id, new WafclastJogador(this));
    }
}
