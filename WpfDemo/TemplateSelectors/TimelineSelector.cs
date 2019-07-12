/************************************************************************************* 
* 类 名 称：TimelineSelector 
* 文 件 名：TimelineSelector
* 创建时间：2019/2/9 16:08:22     
* 作  者：QQ81867376     
* 说   明：     
* 修改时间：     
* 修 改 人：
*************************************************************************************/
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MultiColorWin.TemplateSelectors
{
    public class TimelineSelector : DataTemplateSelector
    {
        /// <summary>
        ///特别模板
        /// </summary>
        public DataTemplate SpecialTemplate { get; set; }

        /// <summary>
        /// 默认模板
        /// </summary>
        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate result = DefaultTemplate;
            //if (item == null || item as VersionInfo == null)
            //{
            //    return result;
            //}
            //var isEdit = item as VersionInfo;
            //if (isEdit.Special)
            //{
            //    result = SpecialTemplate; 
            //}
            return result;
        }
    }
}
