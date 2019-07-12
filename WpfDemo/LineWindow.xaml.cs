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
using WpfDemo.Controls2;

namespace WpfDemo
{
    /// <summary>
    /// Window4.xaml 的交互逻辑
    /// </summary>
    public partial class LineWindow : Window
    {

        /// <summary>
        /// 定时器
        /// </summary>
        private System.Windows.Threading.DispatcherTimer m_Timer1 = new System.Windows.Threading.DispatcherTimer();

        double m_Percent = 0;
        bool m_IsStart = false;

        List<CycleProcessBar> allControls = new List<CycleProcessBar>();
        public LineWindow()
        {
            InitializeComponent();
            m_Timer1.Interval = new TimeSpan(0, 0, 0, 0, 100);
            m_Timer1.Tick += M_Timer1_Tick;
            CreateControl();
        }

        private void CreateControl()
        {
            circleProgressBar.Visibility = Visibility.Collapsed;
            for (int i = 0; i < 8; i++)
            {
                var temp = new CycleProcessBar();
                temp.DrawPosition = (DrawPosition)(i % 4);
                allControls.Add(temp);
                var num = new Random().Next(10);
                temp.ShowDash = num % 3 == 0;
                CycleProcessBarMian.Children.Add(temp);
                if (i > 3)
                {
                    temp.BackgroundBarColor = new SolidColorBrush(Colors.Green);
                    temp.Clockwise = false;
                }
            }
            for (int i = 0; i < 8; i++)
            {
                var temp = new CycleProcessBar();
                temp.DrawPosition = (DrawPosition)(i % 4);
                temp.ShowBackGroundArc = false;

                allControls.Add(temp);
                CycleProcessBarMian1.Children.Add(temp);
                if (i > 3)
                {
                    temp.Clockwise = false;
                }
            }
        }

        int count = 0;
        private void M_Timer1_Tick(object sender, EventArgs e)
        {
            m_Percent += 0.01;
            if (m_Percent > 1)
            {
                m_Percent = 1;
                m_Timer1.Stop();
                m_IsStart = false;
                StartChange(m_IsStart);
                //if (count < 4)
                //{
                //    count++;
                //    circleProgressBar.DrawPosition = (DrawPosition)count;

                //    m_Percent = 0;
                //    Button_Click(null, null);
                //}
            }
            if (circleProgressBar.Visibility == Visibility.Visible)
            {
                circleProgressBar.CurrentValue = m_Percent;
            }
            else
            {
                foreach (var item in allControls)
                {
                    item.CurrentValue = m_Percent;
                }
            }


        }

        /// <summary>
        /// UI变化
        /// </summary>
        /// <param name="bState"></param>
        private void StartChange(bool bState)
        {
            if (bState)
                btnStart.Content = "停止";
            else
                btnStart.Content = "开始";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (m_IsStart)
            {
                m_Timer1.Stop();
                m_IsStart = false;
            }
            else
            {
                m_Percent = 0;
                m_Timer1.Start();
                m_IsStart = true;

            }
            StartChange(m_IsStart);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_Timer1.Stop();
        }
    }
}
