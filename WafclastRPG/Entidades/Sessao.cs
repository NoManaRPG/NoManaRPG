using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Game.Entidades;

namespace WafclastRPG.Bot.Entidades
{
    public class Sessao
    {
        public WafclastJogador Jogador { get; }

        public SemaphoreSlim Balde { get; }

        public Sessao(WafclastJogador jogador, SemaphoreSlim balde)
        {
            this.Jogador = jogador;
            this.Balde = balde;
        }

        public Task Esperar()
            => this.Balde.WaitAsync();

        public void Soltar()
            => this.Balde.Release();
    }
}
