using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using System.Windows.Media.Media3D;
using System.ComponentModel;
using System.Threading;
using System.Windows.Interop;
namespace _3D
{
	/// <summary>
	/// UserControl1.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class UserControl1 : UserControl
	{
        #region TrackBall
        private Trackball trackball;
        #endregion

        #region model
        private ModelVisual3D modelVisual3D { get; set; }
		private Model3DGroup model_group { get; set; }
		#endregion

		#region camera
		private PerspectiveCamera TheCamera;
		#endregion
	   	public UserControl1()
		{
			InitializeComponent();

			this.Loaded += delegate
			{
				var source = PresentationSource.FromVisual(this);
				var hwndTarget = source.CompositionTarget as HwndTarget;
				if (hwndTarget != null)
				{
					hwndTarget.RenderMode = RenderMode.SoftwareOnly;
				}
			};

			// Give the camera its initial position
			TheCamera = new PerspectiveCamera();
			TheCamera.FieldOfView = 600.0;
			MainViewport.Camera = TheCamera;

			InitScene();

			this.trackball = new Trackball();
			this.trackball.Attach(this);
			this.trackball.ViewportList.Add(this.MainViewport);
			this.trackball.Enabled = true;
		}
        ~UserControl1()
        {
			trackball.Detach(this);
		}
		public void InitScene(e_ViewPlane selectedPlane = e_ViewPlane.Default)
		{
			// Look toward the origin.
			TheCamera.Position = new Point3D(0, 0, 300);
			TheCamera.LookDirection = new Vector3D(0, 0, -1);

			// Set the Up direction.
			TheCamera.UpDirection = new Vector3D(0, 1, 0);
			TheCamera.NearPlaneDistance = 0.1;
			TheCamera.FarPlaneDistance = 5000;
			TheCamera.FieldOfView = 1000;


			// Make a model group
			this.model_group = new Model3DGroup();

			// Add the group of models to a ModelVisual3D
			this.modelVisual3D = new ModelVisual3D(); // AKA model_visual
			this.modelVisual3D.Content = this.model_group;

			InitPlane();
			this.modelVisual3D.Transform = viewPlane[e_ViewPlane.Default].Transform3DGroup;


			//Light 표기 관련 내용
			DirectionalLight dirLight = new DirectionalLight();
			dirLight.Color = Colors.White;
			dirLight.Direction = new Vector3D(-3, -4, -5);
			model_group.Children.Add(dirLight);

			MainViewport.Children.Clear();
			MainViewport.Children.Add(this.modelVisual3D);

			//Model List Clear
			modelList.Clear();

            //Change Form
            if (trackball != null)
            {
				trackball.Reset();
			}
		}

		#region View Plane (Default/XY/YZ/XZ)
		public Dictionary<e_ViewPlane, ViewPlane> viewPlane = new Dictionary<e_ViewPlane, ViewPlane>();

		public enum e_ViewPlane
        {
			Default,
			XY,
			YZ,
			XZ
        }
		public class ViewPlane
		{
			// Add the rotation transform to a Transform3DGroup
			public Transform3DGroup Transform3DGroup = new Transform3DGroup();

			// Create and apply a scale transformation that stretches the object along the local x-axis
			// by 200 percent and shrinks it along the local y-axis by 50 percent.
			public ScaleTransform3D ScaleTransform3D = new ScaleTransform3D();

			//Transform
			// Create and apply a transformation that rotates the object.
			public RotateTransform3D RotateTransform3D = new RotateTransform3D();
			public AxisAngleRotation3D AxisAngleRotation3d = new AxisAngleRotation3D();

			public TranslateTransform3D TranslateTransform3D = new TranslateTransform3D();
        }


		private bool InitPlane()
        {
			bool result = false;
			viewPlane.Clear();
			for (int i = 0; i < Enum.GetNames(typeof(e_ViewPlane)).Length; i++)
            {
				viewPlane.Add((e_ViewPlane)i, new ViewPlane());
			}

			viewPlane[e_ViewPlane.Default].ScaleTransform3D.ScaleX =
			viewPlane[e_ViewPlane.Default].ScaleTransform3D.ScaleY =
			viewPlane[e_ViewPlane.Default].ScaleTransform3D.ScaleZ = 0.76;

			viewPlane[e_ViewPlane.Default].Transform3DGroup.Children.Add(viewPlane[e_ViewPlane.Default].ScaleTransform3D);

			viewPlane[e_ViewPlane.Default].AxisAngleRotation3d.Axis = new Vector3D(0.08, -0.31, 0.95);
			viewPlane[e_ViewPlane.Default].AxisAngleRotation3d.Angle = 148;
			viewPlane[e_ViewPlane.Default].RotateTransform3D.Rotation = viewPlane[e_ViewPlane.Default].AxisAngleRotation3d;
			viewPlane[e_ViewPlane.Default].Transform3DGroup.Children.Add(viewPlane[e_ViewPlane.Default].RotateTransform3D);

			viewPlane[e_ViewPlane.Default].TranslateTransform3D.OffsetX = 10;
			viewPlane[e_ViewPlane.Default].TranslateTransform3D.OffsetY = 33;
			viewPlane[e_ViewPlane.Default].TranslateTransform3D.OffsetZ = 0;
			viewPlane[e_ViewPlane.Default].Transform3DGroup.Children.Add(viewPlane[e_ViewPlane.Default].TranslateTransform3D);

			viewPlane[e_ViewPlane.XY].ScaleTransform3D.ScaleX =
			viewPlane[e_ViewPlane.XY].ScaleTransform3D.ScaleY =
			viewPlane[e_ViewPlane.XY].ScaleTransform3D.ScaleZ = 0.75;

			viewPlane[e_ViewPlane.XY].Transform3DGroup.Children.Add(viewPlane[e_ViewPlane.XY].ScaleTransform3D);

			viewPlane[e_ViewPlane.XY].AxisAngleRotation3d.Axis = new Vector3D(0.0, 0.0, 0.9);
			viewPlane[e_ViewPlane.XY].AxisAngleRotation3d.Angle = 180;
			viewPlane[e_ViewPlane.XY].RotateTransform3D.Rotation = viewPlane[e_ViewPlane.XY].AxisAngleRotation3d;
			viewPlane[e_ViewPlane.XY].Transform3DGroup.Children.Add(viewPlane[e_ViewPlane.XY].RotateTransform3D);

			viewPlane[e_ViewPlane.XY].TranslateTransform3D.OffsetX = 0;
			viewPlane[e_ViewPlane.XY].TranslateTransform3D.OffsetY = 0;
			viewPlane[e_ViewPlane.XY].TranslateTransform3D.OffsetZ = 0;
			viewPlane[e_ViewPlane.XY].Transform3DGroup.Children.Add(viewPlane[e_ViewPlane.XY].TranslateTransform3D);

			viewPlane[e_ViewPlane.XZ].ScaleTransform3D.ScaleX =
			viewPlane[e_ViewPlane.XZ].ScaleTransform3D.ScaleY =
			viewPlane[e_ViewPlane.XZ].ScaleTransform3D.ScaleZ = 0.76;

			viewPlane[e_ViewPlane.XZ].Transform3DGroup.Children.Add(viewPlane[e_ViewPlane.XZ].ScaleTransform3D);

			viewPlane[e_ViewPlane.XZ].AxisAngleRotation3d.Axis = new Vector3D(0.0, 0.46, -0.88);
			viewPlane[e_ViewPlane.XZ].AxisAngleRotation3d.Angle = 180;
			viewPlane[e_ViewPlane.XZ].RotateTransform3D.Rotation = viewPlane[e_ViewPlane.XZ].AxisAngleRotation3d;
			viewPlane[e_ViewPlane.XZ].Transform3DGroup.Children.Add(viewPlane[e_ViewPlane.XZ].RotateTransform3D);

			viewPlane[e_ViewPlane.XZ].TranslateTransform3D.OffsetX = 6;
			viewPlane[e_ViewPlane.XZ].TranslateTransform3D.OffsetY = 47;
			viewPlane[e_ViewPlane.XZ].TranslateTransform3D.OffsetZ = 0;
			viewPlane[e_ViewPlane.XZ].Transform3DGroup.Children.Add(viewPlane[e_ViewPlane.XZ].TranslateTransform3D);

			viewPlane[e_ViewPlane.YZ].ScaleTransform3D.ScaleX =
			viewPlane[e_ViewPlane.YZ].ScaleTransform3D.ScaleY =
			viewPlane[e_ViewPlane.YZ].ScaleTransform3D.ScaleZ = 0.88;

			viewPlane[e_ViewPlane.YZ].Transform3DGroup.Children.Add(viewPlane[e_ViewPlane.YZ].ScaleTransform3D);

			viewPlane[e_ViewPlane.YZ].AxisAngleRotation3d.Axis = new Vector3D(0.59, -0.56, 0.58);
			viewPlane[e_ViewPlane.YZ].AxisAngleRotation3d.Angle = 116.8;
			viewPlane[e_ViewPlane.YZ].RotateTransform3D.Rotation = viewPlane[e_ViewPlane.YZ].AxisAngleRotation3d;
			viewPlane[e_ViewPlane.YZ].Transform3DGroup.Children.Add(viewPlane[e_ViewPlane.YZ].RotateTransform3D);

			viewPlane[e_ViewPlane.YZ].TranslateTransform3D.OffsetX = -6;
			viewPlane[e_ViewPlane.YZ].TranslateTransform3D.OffsetY = 71;
			viewPlane[e_ViewPlane.YZ].TranslateTransform3D.OffsetZ = 0;
			viewPlane[e_ViewPlane.YZ].Transform3DGroup.Children.Add(viewPlane[e_ViewPlane.YZ].TranslateTransform3D);

			return result;
        }

		public void ChangePlane(e_ViewPlane selectedPlane)
        {
			InitPlane();
			modelVisual3D.Transform = viewPlane[selectedPlane].Transform3DGroup;
		}
        #endregion

        #region GeometryModel3D Add/Remove
        private Dictionary<string, GeometryModel3D> modelList = new Dictionary<string, GeometryModel3D>();

		//TDL Mash로 만들수 있는지 확인 
		//https://icodebroker.tistory.com/6566 참고
		/// <summary>
		/// 사각형을 그려 줍니다. P1 ~ P4 반시계 방향으로
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="p3"></param>
		/// <param name="p4"></param>
		/// <param name="brush"></param>
		/// <param name="thik"></param>
		public void AddSquare(Point3D p1, Point3D p2, Point3D p3, Point3D p4, SolidColorBrush brush, double thik = 0.5)
		{
			AddLine(p1, p2, brush, thik);
			AddLine(p2, p3, brush, thik);
			AddLine(p3, p4, brush, thik);
			AddLine(p4, p1, brush, thik);
		}
		/// <summary>
		/// X-Y 평면에 평행한 원을 생성 합니다.
		/// </summary>
		/// <param name="originPoint">원의 중심점</param>
		/// <param name="radius">반지름</param>
		/// <param name="res">도형의 분해능</param>
		/// <param name="brush">색상</param>
		/// <param name="tick">원의 두께</param>
		/// <param name="bCrossLine">중심 기준 + 모양 </param>
		public void AddXYCircle(Point3D originPoint, double radius, double tubeRadius , SolidColorBrush brush, int sliceCount = 200, int stackCount = 100,  bool bCrossLine = true)
		{
			//원 굵기, 반지름, 원점 , 색상 

			TorusMesh torusMesh = new TorusMesh();
			torusMesh.Radius = radius;
			torusMesh.TubeRadius = tubeRadius;
			torusMesh.SliceCount = sliceCount;
			torusMesh.StackCount = stackCount;
			torusMesh.OriginPoint = originPoint;

			DiffuseMaterial diffuseMaterial = new DiffuseMaterial(brush);
			GeometryModel3D model = new GeometryModel3D(torusMesh.Geometry, diffuseMaterial);
			this.model_group.Children.Add(model);

			if (bCrossLine)
			{
				Point3D xLineStartPoint = new Point3D(originPoint.X - radius, originPoint.Y, originPoint.Z);
				Point3D xLineEndPoint = new Point3D(originPoint.X + radius, originPoint.Y, originPoint.Z);

				Point3D yLineStartPoint = new Point3D(originPoint.X, originPoint.Y - radius, originPoint.Z);
				Point3D yLineEndPoint = new Point3D(originPoint.X, originPoint.Y + radius, originPoint.Z);

				AddLine(xLineStartPoint, xLineEndPoint, brush, torusMesh.TubeRadius / 2);
				AddLine(yLineStartPoint, yLineEndPoint, brush, torusMesh.TubeRadius / 2);
			}
		}
		/// <summary>
		/// 격자 모양의 차트 축을 생성 합니다.
		/// </summary>
		/// <param name="startPoint">차트의 시작 위치를 지정 합니다.</param>
		/// <param name="xCount">X 격자 갯 수</param>
		/// <param name="xInterval">X 격자 간격</param>
		/// <param name="yCount">Y 격자 갯 수</param>
		/// <param name="yInterval">Y 격자 간격</param>
		/// <param name="zCount">Z 격자 갯 수</param>
		/// <param name="zInterval">Z 격자 간격</param>
		/// <param name="tick">격자의 두께</param>
		public void AddAxis(Point3D startPoint, int xCount, int xInterval, int yCount, int yInterval, int zCount, int zInterval, double tick = 0.5)
		{
			double xLineSize = startPoint.X + xInterval * (xCount);
			double yLineSize = startPoint.Y + yInterval * (yCount);
			double zLineSize = startPoint.Z + zInterval * (zCount);

			SolidColorBrush axisLineBrush = Brushes.Yellow;
			SolidColorBrush axisTextBrush = Brushes.Black;

			#region X - Y Plane
			for (int x = 0; x <= xCount; x++)
			{
				Point3D startXpoint = new Point3D(startPoint.X + x * xInterval, startPoint.Y, startPoint.Z);
				Point3D endXpoint = new Point3D(startPoint.X + x * xInterval, yLineSize, startPoint.Z);
				AddLine(startXpoint, endXpoint, axisLineBrush, tick);

				Point3D labelXPoint = new Point3D(startXpoint.X, startXpoint.Y - yInterval, startXpoint.Z);
				AddLabel(string.Format("{0}", startXpoint.X), axisTextBrush, true, xInterval / 2, labelXPoint, new Vector3D(0.0, 1.0, 0.0), new Vector3D(-1.0, 0.0, 0.0), null);
			}

			double xCenterPoint = startPoint.X + (xInterval * xCount) / 2;
			Point3D xLabelPoint = new Point3D(xCenterPoint, startPoint.Y - yInterval * 3, startPoint.Z);
			AddLabel(string.Format("X-Axis"), axisTextBrush, true, xInterval / 2, xLabelPoint, new Vector3D(1.0, 0.0, 0.0), new Vector3D(0.0, 1.0, 0.0), null);

			for (int y = 0; y <= yCount; y++)
			{
				Point3D startYpoint = new Point3D(startPoint.X, startPoint.Y + y * yInterval, startPoint.Z);
				Point3D endYpoint = new Point3D(xLineSize, startPoint.Y + y * yInterval, startPoint.Z);
				AddLine(startYpoint, endYpoint, axisLineBrush, tick);

				Point3D labelYPoint = new Point3D(endYpoint.X + xInterval, endYpoint.Y, startPoint.Z);
				AddLabel(string.Format("{0}", startYpoint.Y), axisTextBrush, true, yInterval / 2, labelYPoint, new Vector3D(1.0, 0.0, 0.0), new Vector3D(0.0, 1.0, 0.0), null);
			}

			double yCenterPoint = startPoint.Y + (yInterval * yCount) / 2;
			Point3D yLabelPoint = new Point3D(xLineSize + xInterval * 3, yCenterPoint, startPoint.Z);
			AddLabel(string.Format("Y-Axis"), axisTextBrush, true, yInterval / 2, yLabelPoint, new Vector3D(0.0, 1.0, 0.0), new Vector3D(-1.0, 0.0, 0.0), null);
            #endregion

            #region X - Z Plane
            //for (int x = 0; x <= xCount; x++)
            //{
            //	Point3D startXpoint = new Point3D(startPoint.X + x * xInterval, startPoint.Y, startPoint.Z);
            //	Point3D endXpoint = new Point3D(startPoint.X + x * xInterval, startPoint.Y, zLineSize);

            //	AddLine(startXpoint, endXpoint, axisLineBrush, tick);
            //}

            //for (int z = 0; z <= zCount; z++)
            //{
            //	Point3D startZpoint = new Point3D(startPoint.X, startPoint.Y, startPoint.Z + z * zInterval);
            //	Point3D endZpoint = new Point3D(xLineSize, startPoint.Y, startPoint.Z + z * zInterval);

            //	AddLine(startZpoint, endZpoint, axisLineBrush, tick);

            //	if (z > 0)
            //	{
            //		Point3D labelZPoint = new Point3D(endZpoint.X + xInterval, endZpoint.Y, endZpoint.Z);
            //		AddLabel(string.Format("{0}", startZpoint.Z), axisTextBrush, true, xInterval / 2, labelZPoint, new Vector3D(1.0, 0.0, 0.0), new Vector3D(0.0, 0.0, 1.0), null);
            //	}
            //}

            //double zCenterPoint = startPoint.Z + (zInterval * zCount) / 2;
            //Point3D zLabelPoint = new Point3D(xLineSize + xInterval * 3, startPoint.Y, zCenterPoint);
            //AddLabel(string.Format("Z-Axis"), axisTextBrush, true, xInterval / 2, zLabelPoint, new Vector3D(0.0, 0.0, 1.0), new Vector3D(1.0, 0.0, 0.0), null);
            #endregion

            #region Y - Z Plane
            for (int y = 0; y <= yCount; y++)
            {
                Point3D startYpoint = new Point3D(startPoint.X, startPoint.Y +y * yInterval, startPoint.Z);
                Point3D endYpoint = new Point3D(startPoint.X, startPoint.Y +y * yInterval, zLineSize);

                AddLine(startYpoint, endYpoint, axisLineBrush, tick);
            }

            for (int z = 0; z <= zCount; z++)
            {
                Point3D startZpoint = new Point3D(startPoint.X, startPoint.Y, startPoint.Z + z * zInterval);
                Point3D endZpoint = new Point3D(startPoint.X, yLineSize, startPoint.Z + z * zInterval);

                AddLine(startZpoint, endZpoint, axisLineBrush, tick);

                if (z > 0)
                {
                    Point3D labelZPoint = new Point3D(startZpoint.X, startZpoint.Y - yInterval, startZpoint.Z);
                    AddLabel(string.Format("{0}", startZpoint.Z), axisTextBrush, true, xInterval / 2, labelZPoint, new Vector3D(0.0, 1.0, 0.0), new Vector3D(0.0, 0.0, 1.0), null);
                }
            }

            double zCenterPoint = startPoint.Z + (zInterval * zCount) / 2;
            Point3D zLabelPoint = new Point3D(startPoint.X, startPoint.Y - 3 * yInterval, zCenterPoint);
            AddLabel(string.Format("Z-Axis"), axisTextBrush, true, xInterval / 2, zLabelPoint, new Vector3D(0.0, 0.0, -1.0), new Vector3D(0.0, 1.0, 0.0), null);
            #endregion
        }

		/// <summary>
		/// 직선을 생성 합니다.
		/// </summary>
		/// <param name="startPoint">시작 점</param>
		/// <param name="endPoint">끝 점</param>
		/// <param name="colorBrush">색상</param>
		/// <param name="tick">두게</param>
		public void AddLine(Point3D startPoint, Point3D endPoint, SolidColorBrush colorBrush ,double tick = 0.1 , string keyName = "")
		{
			Vector3D vector = new Vector3D(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y, endPoint.Z - startPoint.Z);

			MeshGeometry3D testAxis = new MeshGeometry3D();
			Cylinder.AddSmoothCylinder(testAxis, startPoint, vector, tick, 8);
			SolidColorBrush brushtestaxis = colorBrush;
			DiffuseMaterial materialtestaxis = new DiffuseMaterial(brushtestaxis);

            if (!string.IsNullOrEmpty(keyName))
            {
#if false
				if (!modelList.ContainsKey(keyName))
                {
					modelList.Add(keyName, new GeometryModel3D(testAxis, materialtestaxis));
					this.model_group.Children.Add(modelList[keyName]);
                }
#else // keyName 존재 시, 기존 데이터 삭제 후 등록
				if (modelList.ContainsKey(keyName))
				{
					RemoveModel(keyName);
				}
#endif
				modelList.Add(keyName, new GeometryModel3D(testAxis, materialtestaxis));
				this.model_group.Children.Add(modelList[keyName]);
			}
            else
            {
				GeometryModel3D model = new GeometryModel3D(testAxis, materialtestaxis);
				this.model_group.Children.Add(model);
			}
		}

		public void RemoveModel(string keyName)
        {
            if (modelList.ContainsKey(keyName))
            {
				model_group.Children.Remove(modelList[keyName]);
				modelList.Remove(keyName);

			}
		}

		public void AddLabel(
			string text,
			Brush textColor,
			bool bDoubleSided,
			double height,
			Point3D center,
			Vector3D over,
			Vector3D up,
			Transform3DGroup transform)
		{
			// First we need a textblock containing the text of our label
			TextBlock tb = new TextBlock(new Run(text));
			tb.Foreground = textColor;
			tb.FontFamily = new FontFamily("Arial");
			tb.FontFamily = new FontFamily("Courier");

			// Now use that TextBlock as the brush for a material
			DiffuseMaterial mat = new DiffuseMaterial();
			mat.Brush = new VisualBrush(tb);

			// We just assume the characters are square
			double width = text.Length * height;

			// Since the parameter coming in was the center of the label,
			// we need to find the four corners
			// p0 is the lower left corner
			// p1 is the upper left
			// p2 is the lower right
			// p3 is the upper right
			Point3D p0 = center - width / 2 * over - height / 2 * up;
			Point3D p1 = p0 + up * 1 * height;
			Point3D p2 = p0 + over * width;
			Point3D p3 = p0 + up * 1 * height + over * width;

			// Now build the geometry for the sign.  It's just a
			// rectangle made of two triangles, on each side.

			MeshGeometry3D mg = new MeshGeometry3D();
			mg.Positions = new Point3DCollection();
			mg.Positions.Add(p0);    // 0
			mg.Positions.Add(p1);    // 1
			mg.Positions.Add(p2);    // 2
			mg.Positions.Add(p3);    // 3

			if (bDoubleSided)
			{
				mg.Positions.Add(p0);    // 4
				mg.Positions.Add(p1);    // 5
				mg.Positions.Add(p2);    // 6
				mg.Positions.Add(p3);    // 7
			}

			mg.TriangleIndices.Add(0);
			mg.TriangleIndices.Add(3);
			mg.TriangleIndices.Add(1);
			mg.TriangleIndices.Add(0);
			mg.TriangleIndices.Add(2);
			mg.TriangleIndices.Add(3);

			if (bDoubleSided)
			{
				mg.TriangleIndices.Add(4);
				mg.TriangleIndices.Add(5);
				mg.TriangleIndices.Add(7);
				mg.TriangleIndices.Add(4);
				mg.TriangleIndices.Add(7);
				mg.TriangleIndices.Add(6);
			}

			// These texture coordinates basically stretch the
			// TextBox brush to cover the full side of the label.

			mg.TextureCoordinates.Add(new Point(0, 1));
			mg.TextureCoordinates.Add(new Point(0, 0));
			mg.TextureCoordinates.Add(new Point(1, 1));
			mg.TextureCoordinates.Add(new Point(1, 0));

			if (bDoubleSided)
			{
				mg.TextureCoordinates.Add(new Point(1, 1));
				mg.TextureCoordinates.Add(new Point(1, 0));
				mg.TextureCoordinates.Add(new Point(0, 1));
				mg.TextureCoordinates.Add(new Point(0, 0));
			}

			// And that's all.  Return the result.

			GeometryModel3D mv3d = new GeometryModel3D(mg, mat);
			this.model_group.Children.Add(mv3d);
		}
#endregion
    }
}
