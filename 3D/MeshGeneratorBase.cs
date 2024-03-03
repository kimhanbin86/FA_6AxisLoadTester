using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace _3D
{
    /// <summary>
    /// �޽� ���ʷ����� ���̽�
    /// </summary>
    [RuntimeNameProperty("Name")]
    public abstract class MeshGeneratorBase : Animatable
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// ���� Ű
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
        /// ��Ī �Ӽ�
        /// </summary>
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register
        (
            "Name",
            typeof(string),
            typeof(MeshGeneratorBase)
        );

        /// <summary>
        /// ���� �Ӽ�
        /// </summary>
        public static readonly DependencyProperty GeometryProperty = GeometryKey.DependencyProperty;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Property
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region ��Ī - Name

        /// <summary>
        /// ��Ī
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
        #region ���� - Geometry

        /// <summary>
        /// ����
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

        #region ������ - MeshGeneratorBase()

        /// <summary>
        /// ������
        /// </summary>
        public MeshGeneratorBase()
        {
            Geometry = new MeshGeometry3D();
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Protected

        #region �Ӽ� ����� ó���ϱ� - PropertyChanged(d, e)

        /// <summary>
        /// �Ӽ� ����� ó���ϱ�
        /// </summary>
        /// <param name="d">���� ��ü</param>
        /// <param name="e">�̺�Ʈ ����</param>
        protected static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MeshGeneratorBase).PropertyChanged(e);
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////// Instance
        //////////////////////////////////////////////////////////////////////////////// Protected

        #region �ﰢ�� �����ϱ� - Triangulate(e, vertexCollection, normalCollection, indexCollection, textureCollection)

        /// <summary>
        /// �ﰢ�� �����ϱ�
        /// </summary>
        /// <param name="e">�̺�Ʈ ����</param>
        /// <param name="vertexCollection">������ �÷���</param>
        /// <param name="normalCollection">���� �÷���</param>
        /// <param name="indexCollection">�ε��� �÷���</param>
        /// <param name="textureCollection">�ؽ��� �÷���</param>
        protected abstract void Triangulate(DependencyPropertyChangedEventArgs e, Point3DCollection vertexCollection, Vector3DCollection normalCollection, Int32Collection indexCollection, PointCollection textureCollection);

        #endregion
        #region �Ӽ� ����� ó���ϱ� - PropertyChanged(e)

        /// <summary>
        /// �Ӽ� ����� ó���ϱ�
        /// </summary>
        /// <param name="e">�̺�Ʈ ����</param>
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