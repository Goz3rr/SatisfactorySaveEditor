using System;
using System.Windows;
using System.Windows.Controls;

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
    }
}