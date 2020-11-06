using System;

namespace WafclastRPG.Game.Entidades.Itens
{
    public class WafclastItemBebida : WafclastItemEmpilhavel
    {
        public double SedeRestaura { get; set; }
        public WafclastItemBebida(string nome, int ocupaEspaco, double precoCompra,
            double sedeRestaura) : base(nome, ocupaEspaco, precoCompra)
        {
            this.SedeRestaura = sedeRestaura;
        }
    }
}
