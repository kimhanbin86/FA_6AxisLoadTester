﻿using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace _3D
{
	public static class ExtensionMethods
	{
		/// <summary>
		/// Normalizes vector (extension method alternative to vector.Normalize())
		/// </summary>
		/// <param name="vector"></param>
		/// <returns>normalized vector</returns>
		public static Vector3D Normalize2(this Vector3D vector)
		{
			Vector3D vnorm = new Vector3D(vector.X / vector.Length, vector.Y / vector.Length, vector.Z / vector.Length);
			return vnorm;
		}

		#region wireframe
		// http://csharphelper.com/blog/2014/10/draw-surface-normals-on-a-3d-model-using-wpf-and-xaml/
		// Return a MeshGeometry3D representing this mesh's triangle normals.
		public static MeshGeometry3D ToTriangleNormals(this MeshGeometry3D mesh,
			double length, double thickness)
		{
			// Make a mesh to hold the normals.
			MeshGeometry3D normals = new MeshGeometry3D();

			// Loop through the mesh's triangles.
			for (int triangle = 0; triangle < mesh.TriangleIndices.Count; triangle += 3)
			{
				// Get the triangle's vertices.
				Point3D point1 = mesh.Positions[mesh.TriangleIndices[triangle]];
				Point3D point2 = mesh.Positions[mesh.TriangleIndices[triangle + 1]];
				Point3D point3 = mesh.Positions[mesh.TriangleIndices[triangle + 2]];

				// Make the triangle's normal
				AddTriangleNormal(mesh, normals,
					point1, point2, point3, length, thickness);
			}

			return normals;
		}

		// Add a segment representing the triangle's normal to the normals mesh.
		private static void AddTriangleNormal(MeshGeometry3D mesh,
			MeshGeometry3D normals, Point3D point1, Point3D point2, Point3D point3,
			double length, double thickness)
		{
			// Get the triangle's normal.
			Vector3D n = FindTriangleNormal(point1, point2, point3);

			// Set the length.
			n = ScaleVector(n, length);

			// Find the center of the triangle.
			Point3D endpoint1 = new Point3D(
				(point1.X + point2.X + point3.X) / 3.0,
				(point1.Y + point2.Y + point3.Y) / 3.0,
				(point1.Z + point2.Z + point3.Z) / 3.0);

			// Find the segment's other end point.
			Point3D endpoint2 = endpoint1 + n;

			// Create the segment.
			AddSegment(normals, endpoint1, endpoint2, thickness);
		}

		// Calculate a triangle's normal vector.
		public static Vector3D FindTriangleNormal(Point3D point1, Point3D point2, Point3D point3)
		{
			// Get two edge vectors.
			Vector3D v1 = point2 - point1;
			Vector3D v2 = point3 - point2;

			// Get the cross product.
			Vector3D n = Vector3D.CrossProduct(v1, v2);

			// Normalize.
			n.Normalize();

			return n;
		}

		// Return a MeshGeometry3D representing this mesh's wireframe.
		public static MeshGeometry3D ToWireframe(this MeshGeometry3D mesh, double thickness)
		{
			// Make a dictionary in case triangles share segments
			// so we don't draw the same segment twice.
			Dictionary<int, int> already_drawn = new Dictionary<int, int>();

			// Make a mesh to hold the wireframe.
			MeshGeometry3D wireframe = new MeshGeometry3D();

			// Loop through the mesh's triangles.
			for (int triangle = 0; triangle < mesh.TriangleIndices.Count; triangle += 3)
			{
				// Get the triangle's corner indices.
				int index1 = mesh.TriangleIndices[triangle];
				int index2 = mesh.TriangleIndices[triangle + 1];
				int index3 = mesh.TriangleIndices[triangle + 2];

				// Make the triangle's three segments.
				AddTriangleSegment(mesh, wireframe, already_drawn, index1, index2, thickness);
				AddTriangleSegment(mesh, wireframe, already_drawn, index2, index3, thickness);
				AddTriangleSegment(mesh, wireframe, already_drawn, index3, index1, thickness);
			}

			return wireframe;
		}

		// Add the triangle's three segments to the wireframe mesh.
		private static void AddTriangleSegment(MeshGeometry3D mesh,
			MeshGeometry3D wireframe, Dictionary<int, int> already_drawn,
			int index1, int index2, double thickness)
		{
			// Get a unique ID for a segment connecting the two points.
			if (index1 > index2)
			{
				int temp = index1;
				index1 = index2;
				index2 = temp;
			}
			int segment_id = index1 * mesh.Positions.Count + index2;

			// If we've already added this segment for
			// another triangle, do nothing.
			if (already_drawn.ContainsKey(segment_id)) return;
			already_drawn.Add(segment_id, segment_id);

			// Create the segment.
			AddSegment(wireframe, mesh.Positions[index1], mesh.Positions[index2], thickness);
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
		// If up is missing, create a perpendicular vector to use.
		public static void AddSegment(MeshGeometry3D mesh,
			Point3D point1, Point3D point2, double thickness, bool extend)
		{
			// Find an up vector that is not colinear with the segment.
			// Start with a vector parallel to the Y axis.
			Vector3D up = new Vector3D(0, 1, 0);

			// If the segment and up vector point in more or less the
			// same direction, use an up vector parallel to the X axis.
			Vector3D segment = point2 - point1;
			segment.Normalize();
			if (System.Math.Abs(Vector3D.DotProduct(up, segment)) > 0.9)
				up = new Vector3D(1, 0, 0);

			// Add the segment.
			AddSegment(mesh, point1, point2, up, thickness, extend);
		}
		public static void AddSegment(MeshGeometry3D mesh,
			Point3D point1, Point3D point2, double thickness)
		{
			AddSegment(mesh, point1, point2, thickness, false);
		}
		public static void AddSegment(MeshGeometry3D mesh,
			Point3D point1, Point3D point2, Vector3D up, double thickness)
		{
			AddSegment(mesh, point1, point2, up, thickness, false);
		}
		public static void AddSegment(MeshGeometry3D mesh,
			Point3D point1, Point3D point2, Vector3D up, double thickness,
			bool extend)
		{
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

		// Set the vector's length.
		public static Vector3D ScaleVector(Vector3D vector, double length)
		{
			double scale = length / vector.Length;
			return new Vector3D(
				vector.X * scale,
				vector.Y * scale,
				vector.Z * scale);
		}
		#endregion
	}
}
