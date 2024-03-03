using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace _3D
{
    /// <summary>
    /// 원환체 메쉬
    /// </summary>
    public class TorusMesh : MeshGeneratorBase
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Public

        #region Field

        /// <summary>
        /// 반경 속성
        /// </summary>
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register
        (
            "Radius",
            typeof(double),
            typeof(TorusMesh),
            new PropertyMetadata(1.0, PropertyChanged)
        );

        /// <summary>
        /// 튜브 반경 속성
        /// </summary>
        public static readonly DependencyProperty TubeRadiusProperty = DependencyProperty.Register
        (
            "TubeRadius",
            typeof(double),
            typeof(TorusMesh),
            new PropertyMetadata(0.25, PropertyChanged)
        );

        /// <summary>
        /// 슬라이스 수 속성
        /// </summary>
        public static readonly DependencyProperty SliceCountProperty = DependencyProperty.Register
        (
            "SliceCount",
            typeof(int),
            typeof(TorusMesh),
            new PropertyMetadata(18, PropertyChanged),
            ValidateSliceCount
        );

        /// <summary>
        /// 스택 수 속성
        /// </summary>
        public static readonly DependencyProperty StackCountProperty = DependencyProperty.Register
        (
            "StackCount",
            typeof(int),
            typeof(TorusMesh),
            new PropertyMetadata(36, PropertyChanged),
            ValidateStackCount
        );

        /// <summary>
        /// 원점
        /// </summary>
        public static readonly DependencyProperty OriginPointProperty = DependencyProperty.Register
       (
           "OriginPoint",
           typeof(Point3D),
           typeof(TorusMesh),
           new PropertyMetadata(new Point3D(0,0,0), PropertyChanged)
       );

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Property
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 반경 - Radius

        /// <summary>
        /// 반경
        /// </summary>
        public double Radius
        {
            set
            {
                SetValue(RadiusProperty, value);
            }
            get
            {
                return (double)GetValue(RadiusProperty);
            }
        }

        /// <summary>
        /// 원점
        /// </summary>
        public Point3D OriginPoint
        {
            set
            {
                SetValue(OriginPointProperty, value);
            }
            get
            {
                return (Point3D)GetValue(OriginPointProperty);
            }
        }

        #endregion
        #region 튜브 반경 - TubeRadius

        /// <summary>
        /// 튜브 반경
        /// </summary>
        public double TubeRadius
        {
            set
            {
                SetValue(TubeRadiusProperty, value);
            }
            get
            {
                return (double)GetValue(TubeRadiusProperty);
            }
        }

        #endregion
        #region 슬라이스 수 - SliceCount

        /// <summary>
        /// 슬라이스 수
        /// </summary>
        public int SliceCount
        {
            set
            {
                SetValue(SliceCountProperty, value);
            }
            get
            {
                return (int)GetValue(SliceCountProperty);
            }
        }

        #endregion
        #region 스택 수 - StackCount

        /// <summary>
        /// 스택 수
        /// </summary>
        public int StackCount
        {
            set
            {
                SetValue(StackCountProperty, value);
            }
            get
            {
                return (int)GetValue(StackCountProperty);
            }
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - TorusMesh()

        /// <summary>
        /// 생성자
        /// </summary>
        public TorusMesh()
        {
            PropertyChanged(new DependencyPropertyChangedEventArgs());
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Private

        #region 슬라이스 수 무결성 체크하기 - ValidateSliceCount(sourceObject)

        /// <summary>
        /// 슬라이스 수 무결성 체크하기
        /// </summary>
        /// <param name="sourceObject">소스 객체</param>
        /// <returns>처리 결과</returns>
        private static bool ValidateSliceCount(object sourceObject)
        {
            return (int)sourceObject > 2;
        }

        #endregion
        #region 스택 수 무결성 체크하기 - ValidateStackCount(sourceObject)

        /// <summary>
        /// 스택 수 무결성 체크하기
        /// </summary>
        /// <param name="sourceObject">소스 객체</param>
        /// <returns>처리 결과</returns>
        private static bool ValidateStackCount(object sourceObject)
        {
            return (int)sourceObject > 2;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////// Instance
        //////////////////////////////////////////////////////////////////////////////// Protected

        #region 삼각형 구성하기 - Triangulate(e, vertexCollection, normalCollection, indexCollection, textureCollection)

        /// <summary>
        /// 삼각형 구성하기
        /// </summary>
        /// <param name="e">이벤트 인자</param>
        /// <param name="vertexCollection">꼭지점 컬렉션</param>
        /// <param name="normalCollection">법선 컬렉션</param>
        /// <param name="indexCollection">인덱스 컬렉션</param>
        /// <param name="textureCollection">텍스처 컬렉션</param>
        protected override void Triangulate(DependencyPropertyChangedEventArgs e, Point3DCollection vertexCollection, Vector3DCollection normalCollection, Int32Collection indexCollection, PointCollection textureCollection)
        {
            vertexCollection.Clear();
            normalCollection.Clear();
            indexCollection.Clear();
            textureCollection.Clear();

            for(int stack = 0; stack <= StackCount; stack++)
            {
                double  phi         = stack * 2 * Math.PI / StackCount;
                double  xCenter     = Radius * Math.Sin(phi);
                double  yCenter     = Radius * Math.Cos(phi);
                Point3D centerPoint = new Point3D(xCenter, yCenter, 0);

                for(int slice = 0; slice <= SliceCount; slice++)
                {
                    double  theta = slice * 2 * Math.PI / SliceCount + Math.PI;
                    double  x     = OriginPoint.X + (Radius + TubeRadius * Math.Cos(theta)) * Math.Sin(phi);
                    double  y     = OriginPoint.Y + (Radius + TubeRadius * Math.Cos(theta)) * Math.Cos(phi);
                    double  z     = OriginPoint.Z - TubeRadius * Math.Sin(theta);
                    Point3D point = new Point3D(x, y, z);

                    vertexCollection.Add(point);

                    normalCollection.Add(point - centerPoint);

                    textureCollection.Add(new Point((double)slice / SliceCount, (double)stack / StackCount));
                }
            }

            for(int stack = 0; stack < StackCount; stack++)
            {
                for(int slice = 0; slice < SliceCount; slice++)
                {
                    indexCollection.Add((stack + 0) * (SliceCount + 1) + slice    );
                    indexCollection.Add((stack + 1) * (SliceCount + 1) + slice    );
                    indexCollection.Add((stack + 0) * (SliceCount + 1) + slice + 1);
 
                    indexCollection.Add((stack + 0) * (SliceCount + 1) + slice + 1);
                    indexCollection.Add((stack + 1) * (SliceCount + 1) + slice    );
                    indexCollection.Add((stack + 1) * (SliceCount + 1) + slice + 1);
                }
            }
        }

        #endregion
        #region 인스턴스 생성하기 (코어) - CreateInstanceCore()

        /// <summary>
        /// 인스턴스 생성하기 (코어)
        /// </summary>
        /// <returns>인스턴스</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new TorusMesh();
        }

        #endregion
    }
}