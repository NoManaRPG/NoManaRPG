using System;

namespace WafclastRPG.Bot.Extensions
{
    public static class RandomExtension
    {
        public static bool Chance(this Random rd, double chance)
            => rd.NextDouble() < chance;
        public static int Sortear(this Random rd, int max)
            => rd.Next(0, max);

        public static int Sortear(this Random rd, int min, int max)
            => rd.Next(min, 1 + max);

        public static double Sortear(this Random rd, double min, double max)
            => rd.NextDouble() * (max - min) + min;

        public static decimal Sortear(this Random rd, decimal max)
            => (decimal)(rd.NextDouble() * (double)max);
    }
}
