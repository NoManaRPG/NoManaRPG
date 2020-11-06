using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades;

namespace WafclastRPG.Bot.Entidades
{
    public class Sessao
    {
        public WafclastJogador Jogador { get; }

        public SemaphoreSlim Balde { get; }

        public Banco Banco { get; }

        public Sessao(WafclastJogador jogador, SemaphoreSlim balde, Banco banco)
        {
            this.Jogador = jogador;
            this.Balde = balde;
            this.Banco = banco;
        }

        public Task Segurar()
            => this.Balde.WaitAsync();

        public void Soltar()
            => this.Balde.Release();

        /// <summary>
        /// Salva o jogador e libera o balde.
        /// </summary>
        public async Task Salvar()
        {
            await Banco.ReplaceJogadorAsync(Jogador.Id, Jogador);
            Balde.Release();
        }
    }
}
