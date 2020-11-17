using System;

namespace WafclastRPG.Game
{
    public class Mathematics
    {
        private Random rd = new Random();

        public bool Chance(double probabilidade)
        {
            return rd.NextDouble() < probabilidade;
        }

        public int Sortear(int min, int max)
        {
            return rd.Next(min, 1 + max);
        }

        public double Sortear(double min, double max)
        {
            return rd.NextDouble() * (max - min) + min;
        }
    }
}
