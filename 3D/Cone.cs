using System;
using System.Windows.Media.Media3D;

namespace _3D
{
	public class Cone
	{
		// http://csharphelper.com/blog/2016/05/draw-cones-using-wpf-c/
		// Add a cone.
		public static void AddCone(MeshGeometry3D mesh, Point3D end_point,
			Vector3D axis, double radius1, double radius2, int num_sides)
		{
			// Get two vectors perpendicular to the axis.
			Vector3D top_v1;
			if ((axis.Z < -0.01) || (axis.Z > 0.01))
				top_v1 = new Vector3D(axis.Z, axis.Z, -axis.X - axis.Y);
			else
				top_v1 = new Vector3D(-axis.Y - axis.Z, axis.X, axis.X);
			Vector3D top_v2 = Vector3D.CrossProduct(top_v1, axis);

			Vector3D bot_v1 = top_v1;
			Vector3D bot_v2 = top_v2;

			// Make the vectors have length radius.
			top_v1 *= (radius1 / top_v1.Length);
			top_v2 *= (radius1 / top_v2.Length);

			bot_v1 *= (radius2 / bot_v1.Length);
			bot_v2 *= (radius2 / bot_v2.Length);

			// Make the top end cap.
			double theta = 0;
			double dtheta = 2 * Math.PI / num_sides;
			for (int i = 0; i < num_sides; i++)
			{
				Point3D p1 = end_point +
					Math.Cos(theta) * top_v1 +
					Math.Sin(theta) * top_v2;
				theta += dtheta;
				Point3D p2 = end_point +
					Math.Cos(theta) * top_v1 +
					Math.Sin(theta) * top_v2;
				AddTriangle(mesh, end_point, p1, p2);
			}

			// Make the bottom end cap.
			Point3D end_point2 = end_point + axis;
			theta = 0;
			for (int i = 0; i < num_sides; i++)
			{
				Point3D p1 = end_point2 +
					Math.Cos(theta) * bot_v1 +
					Math.Sin(theta) * bot_v2;
				theta += dtheta;
				Point3D p2 = end_point2 +
					Math.Cos(theta) * bot_v1 +
					Math.Sin(theta) * bot_v2;
				AddTriangle(mesh, end_point2, p2, p1);
			}

			// Make the sides.
			theta = 0;
			for (int i = 0; i < num_sides; i++)
			{
				Point3D p1 = end_point +
					Math.Cos(theta) * top_v1 +
					Math.Sin(theta) * top_v2;
				Point3D p3 = end_point + axis +
					Math.Cos(theta) * bot_v1 +
					Math.Sin(theta) * bot_v2;
				theta += dtheta;
				Point3D p2 = end_point +
					Math.Cos(theta) * top_v1 +
					Math.Sin(theta) * top_v2;
				Point3D p4 = end_point + axis +
					Math.Cos(theta) * bot_v1 +
					Math.Sin(theta) * bot_v2;

				AddTriangle(mesh, p1, p3, p2);
				AddTriangle(mesh, p2, p3, p4);
			}
		}

		// Add a cylinder.
		private void AddCylinder(MeshGeometry3D mesh, Point3D end_point, Vector3D axis, double radius, int num_sides)
		{
			// Get two vectors perpendicular to the axis.
			Vector3D v1;
			if ((axis.Z < -0.01) || (axis.Z > 0.01))
				v1 = new Vector3D(axis.Z, axis.Z, -axis.X - axis.Y);
			else
				v1 = new Vector3D(-axis.Y - axis.Z, axis.X, axis.X);
			Vector3D v2 = Vector3D.CrossProduct(v1, axis);

			// Make the vectors have length radius.
			v1 *= (radius / v1.Length);
			v2 *= (radius / v2.Length);

			// Make the top end cap.
			double theta = 0;
			double dtheta = 2 * Math.PI / num_sides;
			for (int i = 0; i < num_sides; i++)
			{
				Point3D p1 = end_point +
					Math.Cos(theta) * v1 +
					Math.Sin(theta) * v2;
				theta += dtheta;
				Point3D p2 = end_point +
					Math.Cos(theta) * v1 +
					Math.Sin(theta) * v2;
				AddTriangle(mesh, end_point, p1, p2);
			}

			// Make the bottom end cap.
			Point3D end_point2 = end_point + axis;
			theta = 0;
			for (int i = 0; i < num_sides; i++)
			{
				Point3D p1 = end_point2 +
					Math.Cos(theta) * v1 +
					Math.Sin(theta) * v2;
				theta += dtheta;
				Point3D p2 = end_point2 +
					Math.Cos(theta) * v1 +
					Math.Sin(theta) * v2;
				AddTriangle(mesh, end_point2, p2, p1);
			}

			// Make the sides.
			theta = 0;
			for (int i = 0; i < num_sides; i++)
			{
				Point3D p1 = end_point +
					Math.Cos(theta) * v1 +
					Math.Sin(theta) * v2;
				theta += dtheta;
				Point3D p2 = end_point +
					Math.Cos(theta) * v1 +
					Math.Sin(theta) * v2;

				Point3D p3 = p1 + axis;
				Point3D p4 = p2 + axis;

				AddTriangle(mesh, p1, p3, p2);
				AddTriangle(mesh, p2, p3, p4);
			}
		}

		// Add a cylinder with smooth sides.
		private void AddSmoothCylinder(MeshGeometry3D mesh, Point3D end_point, Vector3D axis, double radius, int num_sides)
		{
			// Get two vectors perpendicular to the axis.
			Vector3D v1;
			if ((axis.Z < -0.01) || (axis.Z > 0.01))
				v1 = new Vector3D(axis.Z, axis.Z, -axis.X - axis.Y);
			else
				v1 = new Vector3D(-axis.Y - axis.Z, axis.X, axis.X);
			Vector3D v2 = Vector3D.CrossProduct(v1, axis);

			// Make the vectors have length radius.
			v1 *= (radius / v1.Length);
			v2 *= (radius / v2.Length);

			// Make the top end cap.
			// Make the end point.
			int pt0 = mesh.Positions.Count; // Index of end_point.
			mesh.Positions.Add(end_point);

			// Make the top points.
			double theta = 0;
			double dtheta = 2 * Math.PI / num_sides;
			for (int i = 0; i < num_sides; i++)
			{
				mesh.Positions.Add(end_point +
					Math.Cos(theta) * v1 +
					Math.Sin(theta) * v2);
				theta += dtheta;
			}

			// Make the top triangles.
			int pt1 = mesh.Positions.Count - 1; // Index of last point.
			int pt2 = pt0 + 1;                  // Index of first point in this cap.
			for (int i = 0; i < num_sides; i++)
			{
				mesh.TriangleIndices.Add(pt0);
				mesh.TriangleIndices.Add(pt1);
				mesh.TriangleIndices.Add(pt2);
				pt1 = pt2++;
			}

			// Make the bottom end cap.
			// Make the end point.
			pt0 = mesh.Positions.Count; // Index of end_point2.
			Point3D end_point2 = end_point + axis;
			mesh.Positions.Add(end_point2);

			// Make the bottom points.
			theta = 0;
			for (int i = 0; i < num_sides; i++)
			{
				mesh.Positions.Add(end_point2 +
					Math.Cos(theta) * v1 +
					Math.Sin(theta) * v2);
				theta += dtheta;
			}

			// Make the bottom triangles.
			theta = 0;
			pt1 = mesh.Positions.Count - 1; // Index of last point.
			pt2 = pt0 + 1;                  // Index of first point in this cap.
			for (int i = 0; i < num_sides; i++)
			{
				mesh.TriangleIndices.Add(num_sides + 1);    // end_point2
				mesh.TriangleIndices.Add(pt2);
				mesh.TriangleIndices.Add(pt1);
				pt1 = pt2++;
			}

			// Make the sides.
			// Add the points to the mesh.
			int first_side_point = mesh.Positions.Count;
			theta = 0;
			for (int i = 0; i < num_sides; i++)
			{
				Point3D p1 = end_point +
					Math.Cos(theta) * v1 +
					Math.Sin(theta) * v2;
				mesh.Positions.Add(p1);
				Point3D p2 = p1 + axis;
				mesh.Positions.Add(p2);
				theta += dtheta;
			}

			// Make the side triangles.
			pt1 = mesh.Positions.Count - 2;
			pt2 = pt1 + 1;
			int pt3 = first_side_point;
			int pt4 = pt3 + 1;
			for (int i = 0; i < num_sides; i++)
			{
				mesh.TriangleIndices.Add(pt1);
				mesh.TriangleIndices.Add(pt2);
				mesh.TriangleIndices.Add(pt4);

				mesh.TriangleIndices.Add(pt1);
				mesh.TriangleIndices.Add(pt4);
				mesh.TriangleIndices.Add(pt3);

				pt1 = pt3;
				pt3 += 2;
				pt2 = pt4;
				pt4 += 2;
			}
		}
		// Add a triangle to the indicated mesh.
		// Do not reuse points so triangles don't share normals.
		private static void AddTriangle(MeshGeometry3D mesh, Point3D point1, Point3D point2, Point3D point3)
		{
			// Create the points.
			int index1 = mesh.Positions.Count;
			mesh.Positions.Add(point1);
			mesh.Positions.Add(point2);
			mesh.Positions.Add(point3);

			// Create the triangle.
			mesh.TriangleIndices.Add(index1++);
			mesh.TriangleIndices.Add(index1++);
			mesh.TriangleIndices.Add(index1);
		}

		// Make a thin rectangular prism between the two points.
		// If extend is true, extend the segment by half the
		// thickness so segments with the same end points meet nicely.
		private static void AddSegment(MeshGeometry3D mesh,
			Point3D point1, Point3D point2, Vector3D up)
		{
			AddSegment(mesh, point1, point2, up, false);
		}

		private static void AddSegment(MeshGeometry3D mesh,
			Point3D point1, Point3D point2, Vector3D up,
			bool extend)
		{
			const double thickness = 0.25;

			// Get the segment's vector.
			Vector3D v = point2 - point1;

			if (extend)
			{
				// Increase the segment's length on both ends by thickness / 2.
				Vector3D n = ScaleVector(v, thickness / 2.0);
				point1 -= n;
				point2 += n;
			}

			// Get the scaled up vector.
			Vector3D n1 = ScaleVector(up, thickness / 2.0);

			// Get another scaled perpendicular vector.
			Vector3D n2 = Vector3D.CrossProduct(v, n1);
			n2 = ScaleVector(n2, thickness / 2.0);

			// Make a skinny box.
			// p1pm means point1 PLUS n1 MINUS n2.
			Point3D p1pp = point1 + n1 + n2;
			Point3D p1mp = point1 - n1 + n2;
			Point3D p1pm = point1 + n1 - n2;
			Point3D p1mm = point1 - n1 - n2;
			Point3D p2pp = point2 + n1 + n2;
			Point3D p2mp = point2 - n1 + n2;
			Point3D p2pm = point2 + n1 - n2;
			Point3D p2mm = point2 - n1 - n2;

			// Sides.
			AddTriangle(mesh, p1pp, p1mp, p2mp);
			AddTriangle(mesh, p1pp, p2mp, p2pp);

			AddTriangle(mesh, p1pp, p2pp, p2pm);
			AddTriangle(mesh, p1pp, p2pm, p1pm);

			AddTriangle(mesh, p1pm, p2pm, p2mm);
			AddTriangle(mesh, p1pm, p2mm, p1mm);

			AddTriangle(mesh, p1mm, p2mm, p2mp);
			AddTriangle(mesh, p1mm, p2mp, p1mp);

			// Ends.
			AddTriangle(mesh, p1pp, p1pm, p1mm);
			AddTriangle(mesh, p1pp, p1mm, p1mp);

			AddTriangle(mesh, p2pp, p2mp, p2mm);
			AddTriangle(mesh, p2pp, p2mm, p2pm);
		}

		// Add a cage.
		private static void AddCage(MeshGeometry3D mesh)
		{
			// Top.
			Vector3D up = new Vector3D(0, 1, 0);
			AddSegment(mesh, new Point3D(1, 1, 1), new Point3D(1, 1, -1), up, true);
			AddSegment(mesh, new Point3D(1, 1, -1), new Point3D(-1, 1, -1), up, true);
			AddSegment(mesh, new Point3D(-1, 1, -1), new Point3D(-1, 1, 1), up, true);
			AddSegment(mesh, new Point3D(-1, 1, 1), new Point3D(1, 1, 1), up, true);

			// Bottom.
			AddSegment(mesh, new Point3D(1, -1, 1), new Point3D(1, -1, -1), up, true);
			AddSegment(mesh, new Point3D(1, -1, -1), new Point3D(-1, -1, -1), up, true);
			AddSegment(mesh, new Point3D(-1, -1, -1), new Point3D(-1, -1, 1), up, true);
			AddSegment(mesh, new Point3D(-1, -1, 1), new Point3D(1, -1, 1), up, true);

			// Sides.
			Vector3D right = new Vector3D(1, 0, 0);
			AddSegment(mesh, new Point3D(1, -1, 1), new Point3D(1, 1, 1), right);
			AddSegment(mesh, new Point3D(1, -1, -1), new Point3D(1, 1, -1), right);
			AddSegment(mesh, new Point3D(-1, -1, 1), new Point3D(-1, 1, 1), right);
			AddSegment(mesh, new Point3D(-1, -1, -1), new Point3D(-1, 1, -1), right);
		}

		// Set the vector's length.
		private static Vector3D ScaleVector(Vector3D vector, double length)
		{
			double scale = length / vector.Length;
			return new Vector3D(
				vector.X * scale,
				vector.Y * scale,
				vector.Z * scale);
		}
	}
}
