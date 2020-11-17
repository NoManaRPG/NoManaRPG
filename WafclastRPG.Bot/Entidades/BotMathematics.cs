using System;
using System.Collections.Generic;
using System.Threading;

namespace WafclastRPG.Game
{
    public class BotMathematics
    {
        private int seed = Environment.TickCount;

        readonly ThreadLocal<Random> rd;

        public BotMathematics()
        {
            rd = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));
        }

        public bool Chance(double probabilidade)
            => rd.Value.NextDouble() < probabilidade;
        public int Sortear(int max)
            => rd.Value.Next(0, max);

        public int Sortear(int min, int max)
            => rd.Value.Next(min, 1 + max);

        public double Sortear(double min, double max)
            => rd.Value.NextDouble() * (max - min) + min;

        public T Sortear<T>(List<T> lista)
        {
            var index = Sortear(lista.Count);
            return lista[index];
        }
    }
}
