using System;
using System.Collections.Generic;
using System.Text;

namespace DragonsDiscordRPG
{
    public static class Calculo
    {
        public static double CalcularEfetividadeXP(double playerLevel, double enemyLevel)
        {
            double efetividade = Math.Abs(playerLevel - enemyLevel);
            if (efetividade > 4)
            {
                efetividade -= 4;
                return Math.Pow(playerLevel + 5, 1.5) / Math.Pow(playerLevel + 5 + Math.Pow(efetividade, 2.5), 1.5);
            }
            return 1;
        }

        public static bool DanoFisicoChanceAcerto(double atacantePrecisao, double defensorEvasao)
        {
            return Chance(Math.Clamp(atacantePrecisao / (atacantePrecisao + Math.Pow(defensorEvasao / 4, 0.8)), 0.05, 0.95));
        }

        public static bool Chance(double probabilidade)
        {
            Random rd = new Random();
            return rd.NextDouble() < probabilidade;
        }

        //Se aplica %de dano reduzido extra
        private static double ReduzirDanoFisico(double dano, double armadura)
        {
            return armadura / (armadura + 10 * dano);
        }

        public static double DanoFisicoCalcular(double dano, double armadura)
        {
            double porcentagemReducao = Math.Clamp(ReduzirDanoFisico(dano, armadura), 0, 0.9);
            porcentagemReducao = dano * porcentagemReducao;
            return dano - porcentagemReducao;
        }

        public static int SortearValor(int min, int max)
        {
            Random rd = new Random();
            return rd.Next(min, 1 + max);
        }

        public static double SortearValor(double min, double max)
        {
            Random rd = new Random();
            return rd.NextDouble() * (max - min) + min;
        }

    }
}
