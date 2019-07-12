using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Utility.Controls;


namespace MeterControls
{
    public partial class DAnimatedDial360 : IDynamicBase
    {
        private const double m_Beating = 150;

        public readonly static DependencyProperty FromProperty;

        public readonly static DependencyProperty ToProperty;


        /// <summary>
        /// 显示时要保留的小数位数
        /// </summary>
        public int DecimalNum
        {
            get { return (int)GetValue(DecimalNumProperty); }
            set { SetValue(DecimalNumProperty, value); }
        }

        public static readonly DependencyProperty DecimalNumProperty =
            DependencyProperty.Register("DecimalNum", typeof(int), typeof(DAnimatedDial360), new PropertyMetadata(DecimalNum_OnChanged));

        [Category("龙坤自定义属性"), Description("指针坐标From值")]
        public double From
        {
            get
            {
                return (double)base.GetValue(DAnimatedDial360.FromProperty);
            }
            set
            {
                base.SetValue(DAnimatedDial360.FromProperty, value);
            }
        }
        [Category("龙坤自定义属性"), Description("指针坐标To值")]
        public double To
        {
            get
            {
                return (double)base.GetValue(DAnimatedDial360.ToProperty);
            }
            set
            {
                base.SetValue(DAnimatedDial360.ToProperty, value);
            }
        }

        /// <summary>
        /// 仪表单位
        /// </summary>
        [Category("龙坤自定义属性"), Description("仪表单位")]
        public string Unit
        {
            get
            {
                return (string)base.GetValue(DAnimatedDial360.UnitProperty);
            }
            set
            {
                base.SetValue(DAnimatedDial360.UnitProperty, value);
            }
        }
        public static readonly DependencyProperty UnitProperty;

        public string Title { get; set; }
        static DAnimatedDial360()
        {
            DAnimatedDial360.FromProperty = DependencyProperty.Register("From", typeof(double), typeof(DAnimatedDial360), new PropertyMetadata(0.0, new PropertyChangedCallback(DAnimatedDial360.RangeChanged)));
            DAnimatedDial360.ToProperty = DependencyProperty.Register("To", typeof(double), typeof(DAnimatedDial360), new PropertyMetadata(100.0, new PropertyChangedCallback(DAnimatedDial360.RangeChanged)));
            DAnimatedDial360.UnitProperty = DependencyProperty.Register("Unit", typeof(string), typeof(DAnimatedDial360), new PropertyMetadata(string.Empty, new PropertyChangedCallback(DAnimatedDial360.UnitChanged)));
        }

        private static void UnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DAnimatedDial360 qAnimatedDial360 = (DAnimatedDial360)d;
            qAnimatedDial360.tbUnit.Text = qAnimatedDial360.Unit;
        }

        /// <summary>
        /// 
        /// </summary>
        public DAnimatedDial360()
        {
            this.InitializeComponent();

            DecimalNum = 2;
            this.Loaded += DAnimatedDial360_Loaded;
        }

        void DAnimatedDial360_Loaded(object sender, RoutedEventArgs e)
        {
            //this.title.Text = Title;
        }
        private void RangeChange()
        {
            int num = 10;
            double num2 = (this.To - this.From) / (double)num;
            for (int i = 0; i <= num; i++)
            {
                string name = string.Format("_txt{0}", i);
                var textBlock = base.FindName(name) as TextBlock;
                textBlock.Visibility = ShowScaleValue ? Visibility.Visible : Visibility.Collapsed;
                if (textBlock != null)
                {
                    double value = this.From + (double)i * num2;
                    textBlock.Text = Math.Round(value, 2).ToString();
                }
            }
        }
        private static void RangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DAnimatedDial360)d).RangeChange();
        }

        /// <summary>
        /// 保留小数位数属性值改变
        /// </summary>
        private static void DecimalNum_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DAnimatedDial360 uc;

            uc = (DAnimatedDial360)d;
            uc.UpdateUIValue();
        }

        public override void UpdateUIValue()
        {
            double num = Value;

            var doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.75));

            base.ToolTip = null;
            if (num > this.To)
            {
                num = this.To;
            }
            if (num < this.From)
            {
                num = this.From;
            }
            doubleAnimation.To = new double?(-150.0 + (num - this.From) / (this.To - this.From) * 2.0 * 150.0);
            this.RTPointer.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);

            string valueText = Math.Round(num, DecimalNum).ToString();
            if (!string.IsNullOrEmpty(ValueFormat))
            {
                valueText = string.Format(ValueFormat, valueText);
            }
            this.value.Text = valueText;
            this.value.Foreground = Brushes.Aqua;
        }

        public override void UpdateUI()
        {
            RangeChange();
        }
    }
}