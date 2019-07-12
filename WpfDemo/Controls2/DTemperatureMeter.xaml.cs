using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Utility.Controls;

namespace MeterControls
{
    /// <summary>
    /// 圆形模拟量控件
    /// </summary>
	public partial class DTemperatureMeter : IDynamicBase
    {
        #region 属性

        /// <summary>
        /// 最大刻度值
        /// </summary>
        [Category("龙坤自定义属性"), Description("最大刻度值")]
        public double DigitText
        {
            get
            {
                return (double)base.GetValue(DTemperatureMeter.DigitTextProperty);
            }
            set
            {
                base.SetValue(DTemperatureMeter.DigitTextProperty, value);
            }
        }
        public readonly static DependencyProperty DigitTextProperty;

        /// <summary>
        /// 每一最小单位代表刻度值
        /// </summary>
        [Category("龙坤自定义属性"), Description("每一最小单位代表刻度值")]
        public double MinDigit
        {
            get
            {
                return (double)base.GetValue(DTemperatureMeter.MinDigitProperty);
            }
            set
            {
                base.SetValue(DTemperatureMeter.MinDigitProperty, value);
            }
        }
        public readonly static DependencyProperty MinDigitProperty;

        /// <summary>
        /// 单位
        /// </summary>
        [Category("龙坤自定义属性"), Description("单位")]
        public string Unit
        {
            get
            {
                return (string)base.GetValue(DTemperatureMeter.UnitProperty);
            }
            set
            {
                base.SetValue(DTemperatureMeter.UnitProperty, value);
            }
        }
        public readonly static DependencyProperty UnitProperty;

        /// <summary>
        /// 上一次的数值
        /// </summary>
        private double _oldValue;

        #endregion

        #region 构造器

        /// <summary>
        /// 静态
        /// </summary>
        static DTemperatureMeter()
        {
            DTemperatureMeter.DigitTextProperty = DependencyProperty.Register("DigitText", typeof(double), typeof(DTemperatureMeter), new PropertyMetadata(50.0, new PropertyChangedCallback(DTemperatureMeter.DigitTextChanged)));
            DTemperatureMeter.MinDigitProperty = DependencyProperty.Register("MinDigit", typeof(double), typeof(DTemperatureMeter), new PropertyMetadata(2.0, new PropertyChangedCallback(DTemperatureMeter.DigitTextChanged)));
            DTemperatureMeter.UnitProperty = DependencyProperty.Register("Unit", typeof(string), typeof(DTemperatureMeter), new PropertyMetadata(string.Empty, new PropertyChangedCallback(DTemperatureMeter.UnitTextChanged)));
        }

        /// <summary>
        /// 动态
        /// </summary>
        public DTemperatureMeter()
        {
            this.InitializeComponent();
            textblockM.Foreground = new SolidColorBrush(base.NormalColor);
        }

        #endregion

        #region 事件处理和回调方法

        /// <summary>
        /// 
        /// </summary>
        public static void DigitTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DTemperatureMeter).DigitChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void UnitTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DTemperatureMeter dTemperatureMeter = d as DTemperatureMeter;
            dTemperatureMeter.UnitText.Text = dTemperatureMeter.Unit.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateUI()
        {
            double num = Value;
            double newValue;
            if (num > this.DigitText)
            {
                num = this.DigitText;
            }
            if (num < this.DigitText - 40.0 * this.MinDigit)
            {
                num = this.DigitText - 40.0 * this.MinDigit;
            }
            this.textblockM.Text = num.ToString("0.0");
            newValue = num * 6.0 / this.MinDigit - (this.DigitText - 20.0 * this.MinDigit) * 6.0 / this.MinDigit;
            this.m_DoubleAnimate.From = this._oldValue;
            this.m_DoubleAnimate.To = newValue;
            this._oldValue = newValue;
            this.m_DoubleAnimate.Duration = new Duration(TimeSpan.FromMilliseconds(600.0));
            this.stroyRotateAngle.Begin();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 
        /// </summary>
        private void DigitChanged()
        {
            if (!double.IsNaN(this.DigitText))
            {
                this.textblock9.Text = this.DigitText.ToString();
                this.textblock8.Text = (this.DigitText - 5.0 * this.MinDigit).ToString();
                this.textblock7.Text = (this.DigitText - 10.0 * this.MinDigit).ToString();
                this.textblock6.Text = (this.DigitText - 15.0 * this.MinDigit).ToString();
                this.textblock5.Text = (this.DigitText - 20.0 * this.MinDigit).ToString();
                this.textblock4.Text = (this.DigitText - 25.0 * this.MinDigit).ToString();
                this.textblock3.Text = (this.DigitText - 30.0 * this.MinDigit).ToString();
                this.textblock2.Text = (this.DigitText - 35.0 * this.MinDigit).ToString();
                this.textblock1.Text = (this.DigitText - 40.0 * this.MinDigit).ToString();
            }
            this.UpdateUI();
        }

        public override void UpdateUIValue()
        {
            UpdateUI();
        }

        #endregion
    }
}