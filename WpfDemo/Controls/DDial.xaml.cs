using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Utility.Controls;


namespace MeterControls
{
    /// <summary>
    /// 油表控件
    /// </summary>
    public partial class DDial : IDynamicBase
    {
        #region 属性

        /// <summary>
        /// 指针坐标From值
        /// </summary>
        [Category("龙坤自定义属性"), Description("指针坐标From值")]
        public double From
        {
            get
            {
                return (double)base.GetValue(DDial.FromProperty);
            }
            set
            {
                base.SetValue(DDial.FromProperty, value);
            }
        }
        public static readonly DependencyProperty FromProperty;

        /// <summary>
        /// 指针坐标To值
        /// </summary>
        [Category("龙坤自定义属性"), Description("指针坐标To值")]
        public double To
        {
            get
            {
                return (double)base.GetValue(DDial.ToProperty);
            }
            set
            {
                base.SetValue(DDial.ToProperty, value);
            }
        }
        public static readonly DependencyProperty ToProperty;

        /// <summary>
        /// 油表值单位
        /// </summary>
        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(string), typeof(DDial), new PropertyMetadata(string.Empty, UnitChanged));


        /// <summary>
        /// 油表标题
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DDial), new PropertyMetadata(string.Empty, TitleChanged));

        #endregion

        #region 构造器
        /// <summary>
        /// 静态构造器
        /// </summary>
        static DDial()
        {
            DDial.FromProperty = DependencyProperty.Register("From", typeof(double), typeof(DDial), new PropertyMetadata(0.0, new PropertyChangedCallback(DDial.FromChanged)));
            DDial.ToProperty = DependencyProperty.Register("To", typeof(double), typeof(DDial), new PropertyMetadata(100.0, new PropertyChangedCallback(DDial.ToChanged)));
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public DDial()
        {
            this.InitializeComponent();
        }
        #endregion

        #region 事件处理

        /// <summary>
        /// 标题值改变
        /// </summary>
        private static void TitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DDial dDial = (DDial)d;
            dDial.title.Text = dDial.Title;
        }

        /// <summary>
        /// 单位值改变
        /// </summary>
        private static void UnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DDial dDial = (DDial)d;
            dDial.tbUnit.Text = dDial.Unit;
        }

        /// <summary>
        /// From值改变
        /// </summary>
        private static void FromChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DDial dDial = (DDial)d;
            dDial.tbFrom.Text = dDial.From.ToString();
            dDial.tbMiddle.Text = ((dDial.From + dDial.To) / 2.0).ToString();
        }

        /// <summary>
        /// To值改变
        /// </summary>
        private static void ToChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DDial dDial = (DDial)d;
            dDial.tbTo.Text = dDial.To.ToString();
            dDial.tbMiddle.Text = ((dDial.From + dDial.To) / 2.0).ToString();
        }

        #endregion

        #region 方法
        /// <summary>
        /// 
        /// </summary>
        public override void UpdateUIValue()
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.75));
            base.ToolTip = null;
            double num = Value;
            if (num > this.To)
            {
                num = this.To;
            }
            if (num < this.From)
            {
                num = this.From;
            }

            doubleAnimation.To = new double?(-75.0 + (num - this.From) / (this.To - this.From) * 2.0 * 75.0);
            this.RTPointer.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
            string valueText = num.ToString();
            if (!string.IsNullOrEmpty(ValueFormat))
            {
                valueText = string.Format(ValueFormat, valueText);
            }
            this.value.Text = valueText;
            this.value.Foreground = Brushes.Aqua;
        }

        public override void UpdateUI()
        {

        }
        #endregion
    }
}