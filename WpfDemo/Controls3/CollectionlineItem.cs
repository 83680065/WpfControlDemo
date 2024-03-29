﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace MultiColorWin.Controls.Timeline
{
    public class CollectionlineItem : ContentControl
    {
        #region DependencyProperty

        #region IsFirstItem
        /// <summary>
        /// 获取或者设置该项在列表中是否是第一个
        /// </summary>
        [Bindable(true), Description("获取或者设置该项在列表中是否是第一个")]
        public bool IsFirstItem
        {
            get { return (bool)GetValue(IsFirstItemProperty); }
            set { SetValue(IsFirstItemProperty, value); }
        }

        public static readonly DependencyProperty IsFirstItemProperty =
            DependencyProperty.Register("IsFirstItem", typeof(bool), typeof(CollectionlineItem), new PropertyMetadata(false));

        #endregion

        #region IsMiddleItem

        /// <summary>
        /// 获取或者设置该项在列表中是否是中间的一个
        /// </summary>
        [Bindable(true), Description("获取或者设置该项在列表中是否是中间的一个")]
        public bool IsMiddleItem
        {
            get { return (bool)GetValue(IsMiddleItemProperty); }
            set { SetValue(IsMiddleItemProperty, value); }
        }

        public static readonly DependencyProperty IsMiddleItemProperty =
            DependencyProperty.Register("IsMiddleItem", typeof(bool), typeof(CollectionlineItem), new PropertyMetadata(false));

        #endregion

        /// <summary>
        /// 获取或者设置该项在列表中是否是中间的一个
        /// </summary>
        [Bindable(true), Description("是否显示标题")]
        public bool IsTitleItem
        {
            get { return (bool)GetValue(IsTitleItemProperty); }
            set { SetValue(IsTitleItemProperty, value); }
        }

        public static readonly DependencyProperty IsTitleItemProperty =
            DependencyProperty.Register("IsTitleItem", typeof(bool), typeof(CollectionlineItem), new PropertyMetadata(false));

        #region IsLastItem
        /// <summary>
        /// 获取或者设置该项在列表中是否是最后一个
        /// </summary>
        [Bindable(true), Description("获取或者设置该项在列表中是否是最后一个")]
        public bool IsLastItem
        {
            get { return (bool)GetValue(IsLastItemProperty); }
            set { SetValue(IsLastItemProperty, value); }
        }

        public static readonly DependencyProperty IsLastItemProperty =
            DependencyProperty.Register("IsLastItem", typeof(bool), typeof(CollectionlineItem), new PropertyMetadata(false));

        #endregion

        #endregion 
        static CollectionlineItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CollectionlineItem), new FrameworkPropertyMetadata(typeof(CollectionlineItem)));
        } 
    }
}
