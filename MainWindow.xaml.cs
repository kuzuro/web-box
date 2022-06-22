using CefSharp;
using CefSharp.Wpf;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using web_box.PopUp;

namespace web_box
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChromiumWebBrowser browser;
        private CefSettings settings;

        private bool titleFlag = false;
        public bool reStartFlag = false;


        public MainWindow()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BrowserSetting();

            browser = new ChromiumWebBrowser();
            Titlebar.Visibility = Visibility.Collapsed;

            BrowserLoad();
        }

        
        private void BrowserSetting()
        {
            settings = new CefSettings();
            settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 /CefSharp Browser" + Cef.CefSharpVersion;
            settings.CachePath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"\Cache");
            settings.CefCommandLineArgs.Add("persist_session_cookies", "1");
            Cef.Initialize(settings);
        }


        private void BrowserLoad()
        {
            if(!Cef.IsInitialized)
            {
                BrowserSetting();
            }

            browser.Address = Properties.Settings.Default.URL;
            browser.MenuHandler = new MenuHandler();

            BrowserControl.Content = browser;

        }



        private void Reload(object sender, EventArgs e)
        {
            if (reStartFlag)
            {
                reStartFlag = false;
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }

            if (!browser.Address.Equals(Properties.Settings.Default.URL))
            {
                BrowserLoad();
            }
        }





        #region window control


        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            Titlebar.Visibility = Visibility.Visible;
            titleFlag = true;

            browserPanel.Margin = new Thickness(0, 0, 0, 0);
        }


        private void Titlebar_MouseEnter(object sender, MouseEventArgs e)
        {
            if (titleFlag)
            {
                Titlebar.Visibility = Visibility.Visible;
            }
            else
            {
                Titlebar.Visibility = Visibility.Collapsed;
            }
        }


        private void Titlebar_MouseLeave(object sender, MouseEventArgs e)
        {
            titleFlag = false;
            Titlebar.Visibility = Visibility.Collapsed;
        }


        private void MainWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            if(titleFlag)
            {
                titleFlag = false;
                Titlebar.Visibility = Visibility.Collapsed;
            }
        }

        private void Btn_Setting_Click(object sender, RoutedEventArgs e)
        {
            SettingPop settingPop = new SettingPop();
            settingPop.Closed += Reload;
            settingPop.ShowDialog();
        }


        private void Btn_minimized_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            Cef.Shutdown();
            Environment.Exit(0);
        }


        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowResize_NORMAL();
                }
                else
                {
                    WindowResize_MAX();
                }
            }
            else
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }
            }
        }


        private void Btn_normal_Click(object sender, RoutedEventArgs e)
        {
            WindowResize_NORMAL();
        }


        private void Btn_maximized_Click(object sender, RoutedEventArgs e)
        {
            WindowResize_MAX();
        }


        private void WindowResize_NORMAL()
        {
            WindowState = WindowState.Normal;

            resizeControl.Visibility = Visibility.Visible;

            Btn_maximized.Visibility = Visibility.Visible;
            Btn_normal.Visibility = Visibility.Collapsed;

            this.BorderThickness = new Thickness(1);
        }


        private void WindowResize_MAX()
        {
            WindowState = WindowState.Maximized;

            resizeControl.Visibility = Visibility.Collapsed;

            Btn_maximized.Visibility = Visibility.Collapsed;
            Btn_normal.Visibility = Visibility.Visible;

            this.BorderThickness = new Thickness(0);
        }


        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                WindowResize_NORMAL();
            }
        }


        #endregion


    }
}
