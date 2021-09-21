using System;

namespace WafclastRPG.Entities.Wafclast {
  public class Vector {
    public double X { get; set; }
    public double Y { get; set; }

    public Vector() { }
    public Vector(int x, int y) {
      X = x;
      Y = y;
    }

    public double Distance(Vector vector) {
      var mat1 = Math.Pow(X - vector.X, 2) + Math.Pow(Y - vector.Y, 2);
      var result = Math.Sqrt(mat1);
      return result;
    }
  }
}
