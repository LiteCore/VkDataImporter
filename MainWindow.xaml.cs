using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using MahApps.Metro.Controls;
using MahApps.Metro;
using Application = System.Windows.Application;

namespace VKDataImporter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            SetStyle(ThemeManager.GetAppTheme(Properties.Settings.Default.Theme), ThemeManager.GetAccent(Properties.Settings.Default.Accent));
        }
        DoubleAnimation windowHeightAnimation;
        bool IsMaximized = true;
        bool IsAnimated = false;
        //bool IsLoaded = false;

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchWindowState();
        }

        /// <summary>
        /// Меняет состояние окна(для настроек)
        /// </summary>
        private void SwitchWindowState()
        {
            if (!IsAnimated)
            {
                IsAnimated = true;
                windowHeightAnimation.From = this.Height;
                windowHeightAnimation.To = IsMaximized ? this.Height + 85 : this.Height - 85;
                if (IsMaximized)
                    SettingsGrid.Visibility = Visibility.Visible;
                BeginAnimation(HeightProperty, windowHeightAnimation);
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            windowHeightAnimation = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(0.6d),
                EasingFunction = new QuarticEase()
                {
                    EasingMode = EasingMode.EaseInOut
                }
            };
            windowHeightAnimation.Completed += WindowHeightAnimation_Completed;

            List<WinTheme> themes = new List<WinTheme>()
            {
                new WinTheme(){ Name = "Dark", AppTheme = ThemeManager.GetAppTheme("BaseDark") },
                new WinTheme(){ Name = "Light", AppTheme = ThemeManager.GetAppTheme("BaseLight") }
            };
            ThemeComboBox.ItemsSource = themes;
            ThemeComboBox.DisplayMemberPath = "Name";
            ThemeComboBox.SelectedValuePath = "AppTheme";
            ThemeComboBox.SelectedValue = ThemeManager.DetectAppStyle(Application.Current).Item1;

            List<WinAccent> accents = new List<WinAccent>()
            {
                new WinAccent(){ Name = "Blue", AppAccent = ThemeManager.GetAccent("Blue") },
                new WinAccent(){ Name = "Red", AppAccent = ThemeManager.GetAccent("Red") },
                new WinAccent(){ Name = "Green", AppAccent = ThemeManager.GetAccent("Green") },
                new WinAccent(){ Name = "Purple", AppAccent = ThemeManager.GetAccent("Purple") },
                new WinAccent(){ Name = "Orange", AppAccent = ThemeManager.GetAccent("Orange") },
                new WinAccent(){ Name = "Lime", AppAccent = ThemeManager.GetAccent("Lime") },
                new WinAccent(){ Name = "Emerald", AppAccent = ThemeManager.GetAccent("Emerald") },
                new WinAccent(){ Name = "Teal", AppAccent = ThemeManager.GetAccent("Teal") },
                new WinAccent(){ Name = "Cyan", AppAccent = ThemeManager.GetAccent("Cyan") },
                new WinAccent(){ Name = "Cobalt", AppAccent = ThemeManager.GetAccent("Cobalt") },
                new WinAccent(){ Name = "Indigo", AppAccent = ThemeManager.GetAccent("Indigo") },
                new WinAccent(){ Name = "Violet", AppAccent = ThemeManager.GetAccent("Violet") },
                new WinAccent(){ Name = "Pink", AppAccent = ThemeManager.GetAccent("BaseLight") },
                new WinAccent(){ Name = "Magenta", AppAccent = ThemeManager.GetAccent("MagPinkenta") },
                new WinAccent(){ Name = "Crimson", AppAccent = ThemeManager.GetAccent("Crimson") },
                new WinAccent(){ Name = "Amber", AppAccent = ThemeManager.GetAccent("Amber") },
                new WinAccent(){ Name = "Yellow", AppAccent = ThemeManager.GetAccent("Yellow") },
                new WinAccent(){ Name = "Brown", AppAccent = ThemeManager.GetAccent("Brown") },
                new WinAccent(){ Name = "Yellow", AppAccent = ThemeManager.GetAccent("Yellow") },
                new WinAccent(){ Name = "Olive", AppAccent = ThemeManager.GetAccent("Olive") },
                new WinAccent(){ Name = "Steel", AppAccent = ThemeManager.GetAccent("Steel") },
                new WinAccent(){ Name = "Mauve", AppAccent = ThemeManager.GetAccent("Mauve") },
                new WinAccent(){ Name = "Taupe", AppAccent = ThemeManager.GetAccent("Taupe") },
                new WinAccent(){ Name = "Sienna", AppAccent = ThemeManager.GetAccent("Sienna") }
            };
            AccentComboBox.ItemsSource = accents;
            AccentComboBox.DisplayMemberPath = "Name";
            AccentComboBox.SelectedValuePath = "AppAccent";
            AccentComboBox.SelectedValue = ThemeManager.DetectAppStyle(Application.Current).Item2;

            DayOfBirthButton.IsChecked = Properties.Settings.Default.DayOfBirth;
            CityButton.IsChecked = Properties.Settings.Default.City;
            FriendsButton.IsChecked = Properties.Settings.Default.Friends;
            GroupsButton.IsChecked = Properties.Settings.Default.Groups;
            PrivateMessagesButton.IsChecked = Properties.Settings.Default.PrivateMessages;
        }

        private void WindowHeightAnimation_Completed(object sender, EventArgs e)
        {
            if (!IsMaximized)
                SettingsGrid.Visibility = Visibility.Hidden;
            IsMaximized = !IsMaximized;
            IsAnimated = false;
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog()
            {
                Filter = "XLSX|*.xlsx",
                Title = "Укажите путь до Excel-документа"
            };
            if (saveFile.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            progressBar.Value = 0;
            progressBar.Visibility = Visibility.Visible;
            Progress<Tuple<int, int>> progress = new System.Progress<Tuple<int, int>>((x) =>
            {
                progressBar.Maximum = x.Item2;
                progressBar.Value = x.Item1;
            });
            bool result = await DataImporter.ImportDataAsync(textBox.Text, saveFile.FileName, progress);
            if(result)
                new MessageWindow("Успех", "Данные успешно записаны").ShowDialog();
            else
                new MessageWindow("Неудача", "Не удалось записать данные").ShowDialog();
            progressBar.Visibility = Visibility.Hidden;
        }

        private void SettingButtons_Click(object sender, RoutedEventArgs e)
        {
            WriteSettings();
        }

        /// <summary>
        /// Запись настроек в конфиг.
        /// Сохраняется при закрытии окна.
        /// </summary>
        private void WriteSettings()
        {
            if (IsLoaded)
            {
                Properties.Settings.Default.DayOfBirth = DayOfBirthButton.IsChecked == null ? false : (bool)DayOfBirthButton.IsChecked;
                Properties.Settings.Default.City = CityButton.IsChecked == null ? false : (bool)CityButton.IsChecked;
                Properties.Settings.Default.Friends = FriendsButton.IsChecked == null ? false : (bool)FriendsButton.IsChecked;
                Properties.Settings.Default.Groups = GroupsButton.IsChecked == null ? false : (bool)GroupsButton.IsChecked;
                Properties.Settings.Default.PrivateMessages = PrivateMessagesButton.IsChecked == null ? false : (bool)PrivateMessagesButton.IsChecked;

            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetStyle(ThemeComboBox.SelectedValue as AppTheme, null);
        }

        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetStyle(null, AccentComboBox.SelectedValue as Accent);
        }

        /// <summary>
        /// Смена стиля приложения и запись в конфиг
        /// </summary>
        /// <param name="theme">Тема, может быть null(тогда берётся из текущей)</param>
        /// <param name="accent">Акцент, может быть null(тогда берётся из текущей)</param>
        private void SetStyle(AppTheme theme, Accent accent)
        {
            var app = Application.Current;
            ThemeManager.ChangeAppStyle(app,
                accent ?? ThemeManager.DetectAppStyle(app).Item2,
                theme ?? ThemeManager.DetectAppStyle(app).Item1);
            Properties.Settings.Default.Theme = ThemeManager.DetectAppStyle(app).Item1.Name;
            Properties.Settings.Default.Accent = ThemeManager.DetectAppStyle(app).Item2.Name;
        }

        private void AutirizeButton_Click(object sender, RoutedEventArgs e)
        {
            BrowserWindow window = new BrowserWindow();
            window.ShowDialog();
            //if (window.ShowDialog() == true)
            //{
            //    Properties.Settings.Default.Token = Authorizator.Api.Token;
            //}
        }
    }
    class WinTheme
    {
        public string Name { get; set; }
        public MahApps.Metro.AppTheme AppTheme { get; set; }
    }
    class WinAccent
    {
        public string Name { get; set; }
        public MahApps.Metro.Accent AppAccent { get; set; }
    }
}
