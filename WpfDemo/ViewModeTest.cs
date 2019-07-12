using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDemo
{
    public class ViewModeTest
    {
        public int OtherCount { get; set; }

        private ObservableCollection<ComParamValue> _paramValues;
        public ObservableCollection<ComParamValue> ParamValues
        {
            get { return _paramValues; }
            set { _paramValues = value; }
        }
    }
}
