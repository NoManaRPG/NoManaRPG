using System.Threading.Tasks;
using WafclastRPG.Game.Entidades;

namespace WafclastRPG.Bot.Entidades
{
    public class BotJogador : WafclastJogador
    {
        private readonly Banco banco;

        public BotJogador(WafclastJogador jogador, Banco banco) : base(jogador)
        {
            this.banco = banco;
        }

        public Task<WafclastRegiao> GetRegiaoAsync()
            => banco.GetRegiaoAsync(Personagem.RegiaoId);

        public Task Salvar() => this.banco.ReplaceJogadorAsync(this.Id, new WafclastJogador(this));
    }
}
