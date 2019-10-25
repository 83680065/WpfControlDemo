using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WpfDemo.Converters
{

    /// <summary>
    /// 矩形转换器
    /// </summary>
    public class RectConverter : IValueConverter
    {


        public static Rect From { get; } = new Rect(0, 0, 800, 0);

        public static Rect To { get; } = new Rect(0, 0, 800, 1200);




        public static RectConverter Ins { get; } = new RectConverter();

        //当值从绑定源传播给绑定目标时，调用方法Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = Rect.Empty;

            if (value == null || parameter == null)
            {
                return result;
            }

            var control = value as FrameworkElement;
            if (control != null)
            {
                if (parameter.ToString() == "1")//最大
                {
                    result = new Rect(0, 0, control.Width, control.Height);
                }
                else
                {
                    result = new Rect(0, 0, control.Width, 0);
                }
            }
            return result;
        }
        //当值从绑定目标传播给绑定源时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return null;
        }
    }

}
