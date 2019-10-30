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
    /// PrintfDemoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PrintfDemoWindow : Window
    {
        public PrintfDemoWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(printArea, "Print Test");
            }
        }

        private void PrintfView_Click(object sender, RoutedEventArgs e)
        { 
            new PrintfViewWindow(printArea).ShowDialog();
        }
       
    }
}
