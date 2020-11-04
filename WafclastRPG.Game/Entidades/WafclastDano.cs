using System;
using System.Collections.Generic;
using System.Text;

namespace WafclastRPG.Game.Entidades
{
   public class WafclastDano
    {
        public double Minimo { get; set; }
        public double Maximo { get; set; }
        public double Sortear { get { return Calculo.SortearValor(Minimo, Maximo); } }

        public WafclastDano(double minimo, double maximo)
        {
            Minimo = minimo;
            Maximo = maximo;
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
