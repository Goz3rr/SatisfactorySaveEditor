using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using SatisfactorySaveEditor.Message;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel;

namespace SatisfactorySaveEditor.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Width = Properties.Settings.Default.WindowWidth;
            Height = Properties.Settings.Default.WindowHeight;

            if (Properties.Settings.Default.WindowLeft > 0) Left = Properties.Settings.Default.WindowLeft;
            if (Properties.Settings.Default.WindowTop > 0) Top = Properties.Settings.Default.WindowTop;
            Messenger.Default.Register<DrawerEnabledMessage>(this, UpdateColumnWidth);
            Messenger.Default.Register<DarkModeEnabledMessage>(this, SetLightDark);
        }

        private void TreeViewSelectedItemChanged(object sender, RoutedEventArgs e)
        {
            if (sender is TreeViewItem item)
            {
                item.BringIntoView();
                e.Handled = true;  
            }
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            Properties.Settings.Default.WindowWidth = Width;
            Properties.Settings.Default.WindowHeight = Height;
            Properties.Settings.Default.WindowLeft = Left;
            Properties.Settings.Default.WindowTop = Top;
            Properties.Settings.Default.Save();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed) return;

            var window = GetWindow(this);
            window?.DragMove();
        }

        private void UpdateColumnWidth(DrawerEnabledMessage message)
        {
            if (message.DrawerEnabled)
            {
                FixedTreeColumn.Width = new GridLength(0);
            }
            else
            {
                FixedTreeColumn.Width = GridLength.Auto;
            }
        }

        private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow == null) return;
            Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState == WindowState.Normal ? WindowState.Minimized : WindowState.Normal;
        }

        private void MaximizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow == null) return;
            Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        public void SetLightDark(DarkModeEnabledMessage message)
        {
            var existingResourceDictionary = Application.Current.Resources.MergedDictionaries
                .Where(rd => rd.Source != null)
                .SingleOrDefault(rd => Regex.Match(rd.Source.OriginalString, @"(\/MaterialDesignThemes.Wpf;component\/Themes\/MaterialDesignTheme\.)((Light)|(Dark))").Success);
            if (existingResourceDictionary == null)
                throw new ApplicationException("Unable to find Light/Dark base theme in Application resources.");

            var source =
                $"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.{(message.DarkModeEnabled ? "Dark" : "Light")}.xaml";
            var newResourceDictionary = new ResourceDictionary() { Source = new Uri(source) };

            Application.Current.Resources.MergedDictionaries.Remove(existingResourceDictionary);
            Application.Current.Resources.MergedDictionaries.Add(newResourceDictionary);

            var existingMahAppsResourceDictionary = Application.Current.Resources.MergedDictionaries
                .Where(rd => rd.Source != null)
                .SingleOrDefault(rd => Regex.Match(rd.Source.OriginalString, @"(\/MahApps.Metro;component\/Styles\/Accents\/)((BaseLight)|(BaseDark))").Success);
            if (existingMahAppsResourceDictionary == null) return;

            source =
                $"pack://application:,,,/MahApps.Metro;component/Styles/Accents/{(message.DarkModeEnabled ? "BaseDark" : "BaseLight")}.xaml";
            var newMahAppsResourceDictionary = new ResourceDictionary { Source = new Uri(source) };

            Application.Current.Resources.MergedDictionaries.Remove(existingMahAppsResourceDictionary);
            Application.Current.Resources.MergedDictionaries.Add(newMahAppsResourceDictionary);
            ((MainViewModel)DataContext).WindowTheme = Properties.Settings.Default.DarkModeEnabled ? BaseTheme.Dark : BaseTheme.Light;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetLightDark(new DarkModeEnabledMessage(Properties.Settings.Default.DarkModeEnabled));
        }
    }
}