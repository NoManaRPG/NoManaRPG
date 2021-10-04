// This file is part of the WafclastRPG project.

using System;

namespace WafclastRPG.Game.Entities.Rooms
{
    public class WafclastVector
    {
        public double X { get; set; }
        public double Y { get; set; }

        public WafclastVector() { }
        public WafclastVector(int x, int y) : this((double)x, (double)y) { }
        public WafclastVector(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double Distance(WafclastVector vector)
        {
            var mat1 = Math.Pow(this.X - vector.X, 2) + Math.Pow(this.Y - vector.Y, 2);
            var result = Math.Sqrt(mat1);
            return result;
        }
    }
}
