using System;

namespace WafclastRPG.Game
{
    public class Formulas
    {
        private Random rd;

        public Formulas()
        {
            rd = new Random();
        }

        public bool Chance(double probabilidade)
            => rd.NextDouble() < probabilidade;
        public int Sortear(int max)
            => rd.Next(0, max);

        public int Sortear(int min, int max)
            => rd.Next(min, 1 + max);

        public double Sortear(double min, double max)
            => rd.NextDouble() * (max - min) + min;

        public decimal Sortear(decimal max)
            => (decimal)(rd.NextDouble() * (double)max);
    }
}
