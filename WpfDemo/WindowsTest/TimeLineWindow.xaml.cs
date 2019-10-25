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

using System.Collections.ObjectModel;

namespace WpfDemo
{
    /// <summary>
    /// TimeLineWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TimeLineWindow : Window
    {
        public TimeLineWindow()
        {
            InitializeComponent();
            this.DataContext = new SigninViewModel();
        }
    }

    public class SigninViewModel
    {
        public SigninViewModel()
        {
            SigninItems = new ObservableCollection<SigninInfo> {
                new SigninInfo {Title="王多鱼在12:12:35签到" },
                new SigninInfo { Title = "孙大圣在12:12:35签到" },
                new SigninInfo {Title="亮亮在12:12:35替班签到" }};
        }
        private ObservableCollection<SigninInfo> signinItems;
        public ObservableCollection<SigninInfo> SigninItems { get { return signinItems; } set { signinItems = value; } }
    }

    public class SigninInfo
    {
        private string title;

        public string Title { get { return title; } set { title = value; } }
    }
}
