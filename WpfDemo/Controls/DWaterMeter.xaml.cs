using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Utility.Controls;


namespace MeterControls
{
    /// <summary>
    /// 水表控件
    /// </summary>
    public partial class DWaterMeter : IDynamicBase
    {
        #region 属性

        /// <summary>
        /// 显示时要保留的小数位数
        /// </summary>
        public int DecimalNum
        {
            get { return (int)GetValue(DecimalNumProperty); }
            set { SetValue(DecimalNumProperty, value); }
        }
        public static readonly DependencyProperty DecimalNumProperty =
            DependencyProperty.Register("DecimalNum", typeof(int), typeof(DWaterMeter), new PropertyMetadata(DecimalNum_OnChanged));


        /// <summary>
        /// 最大刻度值
        /// </summary>
        [Category("龙坤自定义属性"), Description("最大刻度值")]
        public double DigitText
        {
            get
            {
                return (double)base.GetValue(DWaterMeter.DigitTextProperty);
            }
            set
            {
                base.SetValue(DWaterMeter.DigitTextProperty, value);
            }
        }
        public static readonly DependencyProperty DigitTextProperty;

        /// <summary>
        /// 每一最小单位代表的刻度值
        /// </summary>
        [Category("龙坤自定义属性"), Description("每一最小单位代表的刻度值")]
        public double MinDigit
        {
            get
            {
                return (double)base.GetValue(DWaterMeter.MinDigitProperty);
            }
            set
            {
                base.SetValue(DWaterMeter.MinDigitProperty, value);
            }
        }
        public static readonly DependencyProperty MinDigitProperty;

        /// <summary>
        /// 单位
        /// </summary>
        [Category("龙坤自定义属性"), Description("单位")]
        public string Unit
        {
            get
            {
                return (string)base.GetValue(DWaterMeter.UnitProperty);
            }
            set
            {
                base.SetValue(DWaterMeter.UnitProperty, value);
            }
        }
        public static readonly DependencyProperty UnitProperty;

        /// <summary>
        /// 
        /// </summary>
        private double _oldValue;

        /// <summary>
        /// 
        /// </summary>
        private double _newValue;

        /// <summary>
        /// 水表标题
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DWaterMeter), new PropertyMetadata(string.Empty, TitleChanged));

        #endregion

        #region 构造器
        /// <summary>
        /// 
        /// </summary>
        static DWaterMeter()
        {
            DWaterMeter.DigitTextProperty = DependencyProperty.Register("DigitText", typeof(double), typeof(DWaterMeter), new PropertyMetadata(50.0, new PropertyChangedCallback(DWaterMeter.DigitTextChanged)));
            DWaterMeter.MinDigitProperty = DependencyProperty.Register("MinDigit", typeof(double), typeof(DWaterMeter), new PropertyMetadata(2.0, new PropertyChangedCallback(DWaterMeter.DigitTextChanged)));
            DWaterMeter.UnitProperty = DependencyProperty.Register("Unit", typeof(string), typeof(DWaterMeter), new PropertyMetadata(string.Empty, new PropertyChangedCallback(DWaterMeter.UnitTextChanged)));
        }

        /// <summary>
        /// 
        /// </summary>
        public DWaterMeter()
        {
            this.InitializeComponent();
        }
        #endregion

        #region 事件处理

        private void DigitChanged()
        {
            if (!double.IsNaN(this.DigitText))
            {
                var scaleVisibility = ShowScaleValue ? Visibility.Visible : Visibility.Collapsed;

                this.textblock5.Text = this.DigitText.ToString();
                this.textblock4.Text = (this.DigitText - 5.0 * this.MinDigit).ToString();
                this.textblock3.Text = (this.DigitText - 10.0 * this.MinDigit).ToString();
                this.textblock2.Text = (this.DigitText - 15.0 * this.MinDigit).ToString();
                this.textblock1.Text = (this.DigitText - 20.0 * this.MinDigit).ToString();
                this.textblock5.Visibility = scaleVisibility;
                this.textblock4.Visibility = scaleVisibility;
                this.textblock3.Visibility = scaleVisibility;
                this.textblock2.Visibility = scaleVisibility;
                this.textblock1.Visibility = scaleVisibility;

            }
            this.UpdateUIValue();
        }

        public static void DigitTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DWaterMeter)d).DigitChanged();
        }

        public static void UnitTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DWaterMeter dWaterMeter = d as DWaterMeter;
            dWaterMeter.UnitText.Text = dWaterMeter.Unit;
        }

        public static void TitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DWaterMeter dWaterMeter = d as DWaterMeter;
            //dWaterMeter.title.Text = dWaterMeter.Title;
        }

        /// <summary>
        /// 保留小数位数属性值改变
        /// </summary>
        private static void DecimalNum_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = (DWaterMeter)d;
            uc.UpdateUIValue();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 
        /// </summary>
        public override void UpdateUIValue()
        {
            double num = Value;
            base.ToolTip = null;
            if (!double.IsNaN(num))
            {
                if (num > this.DigitText)
                {
                    num = this.DigitText;
                }
                if (num < this.DigitText - 20.0 * this.MinDigit)
                {
                    num = this.DigitText - 20.0 * this.MinDigit;
                }
                this._newValue = num * 10.8 / this.MinDigit - (this.DigitText - 10.0 * this.MinDigit) * 10.8 / this.MinDigit;
            }
            else
            {
                this._newValue = 0.0;
            }

            string valueText = Math.Round(num, DecimalNum).ToString();
            if (!string.IsNullOrEmpty(ValueFormat))
            {
                valueText = string.Format(ValueFormat, valueText);
            }
            this.value.Text = valueText;
            this.value.Foreground = Brushes.Aqua;


            this.m_DoubleAnimate.From = new double?(this._oldValue);
            this.m_DoubleAnimate.To = new double?(this._newValue);
            this._oldValue = this._newValue;
            this.m_DoubleAnimate.Duration = new Duration(TimeSpan.FromMilliseconds(600.0));
            this.stroyRotateAngle.Begin();
        }

        public override void UpdateUI()
        {
            DigitChanged();
        }
        #endregion
    }
}