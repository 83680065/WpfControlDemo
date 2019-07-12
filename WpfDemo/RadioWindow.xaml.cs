using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// RadioWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RadioWindow : Window
    {

        private DataModel m_dataModel;
        public RadioWindow()
        {
            InitializeComponent();
            m_dataModel = new DataModel();
            DataContext = m_dataModel;
        }

        private void btnGetRoBValue_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(m_dataModel.SampleEnum.ToString());
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menu = sender as MenuItem;
            if (menu.IsChecked)
            {
                return;
            }
            else
            {
                menu.IsChecked = true;
            }
        }
    }


    public class DataModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string p_propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p_propertyName));
            }
        }
        private string testText = "A";

        private EmunMyString _sampleEnum;

        public EmunMyString SampleEnum
        {
            get
            {
                return _sampleEnum;
            }

            set
            {
                _sampleEnum = value;
                OnPropertyChanged("SampleEnum");
            }
        }

        public string TestText
        {
            get
            {
                return testText;
            }

            set
            {
                testText = value;
                OnPropertyChanged("TestText");
            }
        }
    }

    public enum EmunMyString
    {
        A,
        B,
        C

    }

    public class EnumToBooleanConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? false : value.Equals(parameter);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value == null ? Binding.DoNothing : value.Equals(true) ? parameter : Binding.DoNothing;
            return result;

            return value != null && value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }

}
