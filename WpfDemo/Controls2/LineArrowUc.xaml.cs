using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControls.Controls
{
    /// <summary>
    /// 线上的方向箭头控件
    /// </summary>
    public partial class LineArrowUc : UserControl
    {
        #region 属性
        /// <summary>
        /// 箭头的颜色
        /// </summary>
        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(LineArrowUc)); 
        #endregion

        #region 构造器
        /// <summary>
        /// 构造器
        /// </summary>
        public LineArrowUc()
        {
            InitializeComponent();
        } 
        #endregion
    }
}
