using SatisfactorySaveParser.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

namespace SatisfactorySaveEditor.Cheats
{
    /// <summary>
    /// Interaction logic for MassDeleteWindow.xaml
    /// </summary>
    public partial class MassDismantleWindow : Window
    {
        public MassDismantleWindow(bool isZWindow = false)
        {
            InitializeComponent();
            xCoordinate.GotFocus += RemovePlaceholder;
            xCoordinate.LostFocus += AddPlaceholder;
            yCoordinate.GotFocus += RemovePlaceholder;
            yCoordinate.LostFocus += AddPlaceholder;
            this.isZWindow = isZWindow;
            if(isZWindow)
            {
                xLabel.Content = "minimum Z";
                yLabel.Content = "maximum Z";
                grid.Children.Remove(nextButton);
                Label leaveEmptyLabel = new Label()
                {
                    Content = "Leave placeholder for infinity"
                };
                grid.Children.Add(leaveEmptyLabel);
                Grid.SetRow(leaveEmptyLabel, 2);
                Grid.SetColumn(leaveEmptyLabel, 0);
            }
        }

        private bool isZWindow = false;
        private bool isPlaceHolderX = true;
        private bool isPlaceHolderY = true;

        public void RemovePlaceholder(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "123,456.78")
            {
                ((TextBox)sender).Text = "";
                if (sender == xCoordinate)
                    isPlaceHolderX = false;
                if (sender == yCoordinate)
                    isPlaceHolderY = false;
            }
        }

        public void AddPlaceholder(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
            {
                ((TextBox)sender).Text = "123,456.78";
                if (sender == xCoordinate)
                    isPlaceHolderX = true;
                if (sender == yCoordinate)
                    isPlaceHolderY = true;
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            xCoordinate.Focus();
            yCoordinate.Focus();
            xCoordinate.Focus();
        }

        private Vector3 result;
        public bool ResultSet { get; private set; }
        public Vector3 Result => result;
        public bool Done { get; private set; }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(xCoordinate.Text) && !string.IsNullOrWhiteSpace(yCoordinate.Text) && !isPlaceHolderX && !isPlaceHolderY)
                {
                    result = new Vector3()
                    {
                        X = float.Parse(xCoordinate.Text),
                        Y = float.Parse(yCoordinate.Text)
                    };
                    ResultSet = true;
                    DialogResult = true;
                }
                else
                    MessageBox.Show("Enter coordinates");
            }
            catch (FormatException)
            {
                MessageBox.Show("Coordinate format: 123.45");
            }
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((!string.IsNullOrWhiteSpace(xCoordinate.Text) && !string.IsNullOrWhiteSpace(yCoordinate.Text) && !isPlaceHolderX && !isPlaceHolderY) || isZWindow)
                {
                    result = new Vector3()
                    {
                        X = float.Parse(xCoordinate.Text),
                        Y = float.Parse(yCoordinate.Text)
                    };
                    ResultSet = true;
                }
                if(isZWindow)
                {
                    if (isPlaceHolderX)
                        result.X = float.NegativeInfinity;
                    if (isPlaceHolderY)
                        result.Y = float.PositiveInfinity;
                }
                DialogResult = true;
                Done = true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Coordinate format: 123.45");
            }
        }
    }
}
