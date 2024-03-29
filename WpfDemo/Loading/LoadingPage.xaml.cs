﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDemo.Loading
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingPage : UserControl
    {
        public LoadingPage()
        {
            InitializeComponent();
        }

        #region 加载中的提示的字体颜色
        [DescriptionAttribute("加载中的提示的字体颜色"), CategoryAttribute("扩展"), DefaultValueAttribute(0)]
        public Brush LoadingTextForeground
        {
            get { return (Brush)GetValue(LoadingTextForegroundProperty); }
            set { SetValue(LoadingTextForegroundProperty, value); }
        }


        public static readonly DependencyProperty LoadingTextForegroundProperty =
            DependencyProperty.Register("LoadingTextForeground", typeof(Brush), typeof(LoadingPage),
            new FrameworkPropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE3953D"))));
        #endregion
        #region 加载中的提示
        [DescriptionAttribute("加载中的提示"), CategoryAttribute("扩展"), DefaultValueAttribute(0)]
        public string LoadingText
        {
            get { return (string)GetValue(LoadingTextProperty); }
            set { SetValue(LoadingTextProperty, value); }
        }


        public static readonly DependencyProperty LoadingTextProperty =
            DependencyProperty.Register("LoadingText", typeof(string), typeof(LoadingPage),
            new FrameworkPropertyMetadata("加载中.."));
        #endregion

        #region 加载中的提示的字体大小
        [DescriptionAttribute("加载中的提示的字体大小"), CategoryAttribute("扩展"), DefaultValueAttribute(0)]
        public int LoadingTextFontSize
        {
            get { return (int)GetValue(LoadingTextFontSizeProperty); }
            set { SetValue(LoadingTextFontSizeProperty, value); }
        }


        public static readonly DependencyProperty LoadingTextFontSizeProperty =
            DependencyProperty.Register("LoadingTextFontSize", typeof(int), typeof(LoadingPage),
            new FrameworkPropertyMetadata(10));
        #endregion

        #region 圆圈的颜色
        [DescriptionAttribute("圆圈的颜色"), CategoryAttribute("扩展"), DefaultValueAttribute(0)]
        public Brush CirclesBrush
        {
            get { return (Brush)GetValue(CirclesBrushProperty); }
            set { SetValue(CirclesBrushProperty, value); }
        }


        public static readonly DependencyProperty CirclesBrushProperty = DependencyProperty.Register("CirclesBrush", typeof(Brush), typeof(LoadingPage),
            new FrameworkPropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF007BE5"))));
        #endregion
    }
}
