using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro;
using Windows.UI.Xaml.Controls;

namespace VKDataImporter
{
    /// <summary>
    /// Логика взаимодействия для BrowserWindow.xaml
    /// </summary>
    public partial class BrowserWindow : MetroWindow
    {
        public BrowserWindow()
        {
            InitializeComponent();
            webView.NavigationCompleted += WebView_NavigationCompleted;
        }

        private void WebView_NavigationCompleted(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationCompletedEventArgs e)
        {
            if (e.Uri.Fragment.Contains("access_token"))
            {
                Properties.Settings.Default.Token = e.Uri.Fragment.Split('&')[0].Substring(14);
                DialogResult = true;
                this.Close();
            }
            else if (!e.Uri.AbsoluteUri.Contains(@"oauth.vk.com/authorize"))
            {
                webView.Navigate(@"https://oauth.vk.com/authorize?client_id=7331234&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=0&response_type=token&v=5.52&revoke=1");
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //await Dispatcher.InvokeAsync(async() =>
            //{
            //    await WebView.ClearTemporaryWebDataAsync();
            //});
            //webView.Refresh();
            webView.Navigate(@"https://oauth.vk.com/authorize?client_id=7331234&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=0&response_type=token&v=5.52&revoke=1");

        }
    }
}
