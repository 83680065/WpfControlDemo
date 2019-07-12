using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDemo
{
    public class DemoData
    {
        public static ObservableCollection<ComParamValue> CreateColumnHeaderTankData()
        {
            var result = new ObservableCollection<ComParamValue>();

            var tablePhaseInfo1 = new ObservableCollection<PhaseInfo>()
                        {
                            new PhaseInfo() {  Name="电压" , PhaseA = "220V",   PhaseB="220V", PhaseC="220V" },
                            new PhaseInfo() {  Name="电流" , PhaseA = "10.2A",   PhaseB="10.2A", PhaseC="10.2A" },
                            new PhaseInfo() {  Name="功率" , PhaseA = "6.7KW",   PhaseB="6.7KW", PhaseC="6.7KW" }
                        };

            var tableElecInfo1 = new ObservableCollection<ElecInfo>();
            int max = 31;
            var rand = new Random(Guid.NewGuid().GetHashCode());
            int count = rand.Next(16, max);


            int type = rand.Next(max);
            for (int i = 0; i < count; i++)
            {
                var trmpElectricity = rand.NextDouble() * 10;
                var tempVoltage = rand.NextDouble() * 100;
                var tempInfo = new ElecInfo()
                {
                    Name = i.ToString(),
                    Type = i == type ? 1 : 0,
                    Electricity = trmpElectricity.ToString("0.00") + "A",
                    Voltage = tempVoltage.ToString("0") + "KW"
                };
                tableElecInfo1.Add(tempInfo);
            }
            //tableElecInfo1.Add(new ElecInfo());

            var tableElecInfo2 = new ObservableCollection<ElecInfo>();
            type = rand.Next(max);
            for (int i = 0; i < count; i++)
            {
                var trmpElectricity = rand.NextDouble() * 10;
                var tempVoltage = rand.NextDouble() * 100;
                var tempInfo = new ElecInfo()
                {
                    Name = i.ToString(),
                    Type = i == type ? 1 : 0,
                    Electricity = trmpElectricity.ToString("0.00") + "A",
                    Voltage = tempVoltage.ToString("0.00") + "KW"
                };
                tableElecInfo2.Add(tempInfo);
            }


            var tablePhaseInfo2 = new ObservableCollection<PhaseInfo>()
                        {
                            new PhaseInfo() {  Name="电压" , PhaseA = "220V",   PhaseB="220V", PhaseC="220V" },
                            new PhaseInfo() {  Name="电流" , PhaseA = "10.2A",   PhaseB="10.2A", PhaseC="10.2A" },
                            new PhaseInfo() {  Name="功率" , PhaseA = "6.7KW",   PhaseB="6.7KW", PhaseC="6.7KW" }
                        };
            result.Add(new ComParamValue() { ParamName = "A路表格", ParamValue = tablePhaseInfo1 });
            result.Add(new ComParamValue() { ParamName = "A路", ParamValue = tableElecInfo1 });
            result.Add(new ComParamValue() { ParamName = "B路", ParamValue = tableElecInfo2 });
            result.Add(new ComParamValue() { ParamName = "B路表格", ParamValue = tablePhaseInfo2 });
            return result;

        }
    }
    public class ComParamValue
    {
        private void NotifyOfPropertyChange()
        {

        }
        public string ParamName
        {
            get { return _paramName; }
            set { _paramName = value; NotifyOfPropertyChange(); }
        }
        private string _paramName;

        public object ParamValue
        {
            get { return _paramValue; }
            set { _paramValue = value; NotifyOfPropertyChange(); }
        }
        private object _paramValue;
    }
    /// <summary>
    /// 电路电压信息
    /// </summary>
    public class ElecInfo
    {
        /// <summary>
        /// 电压
        /// </summary>
        public string Voltage { get; set; }
        /// <summary>
        /// 电流
        /// </summary>
        public string Electricity { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// 是否报警
        /// </summary>
        public int Type { get; set; }
    }
    public class PhaseInfo
    {
        public string Name { get; set; }
        public string PhaseA { get; set; }
        public string PhaseB { get; set; }
        public string PhaseC { get; set; }
    }
}
