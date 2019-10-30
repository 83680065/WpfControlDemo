using LKsoft.Noah.Common.Helper;
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

namespace WpfDemo.Printf
{
    /// <summary>
    /// PrintfViewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PrintfViewWindow : Window
    {
        public PrintfViewWindow()
        {
            InitializeComponent();
        }

        public PrintfViewWindow(FrameworkElement element) : this()
        {
            //PrintfHelper.LoadDocument(view);

            PrintfHelper.LoadDocumentByControl(view, element);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintfHelper.SaveDocument(view);
        }
    }
}
