using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace _3D
{
    /// <summary>
    /// 메쉬 제너레이터 베이스
    /// </summary>
    [RuntimeNameProperty("Name")]
    public abstract class MeshGeneratorBase : Animatable
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 도형 키
        /// </summary>
        private static DependencyPropertyKey GeometryKey = DependencyProperty.RegisterReadOnly
        (
            "Geometry",
            typeof(MeshGeometry3D),
            typeof(MeshGeneratorBase),
            new PropertyMetadata(new MeshGeometry3D())
        );

        #endregion

        //////////////////////////////////////////////////////////////////////////////// Public

        #region Field

        /// <summary>
        /// 명칭 속성
        /// </summary>
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register
        (
            "Name",
            typeof(string),
            typeof(MeshGeneratorBase)
        );

        /// <summary>
        /// 도형 속성
        /// </summary>
        public static readonly DependencyProperty GeometryProperty = GeometryKey.DependencyProperty;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Property
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 명칭 - Name

        /// <summary>
        /// 명칭
        /// </summary>
        public string Name
        {
            set
            {
                SetValue(NameProperty, value);
            }
            get
            {
                return (string)GetValue(NameProperty);
            }
        }

        #endregion
        #region 도형 - Geometry

        /// <summary>
        /// 도형
        /// </summary>
        public MeshGeometry3D Geometry
        {
            get
            {
                return (MeshGeometry3D)GetValue(GeometryProperty);
            }
            protected set
            {
                SetValue(GeometryKey, value);
            }
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - MeshGeneratorBase()

        /// <summary>
        /// 생성자
        /// </summary>
        public MeshGeneratorBase()
        {
            Geometry = new MeshGeometry3D();
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Protected

        #region 속성 변경시 처리하기 - PropertyChanged(d, e)

        /// <summary>
        /// 속성 변경시 처리하기
        /// </summary>
        /// <param name="d">의존 객체</param>
        /// <param name="e">이벤트 인자</param>
        protected static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MeshGeneratorBase).PropertyChanged(e);
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
        /// <param name="textureCollection">텍스쳐 컬렉션</param>
        protected abstract void Triangulate(DependencyPropertyChangedEventArgs e, Point3DCollection vertexCollection, Vector3DCollection normalCollection, Int32Collection indexCollection, PointCollection textureCollection);

        #endregion
        #region 속성 변경시 처리하기 - PropertyChanged(e)

        /// <summary>
        /// 속성 변경시 처리하기
        /// </summary>
        /// <param name="e">이벤트 인자</param>
        protected virtual void PropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            MeshGeometry3D mesh = Geometry;

            Point3DCollection  vertexCollection  = mesh.Positions;
            Vector3DCollection normalCollection  = mesh.Normals;
            Int32Collection    indexCollection   = mesh.TriangleIndices;
            PointCollection    textureCollection = mesh.TextureCoordinates;

            mesh.Positions          = null;
            mesh.Normals            = null;
            mesh.TriangleIndices    = null;
            mesh.TextureCoordinates = null;

            Triangulate(e, vertexCollection, normalCollection, indexCollection, textureCollection);

            mesh.TextureCoordinates = textureCollection;
            mesh.TriangleIndices    = indexCollection;
            mesh.Normals            = normalCollection;
            mesh.Positions          = vertexCollection;
        }

        #endregion
    }
}