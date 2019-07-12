
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Utility.Controls
{
    public abstract class IDynamicBase : UserControl
    {
        public readonly static DependencyProperty MinAlarmProperty;

        public readonly static DependencyProperty MaxAlarmProperty;

        public readonly static DependencyProperty MinFatalProperty;

        public readonly static DependencyProperty MaxFatalProperty;

        public readonly static DependencyProperty NormalColorProperty;

        public readonly static DependencyProperty AlarmColorProperty;

        public readonly static DependencyProperty FatalColorProperty;


        public readonly static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(IDynamicBase),
            new PropertyMetadata(new PropertyChangedCallback(UpdateUIValueChangedCallback)));

        public readonly static DependencyProperty ValueFormatProperty = DependencyProperty.Register("ValueFormat", typeof(string), typeof(IDynamicBase),
         new PropertyMetadata(new PropertyChangedCallback(UpdateUIValueChangedCallback)));

        public readonly static DependencyProperty ShowScaleValueProperty = DependencyProperty.Register("ShowScaleValue", typeof(bool), typeof(IDynamicBase),
           new PropertyMetadata(true, new PropertyChangedCallback(UpdateUIChangedCallback)));


        static IDynamicBase()
        {
            MinAlarmProperty = DependencyProperty.Register("MinAlarm", typeof(string), typeof(IDynamicBase), new PropertyMetadata(string.Empty));
            MaxAlarmProperty = DependencyProperty.Register("MaxAlarm", typeof(string), typeof(IDynamicBase), new PropertyMetadata(string.Empty));
            MinFatalProperty = DependencyProperty.Register("MinFatal", typeof(string), typeof(IDynamicBase), new PropertyMetadata(string.Empty));
            MaxFatalProperty = DependencyProperty.Register("MaxFatal", typeof(string), typeof(IDynamicBase), new PropertyMetadata(string.Empty));
            NormalColorProperty = DependencyProperty.Register("NormalColor", typeof(Color), typeof(IDynamicBase), new PropertyMetadata(Colors.Aqua, new PropertyChangedCallback(ShowChanged)));
            AlarmColorProperty = DependencyProperty.Register("AlarmColor", typeof(Color), typeof(IDynamicBase), new PropertyMetadata(Colors.Red));
            FatalColorProperty = DependencyProperty.Register("FatalColor", typeof(Color), typeof(IDynamicBase), new PropertyMetadata(Colors.DarkRed));
        }

        private string[] idArray = new string[0];

        protected string[] valueArray = new string[0];

        private bool[] isAlarmArray = new bool[0];

        [Category("龙坤自定义属性")]
        [Description("报警颜色")]
        public Color AlarmColor
        {
            get
            {
                return (Color)base.GetValue(IDynamicBase.AlarmColorProperty);
            }
            set
            {
                base.SetValue(IDynamicBase.AlarmColorProperty, value);
            }
        }

        public string Command
        {
            get;
            set;
        }

        [Category("龙坤自定义属性")]
        [Description("数据参数数组(支持任意WCF接口)")]
        public string[] DataArguments
        {
            get;
            set;
        }

        public string Expression
        {
            get;
            set;
        }

        [Category("龙坤自定义属性")]
        [Description("严重报警颜色（已过时）")]
        public Color FatalColor
        {
            get
            {
                return (Color)base.GetValue(IDynamicBase.FatalColorProperty);
            }
            set
            {
                base.SetValue(IDynamicBase.FatalColorProperty, value);
            }
        }

        [Category("龙坤自定义属性")]
        [Description("整个控件是否报警")]
        public bool IsAlarm
        {
            get;
            set;
        }

        public string IsAlarmForDebug
        {
            get
            {
                string str;
                lock (this.idArray)
                {
                    if (this.isAlarmArray == null || this.isAlarmArray.Length == 0)
                    {
                        str = null;
                    }
                    else if ((int)this.isAlarmArray.Length != 1)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        for (int i = 0; i < (int)this.isAlarmArray.Length; i++)
                        {
                            stringBuilder.Append(string.Format("ID_{0}_IsAlarm:{1};", i, this.isAlarmArray[i]));
                        }
                        str = stringBuilder.ToString();
                    }
                    else
                    {
                        str = this.isAlarmArray[0].ToString();
                    }
                }
                return str;
            }
        }

        [Category("龙坤自定义属性")]
        [Description("报警上限（已过时）")]
        public string MaxAlarm
        {
            get
            {
                return (string)base.GetValue(IDynamicBase.MaxAlarmProperty);
            }
            set
            {
                base.SetValue(IDynamicBase.MaxAlarmProperty, value);
            }
        }

        [Category("龙坤自定义属性")]
        [Description("严重报警上限（已过时）")]
        public string MaxFatal
        {
            get
            {
                return (string)base.GetValue(IDynamicBase.MaxFatalProperty);
            }
            set
            {
                base.SetValue(IDynamicBase.MaxFatalProperty, value);
            }
        }



        [Category("龙坤自定义属性")]
        [Description("报警下限（已过时）")]
        public string MinAlarm
        {
            get
            {
                return (string)base.GetValue(IDynamicBase.MinAlarmProperty);
            }
            set
            {
                base.SetValue(IDynamicBase.MinAlarmProperty, value);
            }
        }

        [Category("龙坤自定义属性")]
        [Description("严重报警下限（已过时）")]
        public string MinFatal
        {
            get
            {
                return (string)base.GetValue(IDynamicBase.MinFatalProperty);
            }
            set
            {
                base.SetValue(IDynamicBase.MinFatalProperty, value);
            }
        }

        [Category("龙坤自定义属性")]
        [Description("正常颜色")]
        public Color NormalColor
        {
            get
            {
                return (Color)base.GetValue(IDynamicBase.NormalColorProperty);
            }
            set
            {
                base.SetValue(IDynamicBase.NormalColorProperty, value);
            }
        }

        [Category("龙坤自定义属性")]
        [Description("值")]
        public double Value
        {
            get
            {
                return (double)base.GetValue(ValueProperty);
            }
            set
            {
                base.SetValue(ValueProperty, value);
            }
        }
        [Category("龙坤自定义属性")]
        [Description("值的格式")]
        public string ValueFormat
        {
            get
            {
                return (string)base.GetValue(ValueFormatProperty);
            }
            set
            {
                base.SetValue(ValueFormatProperty, value);
            }
        }

        [Category("龙坤自定义属性")]
        [Description("是否显示刻度对应数值")]
        public bool ShowScaleValue
        {
            get
            {
                return (bool)base.GetValue(ShowScaleValueProperty);
            }
            set
            {
                base.SetValue(ShowScaleValueProperty, value);
            }
        }
        public string ValueForDebug
        {
            get
            {
                string str;
                lock (this.idArray)
                {
                    if (this.valueArray == null || this.valueArray.Length == 0)
                    {
                        str = null;
                    }
                    else if ((int)this.valueArray.Length != 1)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        for (int i = 0; i < (int)this.valueArray.Length; i++)
                        {
                            stringBuilder.Append(string.Format("ID_{0}_Value:{1};", i, this.valueArray[i]));
                        }
                        str = stringBuilder.ToString();
                    }
                    else
                    {
                        str = this.valueArray[0];
                    }
                }
                return str;
            }
        }



        protected IDynamicBase()
        {
        }

        public bool GetIdIsAlarm()
        {
            return this.GetIdIsAlarm(0);
        }

        public bool GetIdIsAlarm(int idIndex)
        {
            bool flag;
            lock (this.idArray)
            {
                flag = ((int)this.isAlarmArray.Length <= idIndex ? false : this.isAlarmArray[idIndex]);
            }
            return flag;
        }

        public string GetIdValue()
        {
            return this.GetIdValue(0);
        }

        public string GetIdValue(int idIndex)
        {
            string str;
            lock (this.idArray)
            {
                if ((int)this.valueArray.Length <= idIndex)
                {
                    str = null;
                }
                else
                {
                    str = this.valueArray[idIndex];
                }
            }
            return str;
        }

        private static void UpdateUIChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IDynamicBase)d).UpdateUI();
        }

        private static void UpdateUIValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IDynamicBase)d).UpdateUIValue();
        }

        private static void ShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IDynamicBase)d).UpdateUIValue();
        }

        /// <summary>
        /// 更新界面
        /// </summary>
        public abstract void UpdateUIValue();

        /// <summary>
        /// 更新界面
        /// </summary>
        public abstract void UpdateUI();

    }


}