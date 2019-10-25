using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using WpfDemo.Helpers;

namespace WpfDemo
{
    /// <summary>
    /// ReportWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ReportWindow : Window
    {
        ReportViewModel viewModel = new ReportViewModel();
        public ReportWindow()
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void btnExp(object sender, RoutedEventArgs e)
        {
            viewModel.Export();
        }
    }


    public class ReportViewModel
    {
        private Random rand = new Random();
        private int dataCount = 10;
        private string dateFormat = "MM月dd号";
        public ReportViewModel()
        {
            Columns = new List<ColumnInfo>();
            Data = new List<ValueInfo>();

            DateTime beginDate = new DateTime(2019, 8, 1);
            DateTime endDate = new DateTime(2019, 8, 28);

            QueryData(beginDate, endDate);
        }

        public void Export()
        {
            var excelFile = ExcelExportHelper.Export(this);
            if (File.Exists(excelFile))
            {
                Process.Start(excelFile);
            }
        }

        private void QueryData(DateTime beginDate, DateTime endDate)
        {
            int columnCount = (endDate - beginDate).Days + 1;
            ColumnsTitle = beginDate.ToString(dateFormat) + "-" + endDate.ToString(dateFormat);

            for (int i = 0; i < columnCount; i++)
            {
                Columns.Add(new ColumnInfo { DateInfo = beginDate.AddDays(i).ToString(dateFormat) });
            }

            for (int i = 1; i <= dataCount; i++)
            {
                var dataValue = new List<ColumnInfo>();

                #region 每一行数据
                int start = rand.Next(5000, 7000);
                for (int j = 0; j < columnCount; j++)
                {
                    var today = rand.Next(100, 200);
                    var total = start;
                    if (j > 1)
                    {
                        total = int.Parse(dataValue[j - 1].DateInfo) + today;
                    }
                    dataValue.Add(new ColumnInfo
                    {
                        DateInfo = total.ToString(),
                        ValueInfo = today.ToString()
                    });
                }
                #endregion

                Data.Add(new ValueInfo { Name = "设备" + i, DataValue = dataValue });
            }
        }

        /// <summary>
        /// 列的信息
        /// </summary>
        public List<ColumnInfo> Columns { get; set; }
        /// <summary>
        /// 表格数据
        /// </summary>
        public List<ValueInfo> Data { get; set; }
        /// <summary>
        /// 合并列标题
        /// </summary>
        public string ColumnsTitle { get; set; }
    }


    public class ColumnInfo
    {
        public string DateInfo { get; set; }
        public string ValueInfo { get; set; } = "当天耗电量";
    }


    public class ValueInfo
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 列对应值
        /// </summary>
        public List<ColumnInfo> DataValue { get; set; }
    }
}
