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

namespace WznGwent
{
    /// <summary>
    /// EntranceWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EntranceWindow : Window
    {
        public EntranceWindow()
        {
            InitializeComponent();
        }

        private void configBtn_click(object sender, RoutedEventArgs e)
        {
            ConfigWindow cfgWin = new ConfigWindow();
            cfgWin.Show();
            this.Close();
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            PlayWindow plyWin = new PlayWindow();
            plyWin.Show();
            this.Close();
        }
    }

    //将输入double除以parameter再输出的转换器
    public class divideDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) 
        {
            return System.Convert.ToDouble(value)/ System.Convert.ToDouble(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }  
}
