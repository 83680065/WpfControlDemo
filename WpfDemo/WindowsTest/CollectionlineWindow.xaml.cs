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
    /// CollectionlineWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CollectionlineWindow : Window
    {
        public CollectionlineWindow()
        {
            InitializeComponent();
            var data = DemoData.CreateColumnHeaderTankData();
            int otherCount = new Random().Next(100) % 10;

            otherCount = 6;

            var viemModel = new ViewModeTest();
            viemModel.ParamValues = data;
            viemModel.OtherCount = 2; 
           
            ATimeline2.DataContext = viemModel; 
            ATimeline3.DataContext = viemModel;
        }
    }
}
