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
    /// WpfAppContainerTest.xaml 的交互逻辑
    /// </summary>
    public partial class WpfAppContainerTest : Window
    {
        public WpfAppContainerTest()
        {
            InitializeComponent();
            this.Loaded += WpfAppContainerTest_Loaded;
        }

        private void WpfAppContainerTest_Loaded(object sender, RoutedEventArgs e)
        {
            ctnTest.StartAndEmbedProcess(@"C:\Windows\system32\mspaint.exe");
        }
    }
}
