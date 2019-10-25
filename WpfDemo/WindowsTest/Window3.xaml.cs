using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();

            var data = DemoData.CreateColumnHeaderTankData();
            int otherCount = new Random().Next(100) % 10;
            otherCount = 0;
            var viemModel = new ViewModeTest();
            viemModel.ParamValues = data;
            viemModel.OtherCount = otherCount;
            this.DataContext = viemModel;

            ATimeline1.DataContext = viemModel;

            var viemModel2 = new ViewModeTest();
            viemModel2.ParamValues = data;
            viemModel2.OtherCount = 1;

            ATimeline2.DataContext = viemModel2;


            var viemModel3 = new ViewModeTest();
            viemModel3.ParamValues = data;
            viemModel3.OtherCount = 10;
            ATimeline3.DataContext = viemModel3;            
        }
    }   
}
