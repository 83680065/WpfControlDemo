using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace WpfDemo.Controls2
{
    /// <summary>
    /// CycleProcessBar.xaml 的交互逻辑
    /// </summary>
    public partial class CycleProcessBar
    {
        private static readonly DependencyProperty CurrentValueProperty = DependencyProperty.Register("CurrentValue", typeof(double), typeof(CycleProcessBar),
          new PropertyMetadata(0.2d, DrawPropertyChangedCallback));
        private static readonly DependencyProperty ArcRadiusProperty = DependencyProperty.Register("ArcRadius", typeof(double), typeof(CycleProcessBar),
        new PropertyMetadata(80.0d));
        private static readonly DependencyProperty ArcWidthProperty = DependencyProperty.Register("ArcWidth", typeof(double), typeof(CycleProcessBar),
            new PropertyMetadata(3.0d));
        private static readonly DependencyProperty ProcessBarColorProperty = DependencyProperty.Register("ProcessBarColor", typeof(Brush), typeof(CycleProcessBar));

        private static readonly DependencyProperty BackgroundBarColorProperty = DependencyProperty.Register("BackgroundBarColor", typeof(Brush), typeof(CycleProcessBar));

        private static readonly DependencyProperty ShowBackGroundArcProperty = DependencyProperty.Register("ShowBackGroundArc", typeof(bool), typeof(CycleProcessBar),
          new PropertyMetadata(true, DrawPropertyChangedCallback));

        private static readonly DependencyProperty ClockwiseProperty = DependencyProperty.Register("Clockwise", typeof(bool), typeof(CycleProcessBar),
         new PropertyMetadata(true, DrawPropertyChangedCallback));

        private DrawPosition drawPosition = DrawPosition.Left;
        private double lastDifference = 0.01;
        private bool showDash = true;

        public CycleProcessBar()
        {
            InitializeComponent();
        }

        private void ChangeBarDash()
        {
            if (ShowDash)
            {
                BackGroundBar.StrokeDashArray = new DoubleCollection { 0.9, 0.1 };
            }
            else
            {
                BackGroundBar.StrokeDashArray = null;
            }

        }
        #region 属性

        /// <summary>
        /// 是否显示分割
        /// </summary>
        public bool ShowDash
        {
            get { return showDash; }
            set
            {
                showDash = value;
                ChangeBarDash();
            }
        }
        /// <summary>
        /// 当前值 0到1之间
        /// </summary>
        public double CurrentValue
        {
            get { return (double)GetValue(CurrentValueProperty); }
            set
            {
                SetValue(CurrentValueProperty, value);
            }
        }
        /// <summary>
        /// 圆弧直径
        /// </summary>
        public double ArcRadius
        {
            get { return (double)GetValue(ArcRadiusProperty); }
            set
            {
                SetValue(ArcRadiusProperty, value);
            }
        }

        /// <summary>
        /// 圆弧宽度
        /// </summary>
        public double ArcWidth
        {
            get { return (double)GetValue(ArcWidthProperty); }
            set { SetValue(ArcWidthProperty, value); }
        }

        /// <summary>
        /// 是否从Y轴开始绘制
        /// </summary>
        public bool ShowBackGroundArc
        {
            get { return (bool)GetValue(ShowBackGroundArcProperty); }
            set { SetValue(ShowBackGroundArcProperty, value); }
        }

        /// <summary>
        /// 绘制起始位置
        /// </summary>
        public DrawPosition DrawPosition
        {
            get { return drawPosition; }
            set
            {
                drawPosition = value;
                UpdateUI();
            }
        }
        /// <summary>
        /// 当前进度颜色
        /// </summary>
        public Brush ProcessBarColor
        {
            get { return (Brush)GetValue(ProcessBarColorProperty); }
            set { SetValue(ProcessBarColorProperty, value); }
        }

        /// <summary>
        /// 当前进度颜色
        /// </summary>
        public Brush BackgroundBarColor
        {
            get { return (Brush)GetValue(BackgroundBarColorProperty); }
            set { SetValue(BackgroundBarColorProperty, value); }
        }

        /// <summary>
        /// 是否顺时针
        /// </summary>
        public bool Clockwise
        {
            get { return (bool)GetValue(ClockwiseProperty); }
            set
            {
                SetValue(ClockwiseProperty, value);
            }
        }
        #endregion

        /// <summary>
        /// 属性变化需要更新UI
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void DrawPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bar = d as CycleProcessBar;
            bar?.UpdateUI();
        }

        private void UpdateUI()
        {
            lbValue.Content = (CurrentValue * 100).ToString("0") + "%";
            if (BackgroundBarColor == null)
            {
                BackgroundBarColor = FindResource("BackgroundBarColor") as Brush;
            }
            if (ProcessBarColor == null)
            {
                ProcessBarColor = FindResource("Linear_Yellow") as Brush;
            }
            if (CurrentValue >= 0.6)
            {
                ProcessBarColor = FindResource("Linear_Green") as Brush;
            }
            DrawPercentArc(CurrentValue);
        }

        /// <summary>
        /// 获取起始坐标
        /// </summary>
        /// <param name="angel"></param>
        /// <returns></returns>
        private List<Point> GetStartEndPoint(double angel)
        {
            double leftStart = 0;
            double topStart = 0;
            double endLeft = 0;
            double endTop = 0;
            double ra;//弧度
            var radius = ArcRadius / 2 - ArcWidth; //环形半径  

            #region 顺时针
            if (Clockwise)
            {
                switch (DrawPosition)
                {
                    case DrawPosition.Left:
                        {
                            leftStart = ArcWidth;
                            topStart = ArcRadius / 2;
                            if (angel <= 90)
                            {
                                ra = (angel) * Math.PI / 180;
                                endLeft = leftStart + radius - Math.Cos(ra) * radius;
                                endTop = topStart - Math.Sin(ra) * radius;
                            }
                            else if (angel <= 180)
                            {
                                ra = (angel - 90) * Math.PI / 180;
                                endLeft = leftStart + radius + Math.Sin(ra) * radius;
                                endTop = topStart - Math.Cos(ra) * radius;
                            }
                            else if (angel <= 270)
                            {
                                ra = (angel - 180) * Math.PI / 180;
                                endLeft = leftStart + radius + Math.Cos(ra) * radius;
                                endTop = topStart + Math.Sin(ra) * radius;
                            }
                            else if (angel < 360)
                            {
                                ra = (angel - 270) * Math.PI / 180;
                                endLeft = leftStart + radius - Math.Sin(ra) * radius;
                                endTop = topStart + Math.Cos(ra) * radius;
                            }
                            else
                            {
                                endLeft = leftStart;
                                endTop = topStart + lastDifference;
                            }
                        }
                        break;
                    case DrawPosition.Top:
                        {
                            leftStart = ArcRadius / 2;
                            topStart = ArcWidth;
                            if (angel <= 90)
                            {
                                ra = (angel) * Math.PI / 180;
                                endLeft = leftStart + +Math.Sin(ra) * radius;
                                endTop = topStart + radius - Math.Cos(ra) * radius;
                            }
                            else if (angel <= 180)
                            {
                                ra = (angel - 90) * Math.PI / 180;
                                endLeft = leftStart + Math.Cos(ra) * radius;
                                endTop = topStart + radius + Math.Sin(ra) * radius;
                            }
                            else if (angel <= 270)
                            {
                                ra = (angel - 180) * Math.PI / 180;
                                endLeft = leftStart - Math.Sin(ra) * radius;
                                endTop = topStart + radius + Math.Cos(ra) * radius;
                            }
                            else if (angel < 360)
                            {
                                ra = (angel - 270) * Math.PI / 180;
                                endLeft = leftStart - Math.Cos(ra) * radius;
                                endTop = topStart + radius - Math.Sin(ra) * radius;
                            }
                            else
                            {
                                endLeft = leftStart - lastDifference;
                                endTop = topStart;
                            }
                        }
                        break;

                    case DrawPosition.Right:
                        {
                            leftStart = ArcRadius - ArcWidth;
                            topStart = ArcRadius / 2;
                            if (angel <= 90)
                            {
                                ra = (angel) * Math.PI / 180;
                                endLeft = leftStart - (radius - Math.Cos(ra) * radius);
                                endTop = topStart + Math.Sin(ra) * radius;
                            }
                            else if (angel <= 180)
                            {
                                ra = (angel - 90) * Math.PI / 180;
                                endLeft = leftStart - radius - Math.Sin(ra) * radius;
                                endTop = topStart + Math.Cos(ra) * radius;
                            }
                            else if (angel <= 270)
                            {
                                ra = (angel - 180) * Math.PI / 180;
                                endLeft = leftStart - radius - Math.Cos(ra) * radius;
                                endTop = topStart - Math.Sin(ra) * radius;
                            }
                            else if (angel < 360)
                            {
                                ra = (angel - 270) * Math.PI / 180;
                                endLeft = leftStart - radius + Math.Sin(ra) * radius;
                                endTop = topStart - Math.Cos(ra) * radius;
                            }
                            else
                            {
                                endLeft = leftStart;
                                endTop = topStart - lastDifference;
                            }
                        }
                        break;
                    case DrawPosition.Bottom:
                        {
                            leftStart = ArcRadius / 2;
                            topStart = ArcRadius - ArcWidth;
                            if (angel <= 90)
                            {
                                ra = (angel) * Math.PI / 180;
                                endLeft = leftStart - Math.Sin(ra) * radius;
                                endTop = topStart - (radius - Math.Cos(ra) * radius);
                            }
                            else if (angel <= 180)
                            {
                                ra = (angel - 90) * Math.PI / 180;
                                endLeft = leftStart - Math.Cos(ra) * radius;
                                endTop = topStart - (radius + Math.Sin(ra) * radius);
                            }
                            else if (angel <= 270)
                            {
                                ra = (angel - 180) * Math.PI / 180;
                                endLeft = leftStart + Math.Sin(ra) * radius;
                                endTop = topStart - (radius + Math.Cos(ra) * radius);
                            }
                            else if (angel < 360)
                            {
                                ra = (angel - 270) * Math.PI / 180;
                                endLeft = leftStart + Math.Cos(ra) * radius;
                                endTop = topStart - (radius - Math.Sin(ra) * radius);
                            }
                            else
                            {
                                endLeft = leftStart + lastDifference;
                                endTop = topStart;
                            }
                        }
                        break;

                }
            }
            #endregion

            #region 逆时针
            else
            {
                switch (DrawPosition)
                {
                    case DrawPosition.Left:
                        {
                            leftStart = ArcWidth;
                            topStart = ArcRadius / 2;
                            if (angel <= 90)
                            {
                                ra = (angel) * Math.PI / 180;
                                endLeft = leftStart + radius - Math.Cos(ra) * radius;
                                endTop = topStart + Math.Sin(ra) * radius;
                            }
                            else if (angel <= 180)
                            {
                                ra = (angel - 90) * Math.PI / 180;
                                endLeft = leftStart + radius + Math.Sin(ra) * radius;
                                endTop = topStart + Math.Cos(ra) * radius;
                            }
                            else if (angel <= 270)
                            {
                                ra = (angel - 180) * Math.PI / 180;
                                endLeft = leftStart + radius + Math.Cos(ra) * radius;
                                endTop = topStart - Math.Sin(ra) * radius;
                            }
                            else if (angel < 360)
                            {
                                ra = (angel - 270) * Math.PI / 180;
                                endLeft = leftStart + radius - Math.Sin(ra) * radius;
                                endTop = topStart - Math.Cos(ra) * radius;
                            }
                            else
                            {
                                endLeft = leftStart;
                                endTop = topStart - lastDifference;
                            }
                        }
                        break;
                    case DrawPosition.Top:
                        {
                            leftStart = ArcRadius / 2;
                            topStart = ArcWidth;
                            if (angel <= 90)
                            {
                                ra = (angel) * Math.PI / 180;
                                endLeft = leftStart - Math.Sin(ra) * radius;
                                endTop = topStart + radius - Math.Cos(ra) * radius;
                            }
                            else if (angel <= 180)
                            {
                                ra = (angel - 90) * Math.PI / 180;
                                endLeft = leftStart - Math.Cos(ra) * radius;
                                endTop = topStart + radius + Math.Sin(ra) * radius;
                            }
                            else if (angel <= 270)
                            {
                                ra = (angel - 180) * Math.PI / 180;
                                endLeft = leftStart + Math.Sin(ra) * radius;
                                endTop = topStart + radius + Math.Cos(ra) * radius;
                            }
                            else if (angel < 360)
                            {
                                ra = (angel - 270) * Math.PI / 180;
                                endLeft = leftStart + Math.Cos(ra) * radius;
                                endTop = topStart + radius - Math.Sin(ra) * radius;
                            }
                            else
                            {
                                endLeft = leftStart + lastDifference;
                                endTop = topStart;
                            }
                        }
                        break;

                    case DrawPosition.Right:
                        {
                            leftStart = ArcRadius - ArcWidth;
                            topStart = ArcRadius / 2;
                            if (angel <= 90)
                            {
                                ra = (angel) * Math.PI / 180;
                                endLeft = (ArcRadius) / 2 + Math.Cos(ra) * radius;
                                endTop = topStart - Math.Sin(ra) * radius;
                            }
                            else if (angel <= 180)
                            {
                                ra = (angel - 90) * Math.PI / 180;
                                endLeft = (ArcRadius) / 2 - Math.Sin(ra) * radius;
                                endTop = topStart - Math.Cos(ra) * radius;
                            }
                            else if (angel <= 270)
                            {
                                ra = (angel - 180) * Math.PI / 180;
                                endLeft = (ArcRadius) / 2 - Math.Cos(ra) * radius;
                                endTop = topStart + Math.Sin(ra) * radius;
                            }
                            else if (angel < 360)
                            {
                                ra = (angel - 270) * Math.PI / 180;
                                endLeft = leftStart - (radius - Math.Sin(ra) * radius);
                                endTop = topStart + Math.Cos(ra) * radius;
                            }
                            else
                            {
                                endLeft = leftStart;
                                endTop = topStart + lastDifference;
                            }
                        }
                        break;
                    case DrawPosition.Bottom:

                        {
                            leftStart = ArcRadius / 2;
                            topStart = ArcRadius - ArcWidth;
                            if (angel <= 90)
                            {
                                ra = (90 - angel) * Math.PI / 180;
                                endLeft = leftStart + Math.Cos(ra) * radius;
                                endTop = topStart - (radius - Math.Sin(ra) * radius);
                            }
                            else if (angel <= 180)
                            {
                                ra = (angel - 90) * Math.PI / 180;
                                endLeft = leftStart + Math.Cos(ra) * radius;
                                endTop = topStart - (radius + Math.Sin(ra) * radius);
                            }
                            else if (angel <= 270)
                            {
                                ra = (angel - 180) * Math.PI / 180;
                                endLeft = leftStart - Math.Sin(ra) * radius;
                                endTop = topStart - (radius + Math.Cos(ra) * radius);
                            }
                            else if (angel < 360)
                            {
                                ra = (angel - 270) * Math.PI / 180;
                                endLeft = leftStart - Math.Cos(ra) * radius;
                                endTop = topStart - (radius - Math.Sin(ra) * radius);
                            }
                            else
                            {
                                endLeft = leftStart - lastDifference;
                                endTop = topStart;
                            }
                        }
                        break;

                }
            }
            #endregion

            var startPoint = new Point(leftStart, topStart);
            var arcEndPoint = new Point(endLeft, endTop);
            return new List<Point> { startPoint, arcEndPoint };
        }

        /// <summary>
        /// 绘制弧度
        /// </summary>
        /// <param name="percentValue"></param>
        private void DrawPercentArc(double percentValue)
        {
            var radius = ArcRadius / 2 - ArcWidth;//环形半径  
            var angel = percentValue * 360; //角度 
            var isLagreCircle = angel > 180; //是否优势弧，即大于180度的弧形 

            var points = GetStartEndPoint(angel);
            var arcStartPoint = points[0];
            var arcEndPoint = points[1];

            var arcSize = new Size(radius, radius);
            var direction = Clockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
            var arcSegment = new ArcSegment(arcEndPoint, arcSize, 0, isLagreCircle, direction, true);
            var pathFigure = new PathFigure()
            {
                StartPoint = arcStartPoint,
                Segments = new PathSegmentCollection { arcSegment }
            };
            var pathGeometry = new PathGeometry()
            {
                Figures = new PathFigureCollection { pathFigure }
            };
            CurrentProcessBar.Data = pathGeometry;

            #region 默认背景
            if (ShowBackGroundArc)
            {
                direction = Clockwise ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;
                BackGroundBar.Data = new PathGeometry
                {
                    Figures = new PathFigureCollection
                  {
                    new PathFigure
                    {
                        StartPoint =arcStartPoint,
                        Segments =new PathSegmentCollection
                        {
                            new ArcSegment(arcEndPoint, arcSize, 0, !isLagreCircle, direction, true)
                        }
                    }
                  }
                };
            }
            else
            {
                BackGroundBar.Data = null;
            }
            #endregion 
        }
    }

    /// <summary>
    /// 绘制位置
    /// </summary>
    public enum DrawPosition
    {
        Left,
        Top,
        Right,
        Bottom
    }
}
