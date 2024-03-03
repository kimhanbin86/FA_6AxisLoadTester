using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace _3D
{
    /// <summary>
    /// ��ȯü �޽�
    /// </summary>
    public class TorusMesh : MeshGeneratorBase
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Public

        #region Field

        /// <summary>
        /// �ݰ� �Ӽ�
        /// </summary>
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register
        (
            "Radius",
            typeof(double),
            typeof(TorusMesh),
            new PropertyMetadata(1.0, PropertyChanged)
        );

        /// <summary>
        /// Ʃ�� �ݰ� �Ӽ�
        /// </summary>
        public static readonly DependencyProperty TubeRadiusProperty = DependencyProperty.Register
        (
            "TubeRadius",
            typeof(double),
            typeof(TorusMesh),
            new PropertyMetadata(0.25, PropertyChanged)
        );

        /// <summary>
        /// �����̽� �� �Ӽ�
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
        /// ���� �� �Ӽ�
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
        /// ����
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

        #region �ݰ� - Radius

        /// <summary>
        /// �ݰ�
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
        /// ����
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
        #region Ʃ�� �ݰ� - TubeRadius

        /// <summary>
        /// Ʃ�� �ݰ�
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
        #region �����̽� �� - SliceCount

        /// <summary>
        /// �����̽� ��
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
        #region ���� �� - StackCount

        /// <summary>
        /// ���� ��
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

        #region ������ - TorusMesh()

        /// <summary>
        /// ������
        /// </summary>
        public TorusMesh()
        {
            PropertyChanged(new DependencyPropertyChangedEventArgs());
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Private

        #region �����̽� �� ���Ἲ üũ�ϱ� - ValidateSliceCount(sourceObject)

        /// <summary>
        /// �����̽� �� ���Ἲ üũ�ϱ�
        /// </summary>
        /// <param name="sourceObject">�ҽ� ��ü</param>
        /// <returns>ó�� ���</returns>
        private static bool ValidateSliceCount(object sourceObject)
        {
            return (int)sourceObject > 2;
        }

        #endregion
        #region ���� �� ���Ἲ üũ�ϱ� - ValidateStackCount(sourceObject)

        /// <summary>
        /// ���� �� ���Ἲ üũ�ϱ�
        /// </summary>
        /// <param name="sourceObject">�ҽ� ��ü</param>
        /// <returns>ó�� ���</returns>
        private static bool ValidateStackCount(object sourceObject)
        {
            return (int)sourceObject > 2;
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
        /// <param name="textureCollection">�ؽ�ó �÷���</param>
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
        #region �ν��Ͻ� �����ϱ� (�ھ�) - CreateInstanceCore()

        /// <summary>
        /// �ν��Ͻ� �����ϱ� (�ھ�)
        /// </summary>
        /// <returns>�ν��Ͻ�</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new TorusMesh();
        }

        #endregion
    }
}