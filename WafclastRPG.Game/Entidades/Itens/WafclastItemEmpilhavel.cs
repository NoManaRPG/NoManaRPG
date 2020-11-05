namespace WafclastRPG.Game.Entidades.Itens
{
    public class WafclastItemEmpilhavel : WafclastItem
    {
        public WafclastItemEmpilhavel(int id, string nome, int ocupaEspaco)
            : base(id, nome, ocupaEspaco)
        {
            this.Pilha = 1;
        }

        public int Pilha { get; private set; }

        public void AddPilha(int quantidade) => this.Pilha += quantidade;
    }
}
