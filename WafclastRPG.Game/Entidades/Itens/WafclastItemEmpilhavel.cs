using WafclastRPG.Game.Enuns;

namespace WafclastRPG.Game.Entidades.Itens
{
    public class WafclastItemEmpilhavel : WafclastItem
    {
        public WafclastItemEmpilhavel(string nome, WafclastTipo tipo, int ocupaEspaco)
            : base(nome, tipo, ocupaEspaco)
        {
            this.Pilha = 1;
        }

        public int Pilha { get; private set; }

        public void AddPilha(int quantidade)
        {
            this.Pilha += quantidade;
        }
    }
}
