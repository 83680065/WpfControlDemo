using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfDemo
{
    /// <summary>
    /// ComboBoxDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ComboBoxDemo : Window
    {
        public ComboBoxDemo()
        {
            InitializeComponent();
            var dataContent = new AlarmInfo();
            var listWarmColors = ColorInfo.GetColorData();
            dataContent.SelectedColor = listWarmColors[2];

            colorComboBox.DataContext = dataContent;
            colorComboBox2.DataContext = dataContent;
            colorComboBox3.DataContext = dataContent;
        }
    }

    public class PropertyChangedBase
    {
        public void NotifyOfPropertyChange()
        {

        }
    }
    public class ColorInfo : PropertyChangedBase
    {
        private static List<ColorInfo> colorData;
        public static List<ColorInfo> GetColorData()
        {
            if (colorData == null)
            {
                colorData = new List<ColorInfo>() {
                new ColorInfo { Name = "红色", Color = "Red" },
                new ColorInfo { Name = "自定义", Color = "#123321" },
                new ColorInfo { Name = "绿色", Color = "Green" },
                new ColorInfo { Name = "蓝色", Color = "blue" },
                new ColorInfo {Name="紫色",Color=Colors.Violet.ToString()}
                };

            }
            return colorData;
        }

        private string color;
        /// <summary>
        /// 显示颜色
        /// </summary>
        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                NotifyOfPropertyChange();
            }
        }

        private string name;
        /// <summary>
        /// 显示颜色
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyOfPropertyChange();
            }
        }
    }
    public class AlarmInfo : PropertyChangedBase
    {
        public AlarmInfo()
        {
            AllColors = ColorInfo.GetColorData();
        }

        private List<ColorInfo> allColors;
        public List<ColorInfo> AllColors
        {
            get { return allColors; }
            set
            {
                allColors = value;
                NotifyOfPropertyChange();
            }
        }


        private ColorInfo color;
        /// <summary>
        /// 显示颜色
        /// </summary>
        public ColorInfo SelectedColor
        {
            get { return color; }
            set
            {
                color = value;
                NotifyOfPropertyChange();
            }
        }

    }
}
