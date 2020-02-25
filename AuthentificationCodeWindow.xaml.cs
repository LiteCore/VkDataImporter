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

namespace VKDataImporter
{
    /// <summary>
    /// Логика взаимодействия для AuthentificationCodeWindow.xaml
    /// </summary>
    public partial class AuthentificationCodeWindow : MetroWindow
    {

        public AuthentificationCodeWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Authorizator.Code = TextBox.Text;
        }
    }
}
