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
    /// ReportWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
        }
        private List<MapSpotModel> MSList = new List<MapSpotModel>();
        public void SelectMS()
        {
            #region 构造行政区数据
            string[] str1 = { "XX县", "ZZ县", "CC县", "VV县" };
            string[] str2 = { "QQQ", "WWW", "AAA", "SSS", "EEE", "DDD", "RRR" };
            #endregion 构造行政区数据_end
            //随机数
            Random rd0 = new Random();
            for (int i = 0; i < str1.Count(); i++)
            {
                int cd = 101 + i;

                #region 构造图斑数据
                for (int j = 0; j < str2.Count(); j++)
                {
                    #region 生成随机图斑及面积个数并计算其值

                    #region 所有图斑个数
                    int AllNumber1 = rd0.Next(1, 3000);
                    #endregion 所有图斑个数_end

                    #region 核查单位已核查图斑个数
                    //总共
                    int HCTureNumber1 = rd0.Next(1, AllNumber1);
                    //已核查通过个数
                    int HCTruePassNumber1 = rd0.Next(1, HCTureNumber1);
                    //已核查不通过个数
                    int HCTrueNoPassNumber1 = HCTureNumber1 - HCTruePassNumber1;
                    //通过率
                    string HCTruePassingRate1 = (Math.Round(((double)HCTruePassNumber1 / HCTureNumber1) * 100)) + "%";
                    #endregion 核查单位已核查图斑个数_end

                    #region 核查单位已核查图斑个数
                    //总共
                    int JLTureNumber1 = rd0.Next(1, AllNumber1);
                    //已核查通过个数
                    int JLTruePassNumber1 = rd0.Next(1, JLTureNumber1);
                    //已核查不通过个数
                    int JLTrueNoPassNumber = JLTureNumber1 - JLTruePassNumber1;
                    //通过率
                    string JLTruePassingRate1 = (Math.Round(((double)JLTruePassNumber1 / JLTureNumber1) * 100)) + "%";
                    #endregion 核查单位已核查图斑个数_end

                    #region 所有图斑面积
                    double AllArea1 = Math.Round((rd0.NextDouble() * 1000), 2);
                    double HCTrueArea1 = rd0.Next(1, (int)AllArea1);
                    double JLTrueArea1 = rd0.Next(1, (int)AllArea1);
                    #endregion

                    #endregion 


                    MSList.Add(new MapSpotModel()
                    {
                        Id = i + 1,
                        Name = str2[j],
                        Code = cd,
                        AllNumber = AllNumber1,
                        AllArea = AllArea1,
                        HCTureNumber = HCTureNumber1,
                        HCTrueArea = HCTrueArea1,
                        HCTruePassNumber = HCTruePassNumber1,
                        HCTrueNoPassNumber = HCTrueNoPassNumber1,
                        HCTruePassingRate = HCTruePassingRate1,
                        JLTureNumber = JLTureNumber1,
                        JLTrueArea = JLTrueArea1,
                        JLTruePassNumber = JLTruePassNumber1,
                        JLTrueNoPassNumber = JLTrueNoPassNumber,
                        JLTruePassingRate = JLTruePassingRate1,
                        DistrictModels = new DistrictModel() { Id = i, Code = cd, Name = str2[j] }
                    });
                }
                #endregion 构造图斑数据_end
            }

            dategrid2.ItemsSource = MSList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SelectMS();
        }
    }
}
