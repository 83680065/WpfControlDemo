using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;

namespace WpfDemo.Helpers
{
    public class UserControlHelper
    {
        #region 字段和属性
        private UserControl m_UserControl;
        private int mainThreadTimeout = 100;
        public UserControl UserControl
        {
            get
            {
                return this.m_UserControl;
            }
        }
        /// <summary>
        /// 模板目录
        /// </summary>
        public static string TemplatePath { get; } = AppDomain.CurrentDomain.BaseDirectory + "Config\\Template";

        #endregion

        #region 构造函数
        public UserControlHelper(string uri)
        {
            try
            {
                this.m_UserControl = this.LoadControlFromXaml(uri);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                this.m_UserControl = this.GetErrorUserControl(exception);
            }
        }
        public UserControlHelper(string uri, string path)
        {
            try
            {
                this.m_UserControl = this.LoadControlFromXaml(uri, path);

            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                this.m_UserControl = this.GetErrorUserControl(exception);
            }
        }
        #endregion 

        #region 方法
        private UserControl GetErrorUserControl(Exception e)
        {
            UserControl userControl = new UserControl();
            TextBox textBox = new TextBox()
            {
                Foreground = Brushes.Red,
                IsReadOnly = true,
                Text = string.Concat("页面解析出错，错误信息：", e.ToString())
            };
            userControl.Content = textBox;
            userControl.MinHeight = 500;
            userControl.MinWidth = 700;
            return userControl;
        }

        /// <summary>
        /// 加载xaml路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private UserControl LoadControlFromXaml(string path)
        {
            return this.LoadControlFromXaml(path, TemplatePath);
        }

        /// <summary>
        /// 加载xaml路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="uiFolderPath"></param>
        /// <returns></returns>
        private UserControl LoadControlFromXaml(string path, string uiFolderPath)
        {
            UserControl errorUserControl;
            if (path == null)
            {
                return this.GetErrorUserControl(new Exception("配置的页面文件路径为null。"));
            }
            if (!path.ToLower().EndsWith(".xaml"))
            {
                return this.GetErrorUserControl(new Exception(string.Format("配置的页面文件[{0}]不是xaml文件。", path)));
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                try
                {
                    string str = Path.Combine(uiFolderPath, path);
                    str = str.Replace('/', Path.DirectorySeparatorChar);
                    if (!File.Exists(str))
                    {
                        errorUserControl = this.GetErrorUserControl(new Exception(string.Format("在UI目录[{0}]中页面[{1}]不存在", uiFolderPath, path)));
                    }
                    else
                    {
                        string end = (new StreamReader((new StreamReader(str, Encoding.UTF8)).BaseStream, Encoding.GetEncoding("UTF-8"))).ReadToEnd();
                        Uri uri = new Uri(uiFolderPath);
                        end = end.Replace("pack://siteoforigin:,,,", uri.AbsoluteUri);

                        var obj = XamlReader.Load(new MemoryStream(Encoding.GetEncoding("UTF-8").GetBytes(end)));
                        var userControl = (UserControl)obj;
                        RoutedEventHandler routedEventHandler = null;
                        routedEventHandler = (sender, e) =>
                        {
                            userControl.Loaded -= routedEventHandler;
                        };
                        userControl.Loaded += routedEventHandler;
                        errorUserControl = userControl;
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            finally
            {
                stopwatch.Stop();
                if (stopwatch.Elapsed.TotalSeconds > 10 && this.mainThreadTimeout > 0)
                {
                    TimeSpan elapsed = stopwatch.Elapsed;
                    // string str1 = string.Format("警告：加载XAML文件[{0}]用时[{1}]，大于10秒钟！", path, elapsed.ToString());
                }
            }
            return errorUserControl;
        }


        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="current"></param>
        /// <param name="handler"></param>
        public static void GetLink(FrameworkElement current, RoutedEventHandler handler)
        {
            RoutedEventHandler routedEventHandler;
            dynamic obj;
            foreach (object child in LogicalTreeHelper.GetChildren(current))
            {
                if (child.GetType().Name == "DButtonLed" || child.GetType().Name == "DButtonLedMgrObj")
                {
                    object obj2 = child;
                    routedEventHandler = handler;
                    obj = obj2;
                    obj.Click -= routedEventHandler;
                    routedEventHandler = handler;
                    obj = obj2;
                    obj.Click += routedEventHandler;
                }
                else if (!(child is Button))
                {
                    if (!(child is FrameworkElement))
                    {
                        continue;
                    }
                    GetLink((FrameworkElement)child, handler);
                }
                else
                {
                    Button button = child as Button;
                    if (button.Tag == null)
                    {
                        continue;
                    }
                    button.Click -= handler;
                    button.Click += handler;
                }
            }
        }
        #endregion       
    }
}