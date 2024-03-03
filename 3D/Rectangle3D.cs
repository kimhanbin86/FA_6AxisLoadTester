using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// http://csharphelper.com/blog/2014/10/use-a-dictionary-to-draw-a-3d-menger-sponge-fractal-more-efficiently-using-wpf-xaml-and-c/
using System.Windows.Media.Media3D;

namespace _3D
{
    public class Rectangle3D
    {
        // The rectangle's approximate points.
        public ApproxPoint[] APoints;

        // The rectangle's approximate points.
        public Point3D[] Points;

        // Initializing constructor.
        public Rectangle3D(Point3D point1, Point3D point2, Point3D point3, Point3D point4)
        {
            // Save the points.
            Points = new Point3D[]
            {
                point1,
                point2,
                point3,
                point4,
            };

            // Save the approximate points.
            APoints = new ApproxPoint[]
            {
                new ApproxPoint(point1),
                new ApproxPoint(point2),
                new ApproxPoint(point3),
                new ApproxPoint(point4),
            };

            // Sort the approximate points.
            Array.Sort(APoints);
        }

        // Return true if the rectangles
        // contain roughly the same points.
        public bool Equals(Rectangle3D other)
        {
            // Return true if the ApproxPoints are equal.
            for (int i = 0; i < 4; i++)
                if (APoints[i] != other.APoints[i]) return false;
            return true;
        }

        public override bool Equals(Object obj)
        {
            // If parameter is null, return false.
            if (obj == null) return false;

            // If parameter cannot be cast into a Rectangle3D, return false.
            if (!(obj is Rectangle3D)) return false;

            // Invoke the previous version of Equals.
            return this.Equals(obj as Rectangle3D);
        }

        public static bool operator ==(Rectangle3D rect1, Rectangle3D rect2)
        {
            return rect1.Equals(rect2);
        }

        public static bool operator !=(Rectangle3D rect1, Rectangle3D rect2)
        {
            return !rect1.Equals(rect2);
        }

        // Return a hash code.
        public override int GetHashCode()
        {
            int hash0 = APoints[0].GetHashCode() << 3;
            int hash1 = APoints[1].GetHashCode() << 5;
            int hash2 = APoints[2].GetHashCode() << 7;
            int hash3 = APoints[3].GetHashCode();
            int result = hash0 ^ hash1 ^ hash2 ^ hash3;
            return result;
        }
    }
}
