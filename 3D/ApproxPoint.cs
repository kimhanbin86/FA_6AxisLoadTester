using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media.Media3D;
// http://csharphelper.com/blog/2014/10/use-a-dictionary-to-draw-a-3d-menger-sponge-fractal-more-efficiently-using-wpf-xaml-and-c/ 

namespace _3D
{
    // A class to represent approximate points.
    public class ApproxPoint : IComparable<ApproxPoint>
    {
        public double X, Y, Z;
        public ApproxPoint(double x, double y, double z)
        {
            X = Math.Round(x, 3); // this is the key to approximating
			Y = Math.Round(y, 3);
            Z = Math.Round(z, 3);
        }
        public ApproxPoint(Point3D point)
            : this(point.X, point.Y, point.Z) { }

        public bool Equals(ApproxPoint point)
        {
            return ((X == point.X) && (Y == point.Y) && (Z == point.Z));
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is ApproxPoint)) return false;
            return this.Equals(obj as ApproxPoint);
        }

        public static bool operator ==(ApproxPoint point1, ApproxPoint point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(ApproxPoint point1, ApproxPoint point2)
        {
            return !point1.Equals(point2);
        }

        public override int GetHashCode()
        {
            int hashx = X.GetHashCode() << 3;
            int hashy = Y.GetHashCode() << 5;
            int hashz = Z.GetHashCode();
            int result = hashx ^ hashy ^ hashz;
            return result;
        }
        public int CompareTo(ApproxPoint other)
        {
            if (X < other.X) return -1;
            if (X > other.X) return 1;
            if (Y < other.Y) return -1;
            if (Y > other.Y) return 1;
            if (Z < other.Z) return -1;
            if (Z > other.Z) return 1;
            return 0;
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }
    }
}
