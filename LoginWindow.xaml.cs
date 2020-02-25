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
using MahApps.Metro.Controls;
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;

namespace VKDataImporter
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
            LoginTextBox.Text = Properties.Settings.Default.Login;
            PasswordTextBox.Text = Properties.Settings.Default.Password;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Login = LoginTextBox.Text;
            Properties.Settings.Default.Password = PasswordTextBox.Text;
            if(Authorizator.Authorize(LoginTextBox.Text, PasswordTextBox.Text))
            {
                DialogResult = true;
                this.Close();
            }
            DialogResult = false;
            Properties.Settings.Default.Save();
        }
    }
}
