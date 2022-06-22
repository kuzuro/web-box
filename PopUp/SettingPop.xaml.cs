using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace web_box.PopUp
{
    /// <summary>
    /// SettingPop.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingPop : Window
    {
        public SettingPop()
        {
            InitializeComponent();

            url.Text = Properties.Settings.Default.URL;
        }


        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.URL = url.Text.Trim();
            Properties.Settings.Default.Save();

            Close();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("캐시 삭제 후 프로그램이 재시작됩니다.", "캐시 삭제", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if(result == MessageBoxResult.No)
            {
                return;
            }
                        
            string path = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"\Cache\");

            DirectoryInfo di = new System.IO.DirectoryInfo(path);

            foreach(var item in di.GetDirectories())
            {
                try
                {
                    item.Delete();
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message.ToString());
                }
            }

            try
            {
                Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            

            ((MainWindow)Application.Current.MainWindow).reStartFlag = true;
            
            Close();
        }

    }
}
