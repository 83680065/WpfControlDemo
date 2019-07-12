using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CustomControls.Controls
{
    /// <summary>
    /// 连线控件
    /// </summary>
    public partial class LineUc : UserControl
    {
        #region 属性

        /// <summary>
        /// 画布宽度（像素）
        /// </summary>
        public double CanvasWidth
        {
            get { return (double)GetValue(CanvasWidthProperty); }
            set { SetValue(CanvasWidthProperty, value); }
        }
        public static readonly DependencyProperty CanvasWidthProperty =
            DependencyProperty.Register("CanvasWidth", typeof(double), typeof(LineUc));

        /// <summary>
        /// 画布高度（像素）
        /// </summary>
        public double CanvasHeight
        {
            get { return (double)GetValue(CanvasHeightProperty); }
            set { SetValue(CanvasHeightProperty, value); }
        }
        public static readonly DependencyProperty CanvasHeightProperty =
            DependencyProperty.Register("CanvasHeight", typeof(double), typeof(LineUc));

        /// <summary>
        /// 数据点路径（直接使用Blend生成的Data即可，只能有直线）
        /// </summary>
        public string PathData
        {
            get { return (string)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }
        public static readonly DependencyProperty PathDataProperty =
            DependencyProperty.Register("PathData", typeof(string), typeof(LineUc), new PropertyMetadata(PathData_OnChangedCallback));

        /// <summary>
        /// 箭头数据（箭头1X,箭头1Y,箭头1正北顺时针角度 箭头2X,箭头2Y,箭头2正北顺时针角度 ...）
        /// </summary>
        public string ArrowData
        {
            get { return (string)GetValue(ArrowDataProperty); }
            set { SetValue(ArrowDataProperty, value); }
        }
        public static readonly DependencyProperty ArrowDataProperty =
            DependencyProperty.Register("ArrowData", typeof(string), typeof(LineUc), new PropertyMetadata(ArrowData_OnChangedCallback));

        /// <summary>
        /// 是否从起始点移动到终止点（移动方向：true正向移动，false反向移动）
        /// </summary>
        public bool IsMoveFromStartToEnd
        {
            get { return (bool)GetValue(IsMoveFromStartToEndProperty); }
            set { SetValue(IsMoveFromStartToEndProperty, value); }
        }
        public static readonly DependencyProperty IsMoveFromStartToEndProperty =
            DependencyProperty.Register("IsMoveFromStartToEnd", typeof(bool), typeof(LineUc), new PropertyMetadata(IsMoveFromStartToEnd_OnChangedCallback));

        /// <summary>
        /// 正常颜色
        /// </summary>
        public Brush NomarlColorBrsuh
        {
            get { return (Brush)GetValue(NomarlColorBrsuhProperty); }
            set { SetValue(NomarlColorBrsuhProperty, value); }
        }
        public static readonly DependencyProperty NomarlColorBrsuhProperty =
            DependencyProperty.Register("NomarlColorBrsuh", typeof(Brush), typeof(LineUc), new PropertyMetadata(Brushes.SpringGreen));

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool? IsChecked
        {
            get { return (bool?)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool?), typeof(LineUc), new PropertyMetadata(IsChecked_OnChangedCallback));

        /// <summary>
        /// 线的背景颜色
        /// </summary>
        public Brush LineBackground
        {
            get { return (Brush)GetValue(LineBackgroundProperty); }
            set { SetValue(LineBackgroundProperty, value); }
        }
        public static readonly DependencyProperty LineBackgroundProperty =
            DependencyProperty.Register("LineBackground", typeof(Brush), typeof(LineUc), new PropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 转角内切圆半径（像素）
        /// </summary>
        private const double CORNER_RADIUS = 20;

        /// <summary>
        /// 状态和颜色的对应字典
        /// </summary>
        private Dictionary<string, string> m_StateColorDic { get; set; }

        /// <summary>
        /// 当前线流动的动画
        /// </summary>
        private Storyboard m_LineStoryboard;

        /// <summary>
        /// 当前的Path
        /// </summary>
        private Path m_CurrentPath;

        /// <summary>
        /// 当前的背景Path
        /// </summary>
        private Path m_CurrentBackgroundPath;

        /// <summary>
        /// 线上的方向箭头的集合
        /// </summary>
        private List<LineArrowUc> m_ArrowList;

        /// <summary>
        /// 用户输入的路径点
        /// </summary>
        private List<Point> m_PathPoints;

        /// <summary>
        /// 当前程序正在控制的分组的集合（防止用户点击导致IsChecked改变造成死循环）
        /// </summary>
        private static HashSet<string> m_BackgroundControlGroups;

        #endregion

        #region 构造器

        /// <summary>
        /// 构造器（静态）
        /// </summary>
        static LineUc()
        {
            m_BackgroundControlGroups = new HashSet<string>();
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public LineUc()
        {
            InitializeComponent();

            m_LineStoryboard = null;
            m_CurrentPath = null;
            m_CurrentBackgroundPath = null;
            PathData = "";
            IsMoveFromStartToEnd = true;
            // LineBackground = "#114092";
            GroupName = "Default";

            this.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 控件左键点击事件
        /// </summary>
        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            DependencyObject parent;
            List<Tuple<LineUc, double?>> disTempList;
            double minDis;

            parent = LogicalTreeHelper.GetParent(this);
            disTempList = new List<Tuple<LineUc, double?>>();
            foreach (var objChild in LogicalTreeHelper.GetChildren(parent))
            {
                LineUc child;

                if (objChild is LineUc)
                {
                    child = (LineUc)objChild;
                }
                else
                {
                    continue;
                }

                if (child.GroupName != this.GroupName)
                {
                    continue;
                }

                disTempList.Add(new Tuple<LineUc, double?>(child, child.GetMinDistanceToPoint(Mouse.GetPosition(child.MainCanvas))));
                Panel.SetZIndex(child, 9);
                m_BackgroundControlGroups.Add(this.GroupName);
                child.IsChecked = false;
                m_BackgroundControlGroups.Remove(this.GroupName);
            }

            if (disTempList.All(tup => tup.Item2.HasValue == false))
            {
                if (disTempList.Count > 0)
                {
                    Panel.SetZIndex(disTempList[0].Item1, 10);
                    m_BackgroundControlGroups.Add(this.GroupName);
                    disTempList[0].Item1.IsChecked = false;
                    m_BackgroundControlGroups.Remove(this.GroupName);
                }
                return;
            }

            minDis = disTempList.Where(tup => tup.Item2.HasValue).Min(tup => tup.Item2.Value);

            var checkedItem = disTempList.FirstOrDefault(it => it.Item2.HasValue && Math.Abs(it.Item2.Value - minDis) < 0.000001);
            if (checkedItem != null)
            {
                Panel.SetZIndex(checkedItem.Item1, 10);
                m_BackgroundControlGroups.Add(this.GroupName);
                checkedItem.Item1.IsChecked = false;
                m_BackgroundControlGroups.Remove(this.GroupName);
            }
        }

        /// <summary>
        /// IsChecked属性值改变事件
        /// </summary>
        private static void IsChecked_OnChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineUc uc;

            uc = (LineUc)d;

            if (m_BackgroundControlGroups.Contains(uc.GroupName))
            {
                return;
            }
            uc.OnPreviewMouseLeftButtonUp(null, null);
        }



        /// <summary>
        /// 路径数据改变回调方法
        /// </summary>
        private static void PathData_OnChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineUc uc;
            BackgroundWorker worker;
            string pathDataStr;
            List<Point> linePoints;
            List<PathSegment> segments;

            uc = (LineUc)d;
            pathDataStr = (string)e.NewValue;

            //清空当前界面
            if (uc.m_LineStoryboard != null)
            {
                uc.m_LineStoryboard.Stop();
            }
            if (uc.m_CurrentPath != null)
            {
                uc.MainCanvas.Children.Remove(uc.m_CurrentPath);
            }
            if (uc.m_CurrentBackgroundPath != null)
            {
                uc.MainCanvas.Children.Remove(uc.m_CurrentBackgroundPath);
            }
            uc.m_CurrentPath = null;
            uc.m_CurrentBackgroundPath = null;

            #region 解析字符串为Point集合

            pathDataStr = pathDataStr.Replace("M", "").Replace("m", "").Replace("L", "").Replace("l", "").Replace("Z", "").Replace("z", "");
            var temp = pathDataStr.Split(' ');

            linePoints = new List<Point>();
            foreach (string pointStr in temp)
            {
                Point pt;
                double x;
                double y;

                var temp1 = pointStr.Split(',');

                if (temp1.Length != 2)
                {
                    continue;
                }

                if (double.TryParse(temp1[0].Trim(), out x) == false)
                {
                    continue;
                }

                if (double.TryParse(temp1[1].Trim(), out y) == false)
                {
                    continue;
                }

                pt = new Point(x, y);
                linePoints.Add(pt);
            }
            uc.m_PathPoints = linePoints;
            #endregion

            #region 生成圆滑拐角
            if (linePoints.Count < 2)
            {
                #region 点数不足2点
                segments = null;
                #endregion
            }
            else if (linePoints.Count == 2)
            {
                #region 只有2点
                segments = new List<PathSegment> { new LineSegment(linePoints.Last(), true) };
                #endregion
            }
            else
            {
                #region 多于2点
                segments = new List<PathSegment>();
                //循环点数（3个点处理一次）
                for (int i = 0; i < linePoints.Count - 2; i++)
                {
                    Point pt0;  //第一点
                    Point pt1;  //拐点
                    Point pt2;  //第二点
                    double ang; //拐点的拐角角度

                    pt0 = linePoints[i];
                    pt1 = linePoints[i + 1];
                    pt2 = linePoints[i + 2];
                    ang = CalVector2Angle(pt0, pt1, pt2);

                    if (Math.Abs(ang - Math.PI) < 0.000001
                        || Math.Abs(ang) < 0.000001)
                    {
                        #region 拐角是0度或180度
                        segments.Add(new LineSegment(pt1, true));
                        #endregion
                    }
                    else
                    {
                        #region 拐角在0度和180度之间
                        double removeDis;    //为了圆滑拐角要在两侧删除的直线长度
                        Point cornerPoint0;  //圆滑拐角于第一个线段的交点
                        Point cornerPoint1;  //圆滑拐角于第二个线段的交点

                        removeDis = Math.Abs(Math.Tan((Math.PI - ang) / 2) * CORNER_RADIUS);

                        #region 计算圆滑拐角的第一个交点
                        if (Math.Abs(pt0.X - pt1.X) < 0.000001)
                        {
                            #region 竖线
                            cornerPoint0 = pt0.Y < pt1.Y
                                              ? new Point(pt0.X, pt1.Y - removeDis)
                                              : new Point(pt0.X, pt1.Y + removeDis);
                            #endregion
                        }
                        else if (Math.Abs(pt0.Y - pt1.Y) < 0.000001)
                        {
                            #region 横线
                            cornerPoint0 = pt0.X < pt1.X
                                               ? new Point(pt1.X - removeDis, pt0.Y)
                                               : new Point(pt1.X + removeDis, pt0.Y);
                            #endregion
                        }
                        else
                        {
                            #region 斜线
                            double ang1;     //线段与X轴的夹角
                            double removeX;  //要删除的X轴距离
                            double newX;     //与拐角的焦点X
                            double newY;     //与拐角的焦点Y

                            ang1 = Math.Atan(Math.Abs((pt1.Y - pt0.Y) / (pt1.X - pt0.X)));
                            removeX = Math.Abs(Math.Cos(ang1) * removeDis);
                            newX = pt0.X < pt1.X
                                ? pt1.X - removeX
                                : pt1.X + removeX;
                            newY = (newX - pt0.X) / (pt1.X - pt0.X) * (pt1.Y - pt0.Y) + pt0.Y;
                            cornerPoint0 = new Point(newX, newY);
                            #endregion
                        }
                        #endregion

                        #region 计算圆滑拐角的第二个交点
                        if (Math.Abs(pt1.X - pt2.X) < 0.000001)
                        {
                            #region 竖线
                            cornerPoint1 = pt1.Y < pt2.Y
                                                 ? new Point(pt1.X, pt1.Y + removeDis)
                                                 : new Point(pt1.X, pt1.Y - removeDis);
                            #endregion
                        }
                        else if (Math.Abs(pt1.Y - pt2.Y) < 0.000001)
                        {
                            #region 横线
                            cornerPoint1 = pt1.X < pt2.X
                                                      ? new Point(pt1.X + removeDis, pt1.Y)
                                                      : new Point(pt1.X - removeDis, pt1.Y);
                            #endregion
                        }
                        else
                        {
                            #region 斜线
                            double ang1;
                            double removeX;
                            double newX;
                            double newY;

                            ang1 = Math.Atan(Math.Abs((pt2.Y - pt1.Y) / (pt2.X - pt1.X)));
                            removeX = Math.Abs(Math.Cos(ang1) * removeDis);
                            newX = pt1.X < pt2.X
                                ? pt1.X + removeX
                                : pt1.X - removeX;
                            newY = (newX - pt1.X) / (pt2.X - pt1.X) * (pt2.Y - pt1.Y) + pt1.Y;
                            cornerPoint1 = new Point(newX, newY);
                            #endregion
                        }
                        #endregion

                        //加入第一条直线
                        segments.Add(new LineSegment(cornerPoint0, true));

                        {
                            SweepDirection direction;  //圆弧顺时针还是逆时针

                            #region 计算圆弧是顺时针还是逆时针
                            if (Math.Abs(pt0.X - cornerPoint0.X) < 0.000001)
                            {
                                #region 第一条线段是竖线

                                if (pt0.Y > cornerPoint0.Y)
                                {
                                    #region 向上

                                    direction = cornerPoint1.X < pt0.X
                                        ? SweepDirection.Counterclockwise
                                        : SweepDirection.Clockwise;

                                    #endregion
                                }
                                else
                                {
                                    #region 向下
                                    direction = cornerPoint1.X < pt0.X
                                        ? SweepDirection.Clockwise
                                        : SweepDirection.Counterclockwise;
                                    #endregion
                                }

                                #endregion
                            }
                            else if (Math.Abs(pt0.Y - cornerPoint0.Y) < 0.000001)
                            {
                                #region 第一条线段是横线

                                if (pt0.X > cornerPoint0.X)
                                {
                                    #region 向左
                                    direction = pt0.Y > cornerPoint1.Y
                                        ? SweepDirection.Clockwise
                                        : SweepDirection.Counterclockwise;
                                    #endregion
                                }
                                else
                                {
                                    #region 向右
                                    direction = pt0.Y > cornerPoint1.Y
                                        ? SweepDirection.Counterclockwise
                                        : SweepDirection.Clockwise;
                                    #endregion
                                }

                                #endregion
                            }
                            else
                            {
                                #region 第一条线段是斜线
                                double k;
                                double newY;

                                k = (cornerPoint0.Y - pt0.Y) / (cornerPoint0.X - pt0.X);
                                newY = (cornerPoint1.X - pt0.X) * k + pt0.Y;
                                if (k > 0)
                                {
                                    #region 一三象限斜线
                                    if (pt0.X < cornerPoint0.X)
                                    {
                                        #region 向右上
                                        direction = newY > cornerPoint1.Y
                                            ? SweepDirection.Counterclockwise
                                            : SweepDirection.Clockwise;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 向左下
                                        direction = newY > cornerPoint1.Y
                                            ? SweepDirection.Clockwise
                                            : SweepDirection.Counterclockwise;
                                        #endregion
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 二四象限斜线
                                    if (pt0.X < cornerPoint0.X)
                                    {
                                        #region 向右下
                                        direction = newY > cornerPoint1.Y
                                            ? SweepDirection.Counterclockwise
                                            : SweepDirection.Clockwise;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 向左上
                                        direction = newY > cornerPoint1.Y
                                            ? SweepDirection.Clockwise
                                            : SweepDirection.Counterclockwise;
                                        #endregion
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion

                            //加入圆滑拐角
                            segments.Add(new ArcSegment(cornerPoint1, new Size(CORNER_RADIUS, CORNER_RADIUS), 0, false, direction, true));
                        }

                        #endregion
                    }
                }
                //加入结尾的最后一条直线
                segments.Add(new LineSegment(linePoints.Last(), true));
                #endregion
            }
            #endregion

            #region 生成路径并加载到屏幕上

            {
                Path backgroundPath;
                Path path;
                Storyboard storyboard;

                if (segments == null)
                {
                    return;
                }

                {
                    Binding binding;

                    backgroundPath = new Path()
                    {
                        Data = new PathGeometry { Figures = new PathFigureCollection() { new PathFigure(linePoints[0], segments, false) } },
                        StrokeThickness = 8
                    };
                    binding = new Binding("LineBackground") { Mode = BindingMode.OneWay, Source = uc };
                    backgroundPath.SetBinding(Path.StrokeProperty, binding);
                    Panel.SetZIndex(backgroundPath, 0);
                }


                path = new Path
                {
                    Data = new PathGeometry { Figures = new PathFigureCollection() { new PathFigure(linePoints[0], segments, false) } },
                    StrokeThickness = 8,
                    StrokeDashArray = new DoubleCollection(new List<double>() { 0.5, 0.25 }),
                };

                {
                    Binding binding;

                    binding = new Binding("NomarlColorBrsuh") { Mode = BindingMode.OneWay, Source = uc };
                    path.SetBinding(Path.StrokeProperty, binding);
                }
                path.Fill = Brushes.Transparent;
                Panel.SetZIndex(path, 1);

                #region 动画
                if (uc.m_LineStoryboard != null)
                {
                    uc.m_LineStoryboard.Stop();
                }
                storyboard = uc.IsMoveFromStartToEnd
                    ? (Storyboard)uc.Resources["LineMoveStoryBoard"]
                    : (Storyboard)uc.Resources["LineMoveBackStoryBoard"];
                Storyboard.SetTarget(storyboard, path);
                uc.m_LineStoryboard = storyboard;
                storyboard.Begin();
                #endregion

                uc.m_CurrentPath = path;
                uc.m_CurrentBackgroundPath = backgroundPath;
                uc.MainCanvas.Children.Add(backgroundPath);
                uc.MainCanvas.Children.Add(path);
            }
            #endregion
        }

        /// <summary>
        /// 箭头数据改变回调方法
        /// </summary>
        private static void ArrowData_OnChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineUc uc;
            string arrowData;

            uc = (LineUc)d;
            arrowData = (string)e.NewValue;


            #region 删除原有箭头
            if (uc.m_ArrowList != null)
            {
                foreach (LineArrowUc arrowUc in uc.m_ArrowList)
                {
                    uc.MainCanvas.Children.Remove(arrowUc);
                }
            }
            uc.m_ArrowList = new List<LineArrowUc>();
            #endregion

            if (string.IsNullOrWhiteSpace(arrowData))
            {
                return;
            }

            #region 创建新的箭头控件
            var temp = arrowData.Split(' ');
            foreach (string arrow in temp)
            {
                LineArrowUc arrowUc;
                double x;
                double y;
                double angle;

                var temp1 = arrow.Split(',');
                if (temp1.Length != 3)
                {
                    continue;
                }

                if (double.TryParse(temp1[0], out x) == false)
                {
                    continue;
                }

                if (double.TryParse(temp1[1], out y) == false)
                {
                    continue;
                }

                if (double.TryParse(temp1[2], out angle) == false)
                {
                    continue;
                }

                arrowUc = new LineArrowUc();
                {
                    Binding binding;

                    binding = new Binding("NomarlColorBrsuh") { Mode = BindingMode.OneWay, Source = uc };
                    arrowUc.SetBinding(LineArrowUc.ColorProperty, binding);
                }
                Canvas.SetLeft(arrowUc, x - arrowUc.Width / 2);
                Canvas.SetTop(arrowUc, y - arrowUc.Width / 2);
                arrowUc.RenderTransformOrigin = new Point(0.5, 0.5);
                arrowUc.RenderTransform = new RotateTransform(angle);
                Panel.SetZIndex(arrowUc, 2);
                uc.m_ArrowList.Add(arrowUc);
            }
            #endregion

            #region 将箭头放在界面上
            foreach (LineArrowUc arrowUc in uc.m_ArrowList)
            {
                uc.MainCanvas.Children.Add(arrowUc);
            }
            #endregion
        }

        /// <summary>
        /// 流动方向改变回调
        /// </summary>
        private static void IsMoveFromStartToEnd_OnChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineUc uc;
            bool isMoveFromStartToEnd;
            Storyboard storyboard;

            uc = (LineUc)d;
            isMoveFromStartToEnd = (bool)e.NewValue;

            if (uc.m_CurrentPath == null)
            {
                return;
            }

            if (uc.m_LineStoryboard != null)
            {
                uc.m_LineStoryboard.Stop();
            }
            storyboard = isMoveFromStartToEnd
                ? (Storyboard)uc.Resources["LineMoveStoryBoard"]
                : (Storyboard)uc.Resources["LineMoveBackStoryBoard"];
            Storyboard.SetTarget(storyboard, uc.m_CurrentPath);
            uc.m_LineStoryboard = storyboard;
            storyboard.Begin();
        }



        #endregion

        #region 方法
        /// <summary>
        /// 计算2个线段的夹角
        /// </summary>
        /// <param name="pt0">线段一起点</param>
        /// <param name="pt1">两线段交点</param>
        /// <param name="pt2">线段二终点</param>
        /// <returns>两线段夹角数值（不带符号）（弧度）</returns>
        public static double CalVector2Angle(Point pt0, Point pt1, Point pt2)
        {
            Point a1;    //向量1起点
            Point a2;    //向量1终点
            Point b1;    //向量2起点
            Point b2;    //向量2终点
            double aAngle;  //向量1的角度
            double bAngle;  //向量2的角度
            double result;  //两向量夹角

            a1 = pt1;
            a2 = pt0;
            b1 = pt1;
            b2 = pt2;

            aAngle = Math.Atan2(a2.Y - a1.Y, a2.X - a1.X);
            bAngle = Math.Atan2(b2.Y - b1.Y, b2.X - b1.X);
            result = Math.Abs(bAngle - aAngle);

            return result;
        }

        /// <summary>
        /// 获取Path与鼠标点击位置的最近距离
        /// </summary>
        /// <param name="mousePosition"></param>
        /// <returns></returns>
        private double? GetMinDistanceToPoint(Point mousePosition)
        {
            double? minDistance;

            if (this.m_PathPoints == null
                || this.m_PathPoints.Count == 0)
            {
                return null;
            }

            minDistance = null;

            for (int i = 0; i < this.m_PathPoints.Count - 1; i++)
            {
                Point pt0;
                Point pt1;

                pt0 = this.m_PathPoints[0];
                pt1 = this.m_PathPoints[1];

                var temp = PointToLine(pt0.X, pt0.Y, pt1.X, pt1.Y, mousePosition.X, mousePosition.Y);
                if (minDistance == null
                    || minDistance.Value > temp)
                {
                    minDistance = temp;
                }
            }

            return minDistance;
        }

        /// <summary>
        /// 计算点到线段的距离
        /// </summary>
        /// <param name="x1">线段点1X</param>
        /// <param name="y1">线段点1Y</param>
        /// <param name="x2">线段点2X</param>
        /// <param name="y2">线段点2Y</param>
        /// <param name="x0">测距点X</param>
        /// <param name="y0">测距点Y</param>
        /// <returns>距离</returns>
        public static double PointToLine(double x1, double y1, double x2, double y2, double x0,
            double y0)
        {
            double space = 0;
            double a, b, c;
            a = LineSpace(x1, y1, x2, y2);// 线段的长度  
            b = LineSpace(x1, y1, x0, y0);// (x1,y1)到点的距离  
            c = LineSpace(x2, y2, x0, y0);// (x2,y2)到点的距离  
            if (c <= 0.000001 || b <= 0.000001)
            {
                space = 0;
                return space;
            }
            if (a <= 0.000001)
            {
                space = b;
                return space;
            }
            if (c * c >= a * a + b * b)
            {
                space = b;
                return space;
            }
            if (b * b >= a * a + c * c)
            {
                space = c;
                return space;
            }
            double p = (a + b + c) / 2;// 半周长  
            double s = Math.Sqrt(p * (p - a) * (p - b) * (p - c));// 海伦公式求面积  
            space = 2 * s / a;// 返回点到线的距离（利用三角形面积公式求高）  
            return space;
        }

        /// <summary>
        /// 计算两点间距离
        /// </summary>
        /// <param name="x1">点1X</param>
        /// <param name="y1">点1Y</param>
        /// <param name="x2">点2X</param>
        /// <param name="y2">点2Y</param>
        /// <returns>两点间的距离</returns>

        public static double LineSpace(double x1, double y1, double x2, double y2)
        {
            double lineLength = 0;
            lineLength = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2)
                                   * (y1 - y2));
            return lineLength;
        }

        #endregion
    }
}
