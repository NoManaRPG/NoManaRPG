// This file is part of the WafclastRPG project.

using System;

namespace WafclastRPG.Game.Entities.Wafclast
{
    public class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector() { }
        public Vector(int x, int y) : this((double)x, (double)y) { }
        public Vector(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double Distance(Vector vector)
        {
            var mat1 = Math.Pow(this.X - vector.X, 2) + Math.Pow(this.Y - vector.Y, 2);
            var result = Math.Sqrt(mat1);
            return result;
        }
    }
}
