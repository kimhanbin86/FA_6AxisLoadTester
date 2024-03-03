using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace _3D
{
    /// <summary>
    /// 트랙볼
    /// </summary>
    public class Trackball
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 뷰포트 리스트
        /// </summary>
        private List<Viewport3D> viewportList;

        /// <summary>
        /// 이용 가능 여부
        /// </summary>
        private bool enabled;

        /// <summary>
        /// 드래그 최초 포인트
        /// </summary>
        private Point dragInitialPoint;

        /// <summary>
        /// 회전 쿼터니언
        /// </summary>
        private Quaternion rotateQuaternion;

        /// <summary>
        /// 회전 여부
        /// </summary>
        private bool isRotating;

        /// <summary>
        /// 스케일링 여부
        /// </summary>
        private bool scaling;

        /// <summary>
        /// 회전 델타 쿼터니언
        /// </summary>
        private Quaternion rotateDeltaQuaternion;

        /// <summary>
        /// 이동 델타 벡터
        /// </summary>
        private Vector3D translateDeltaVector;

        /// <summary>
        /// 이동 벡터
        /// </summary>
        private Vector3D translateVector;

        /// <summary>
        /// 스케일
        /// </summary>
        private double scale;

        /// <summary>
        /// 스케일 델타
        /// </summary>
        private double scaleDelta;

        /// <summary>
        /// 중심 여부
        /// </summary>
        private bool isCentered;

        /// <summary>
        /// 중심 벡터
        /// </summary>
        private Vector3D centerVector;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Property
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 뷰포트 리스트 - ViewportList

        /// <summary>
        /// 뷰포트 리스트
        /// </summary>
        public List<Viewport3D> ViewportList
        {
            get
            {
                if (this.viewportList == null)
                {
                    this.viewportList = new List<Viewport3D>();
                }

                return this.viewportList;
            }
            set
            {
                this.viewportList = value;
            }
        }

        #endregion
        #region 이용 가능 여부 - Enabled

        /// <summary>
        /// 이용 가능 여부
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this.enabled && (this.viewportList != null) && (this.viewportList.Count > 0);
            }
            set
            {
                this.enabled = value;
            }
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - Trackball()

        /// <summary>
        /// 생성자
        /// </summary>
        public Trackball()
        {
            Reset();
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 접착하기 - Attach(element)

        /// <summary>
        /// 접착하기
        /// </summary>
        /// <param name="element">프레임워크 엘리먼트</param>
        public void Attach(FrameworkElement element)
        {
            element.MouseMove += element_MouseMove;
            element.MouseLeftButtonDown += element_MouseLeftButtonDown;
            element.MouseLeftButtonUp += element_MouseLeftButtonUp;
            element.MouseWheel += element_MouseWheel;
        }

        #endregion
        #region 탈착하기 - Detach(element)

        /// <summary>
        /// 탈착하기
        /// </summary>
        /// <param name="element">프레임워크 엘리먼트</param>
        public void Detach(FrameworkElement element)
        {
            element.MouseMove -= element_MouseMove;
            element.MouseLeftButtonDown -= element_MouseLeftButtonDown;
            element.MouseLeftButtonUp -= element_MouseLeftButtonUp;
            element.MouseWheel -= element_MouseWheel;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////// Private
        //////////////////////////////////////////////////////////////////////////////// Event

        #region 엘리먼트 마우스 이동시 처리하기 - element_MouseMove(sender, e)

        /// <summary>
        /// 엘리먼트 마우스 이동시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void element_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            e.Handled = true;

            UIElement element = sender as UIElement;

            if (element.IsMouseCaptured)
            {
                Vector delta = this.dragInitialPoint - e.MouseDevice.GetPosition(element);

                Vector3D verctor = new Vector3D();

                delta /= 2;

                Quaternion quaternion = this.rotateQuaternion;

                if (this.isRotating == true)
                {
                    Vector3D mouseVector = new Vector3D(delta.X, -delta.Y, 0);

                    Vector3D axisVector = Vector3D.CrossProduct(mouseVector, new Vector3D(0, 0, 1));

                    double axisVectorLength = axisVector.Length;

                    if (axisVectorLength < 0.00001 || this.scaling)
                    {
                        this.rotateDeltaQuaternion = new Quaternion(new Vector3D(0, 0, 1), 0);
                    }
                    else
                    {
                        this.rotateDeltaQuaternion = new Quaternion(axisVector, axisVectorLength);
                    }

                    quaternion = this.rotateDeltaQuaternion * this.rotateQuaternion;
                }
                else
                {
                    delta /= 5;

                    this.translateDeltaVector.X = delta.X * -1;
                    this.translateDeltaVector.Y = delta.Y;
                }

                verctor = this.translateVector + this.translateDeltaVector;

                UpdateViewportList(quaternion, this.scale * this.scaleDelta, verctor);
            }
        }

        #endregion
        #region 엘리먼트 마우스 오른쪽 버튼 DOWN 처리하기 - element_MouseRightButtonDown(sender, e)

        /// <summary>
        /// 엘리먼트 마우스 오른쪽 버튼 DOWN 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            e.Handled = true;

            if (Keyboard.IsKeyDown(Key.F1) == true)
            {
                Reset();

                return;
            }

            UIElement element = sender as UIElement;

            this.dragInitialPoint = e.MouseDevice.GetPosition(element);

            if (!this.isCentered)
            {
                ProjectionCamera camera = (ProjectionCamera)this.viewportList[0].Camera;

                this.centerVector = camera.LookDirection;

                this.isCentered = true;
            }

            this.scaling = (e.MiddleButton == MouseButtonState.Pressed);

            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                this.isRotating = false;
            }
            else
            {
                this.isRotating = true;
            }

            element.CaptureMouse();
        }

        #endregion
        #region 엘리먼트 마우스 오른쪽 버튼 UP 처리하기 - element_MouseRightButtonUp(sender, e)

        /// <summary>
        /// 엘리먼트 마우스 오른쪽 버튼 UP 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.enabled)
            {
                return;
            }

            e.Handled = true;

            if (this.isRotating == true)
            {
                this.rotateQuaternion = this.rotateDeltaQuaternion * this.rotateQuaternion;
            }
            else
            {
                this.translateVector += this.translateDeltaVector;

                this.translateDeltaVector.X = 0;
                this.translateDeltaVector.Y = 0;
            }

            UIElement element = sender as UIElement;

            element.ReleaseMouseCapture();
        }

        #endregion
        #region 엘리먼트 마우스 휠 처리하기 - element_MouseWheel(sender, e)

        /// <summary>
        /// 엘리먼트 마우스 휠 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void element_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            this.scaleDelta += (double)((double)e.Delta / (double)1000);

            Quaternion quaternion = this.rotateQuaternion;

            UpdateViewportList(quaternion, this.scale * this.scaleDelta, this.translateVector);
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////// Function

        #region 뷰포트 리스트 업데이트하기 - UpdateViewportList(quaternion, scale, vector)

        /// <summary>
        /// 뷰포트 리스트 업데이트하기
        /// </summary>
        /// <param name="quaternion">쿼터니언</param>
        /// <param name="scale">스케일</param>
        /// <param name="vector">벡터</param>
        private void UpdateViewportList(Quaternion quaternion, double scale, Vector3D vector)
        {
            if (this.viewportList != null)
            {
                foreach (Viewport3D viewport in this.viewportList)
                {
                    ModelVisual3D modelVisual = viewport.Children[0] as ModelVisual3D;

                    Transform3DGroup transformGroup = modelVisual.Transform as Transform3DGroup;

                    ScaleTransform3D groupScaleTransform = transformGroup.Children[0] as ScaleTransform3D;

                    RotateTransform3D groupRotateTransform = transformGroup.Children[1] as RotateTransform3D;

                    TranslateTransform3D groupTranslateTransform = transformGroup.Children[2] as TranslateTransform3D;

                    groupScaleTransform.ScaleX = scale;
                    groupScaleTransform.ScaleY = scale;
                    groupScaleTransform.ScaleZ = scale;

                    groupRotateTransform.Rotation = new AxisAngleRotation3D(quaternion.Axis, quaternion.Angle);

                    groupTranslateTransform.OffsetX = vector.X;
                    groupTranslateTransform.OffsetY = vector.Y;
                    groupTranslateTransform.OffsetZ = vector.Z;
                }
            }
        }

        #endregion
        #region 리셋하기 - Reset()

        /// <summary>
        /// 리셋하기
        /// </summary>
        public void Reset()
        {
            this.rotateQuaternion = new Quaternion(0, 0, 0, 1);

            this.scale = 1;

            this.translateVector.X = 0;
            this.translateVector.Y = 0;
            this.translateVector.Z = 0;

            this.translateDeltaVector.X = 0;
            this.translateDeltaVector.Y = 0;
            this.translateDeltaVector.Z = 0;

            this.rotateDeltaQuaternion = Quaternion.Identity;

            this.scaleDelta = 1;

            UpdateViewportList(this.rotateQuaternion, this.scale, this.translateVector);
        }

        #endregion
    }
}