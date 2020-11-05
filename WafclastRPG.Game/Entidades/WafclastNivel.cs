using System;
using System.Collections.Generic;
using System.Text;

namespace WafclastRPG.Game.Entidades
{
    public class WafclastNivel
    {
        public int Atual { get; private set; }
        public double ExpAtual { get; private set; }
        public double ExpMax { get; private set; }

        public WafclastNivel()
        {
            ExpAtual = 0;
            Atual = 1;
            ExpMax = 525;
        }

        public void Penalizar() => ExpAtual *= 0.9;

        public int AddExp(double exp)
        {
            double expResultante = ExpAtual + exp;
            int quantEvo = 0;
            if (expResultante >= ExpMax)
            {
                do
                {
                    double quantosPrecisaProxNivel = expResultante - ExpMax;
                    Evoluir();
                    quantEvo++;
                    expResultante = quantosPrecisaProxNivel;
                } while (expResultante >= ExpMax);
                ExpAtual += expResultante;
                return quantEvo;
            }
            ExpAtual += exp;
            return quantEvo;
        }

        private void Evoluir()
        {
            Atual++;
            ExpMax *= 1.07;
            ExpAtual = 0;
        }
    }
}
