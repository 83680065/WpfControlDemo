using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace MultiColorWin.Controls.Timeline
{

    public class Collectionline : ItemsControl
    {

        #region DependencyProperty

        #region FirstLineTemplate

        /// <summary>
        /// 获取或者设置第一个的样子
        /// </summary>
        [Bindable(true), Description("获取或者设置第一个Line的样子")]
        public DataTemplate FirstLineTemplate
        {
            get { return (DataTemplate)GetValue(FirstLineTemplateProperty); }
            set { SetValue(FirstLineTemplateProperty, value); }
        }

        public static readonly DependencyProperty FirstLineTemplateProperty =
            DependencyProperty.Register("FirstLineTemplate", typeof(DataTemplate), typeof(Collectionline));

        #endregion

        /// <summary>
        /// 获取或者设置中间的的样子
        /// </summary>
        [Bindable(true), Description("获取或者设置标题模板")]
        public DataTemplate TitleLineTemplate
        {
            get { return (DataTemplate)GetValue(TitleLineTemplateProperty); }
            set { SetValue(TitleLineTemplateProperty, value); }
        }

        public static readonly DependencyProperty TitleLineTemplateProperty =
            DependencyProperty.Register("TitleLineTemplate", typeof(DataTemplate), typeof(Collectionline));


        #region MiddleLineTemplate

        /// <summary>
        /// 获取或者设置中间的的样子
        /// </summary>
        [Bindable(true), Description("获取或者设置中间的的样子")]
        public DataTemplate MiddleLineTemplate
        {
            get { return (DataTemplate)GetValue(MiddleLineTemplateProperty); }
            set { SetValue(MiddleLineTemplateProperty, value); }
        }

        public static readonly DependencyProperty MiddleLineTemplateProperty =
            DependencyProperty.Register("MiddleLineTemplate", typeof(DataTemplate), typeof(Collectionline));

        #endregion

        #region LastItemTemplate

        /// <summary>
        /// 获取或者设置最后一个的样子
        /// </summary>
        [Bindable(true), Description("获取或者设置最后一个模板")]
        public DataTemplate LastLineTemplate
        {
            get { return (DataTemplate)GetValue(LastLineTemplateProperty); }
            set { SetValue(LastLineTemplateProperty, value); }
        }

        public static readonly DependencyProperty LastLineTemplateProperty =
            DependencyProperty.Register("LastLineTemplate", typeof(DataTemplate), typeof(Collectionline));

        #endregion


        #region IsCustomEveryLine

        /// <summary>
        /// 获取或者设置是否自定义每一个的外观。
        /// </summary>
        [Bindable(true), Description("获取或者设置是否自定义每一个Line的外观。当属性值为True时，FirstLineTemplate、MiddleLineTemplate、LastLineTemplate属性都将失效，只能设置LineTemplate来定义每一个的样式")]
        public bool IsCustomEveryLine
        {
            get { return (bool)GetValue(IsCustomEveryLineProperty); }
            set { SetValue(IsCustomEveryLineProperty, value); }
        }

        public static readonly DependencyProperty IsCustomEveryLineProperty =
            DependencyProperty.Register("IsCustomEveryLine", typeof(bool), typeof(Collectionline), new PropertyMetadata(false));

        #endregion


        #region LineTemplate

        /// <summary>
        /// 获取或者设置每个的外观
        /// </summary>
        [Bindable(true), Description("获取或者设置每个的外观。只有当IsCustomEveryLine属性为True时，该属性才生效")]
        public DataTemplate LineTemplate
        {
            get { return (DataTemplate)GetValue(LineTemplateProperty); }
            set { SetValue(LineTemplateProperty, value); }
        }

        public static readonly DependencyProperty LineTemplateProperty =
            DependencyProperty.Register("LineTemplate", typeof(DataTemplate), typeof(Collectionline));

        #endregion 


        public static readonly DependencyProperty ItemLineHeightProperty =
          DependencyProperty.Register("ItemLineHeight", typeof(double), typeof(Collectionline), new PropertyMetadata(20.0));
        /// <summary>
        /// 画线的高度
        /// </summary>
        [Bindable(true), Description("画线的高度")]
        public double ItemLineHeight
        {
            get { return (double)GetValue(ItemLineHeightProperty); }
            set { SetValue(ItemLineHeightProperty, value); }
        }

        public static readonly DependencyProperty ItemLineWidthProperty =
         DependencyProperty.Register("ItemLineWidth", typeof(double), typeof(Collectionline), new PropertyMetadata(60.0));
        /// <summary>
        /// 画线的宽度
        /// </summary>
        [Bindable(true), Description("画线的宽度")]
        public double ItemLineWidth
        {
            get { return (double)GetValue(ItemLineWidthProperty); }
            set { SetValue(ItemLineWidthProperty, value); }
        }


        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Collectionline));
        /// <summary>
        /// 获取或者设置集合标题
        /// </summary>
        [Bindable(true), Description("获取或者设置集合标题")]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        #endregion

        static Collectionline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Collectionline), new FrameworkPropertyMetadata(typeof(Collectionline)));
        }

        protected virtual void ShowTitleItem(int currentIndex)
        {
            int titleIndex = Items.Count / 2;
            if (titleIndex == currentIndex)
            {
                var timelineItem = this.ItemContainerGenerator.ContainerFromIndex(currentIndex) as CollectionlineItem;
                if (timelineItem == null)
                {
                    return;
                }
                timelineItem.IsTitleItem = true;
            }
        }

        #region Override

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            int index = this.ItemContainerGenerator.IndexFromContainer(element);
            CollectionlineItem timelineItem = element as CollectionlineItem;
            if (timelineItem == null)
            {
                return;
            }
            if (index == 0)
            {
                timelineItem.IsFirstItem = true;
            }
            if (index == this.Items.Count - 1)
            {
                timelineItem.IsLastItem = true;
            }
            ShowTitleItem(index);


            base.PrepareContainerForItemOverride(timelineItem, item);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CollectionlineItem();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
       
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex == 0)
                    {
                        this.SetTimelineItem(e.NewStartingIndex + e.NewItems.Count);
                    }
                    //如果新添加项是放在最后一位，则更改原来的最后一位的属性值
                    if (e.NewStartingIndex == this.Items.Count - e.NewItems.Count)
                    {
                        this.SetTimelineItem(e.NewStartingIndex - 1);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex == 0)
                    {
                        this.SetTimelineItem(0);
                    }
                    else
                    {
                        this.SetTimelineItem(e.OldStartingIndex - 1);
                    }
                    break;
            }
        }
       
        #endregion


        /// <summary>
        /// 设置TimelineItem的位置属性
        /// </summary>
        /// <param name="index"></param>
        private void SetTimelineItem(int index)
        {
            if (index > this.Items.Count || index < 0)
            {
                return;
            }
            CollectionlineItem timelineItem = this.ItemContainerGenerator.ContainerFromIndex(index) as CollectionlineItem;
            if (timelineItem == null)
            {
                return;
            }
            ShowTitleItem(index);
            timelineItem.IsFirstItem = index == 0;
            timelineItem.IsLastItem = index == this.Items.Count - 1;
            timelineItem.IsMiddleItem = index > 0 && index < this.Items.Count - 1;
        }

        /// <summary>
        /// 画线位置，当前只实现 上和下
        /// </summary>
        public Dock LinePlacement
        {
            get { return (Dock)GetValue(LinePlacementProperty); }
            set { SetValue(LinePlacementProperty, value); }
        }

        public static readonly DependencyProperty LinePlacementProperty =
            DependencyProperty.Register("LinePlacement", typeof(Dock), typeof(Collectionline), new PropertyMetadata(Dock.Top));
    }
}
