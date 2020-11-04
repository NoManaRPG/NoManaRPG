namespace WafclastRPG.Game.Entidades
{
    public class WafclastDano
    {
        public double Minimo { get; set; }
        public double Maximo { get; set; }

        public WafclastDano() { }

        public WafclastDano(double minimo, double maximo)
        {
            Minimo = minimo;
            Maximo = maximo;
        }

        public WafclastDano(WafclastDano dano)
        {
            this.Minimo = dano.Minimo;
            this.Maximo = dano.Maximo;
        }

        public void Somar(WafclastDano dano)
        {
            Minimo += dano.Minimo;
            Maximo += dano.Maximo;
        }

        public void Subtrair(WafclastDano dano)
        {
            Minimo -= dano.Minimo;
            Maximo -= dano.Maximo;
        }
    }
}
