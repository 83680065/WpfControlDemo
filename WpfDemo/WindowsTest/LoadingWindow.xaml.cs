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
    /// LoadingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }
        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            this._loading.Visibility = Visibility.Visible;
        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            this._loading.Visibility = Visibility.Collapsed;
        }
    }
}
