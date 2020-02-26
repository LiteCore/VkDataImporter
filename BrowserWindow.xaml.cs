using System.Linq;
using System.Windows;

namespace VKDataImporter
{
    /// <summary>
    /// Логика взаимодействия для BrowserWindow.xaml
    /// </summary>
    public class BrowserWindow : MetroWindow
    {
        public BrowserWindow()
        {
            InitializeComponent();
            webView.NavigationCompleted += WebView_NavigationCompleted;
        }

        private void WebView_NavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs e)
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
            webView.Navigate(@"https://oauth.vk.com/authorize?client_id=7331234&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=0&response_type=token&v=5.52&revoke=1");
        }
    }
}