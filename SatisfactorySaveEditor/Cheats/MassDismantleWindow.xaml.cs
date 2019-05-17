using SatisfactorySaveParser.Structures;
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

namespace SatisfactorySaveEditor.Cheats
{
    /// <summary>
    /// Interaction logic for MassDeleteWindow.xaml
    /// </summary>
    public partial class MassDismantleWindow : Window
    {
        public MassDismantleWindow()
        {
            InitializeComponent();
            xCoordinate.GotFocus += RemovePlaceholder;
            xCoordinate.LostFocus += AddPlaceholder;
            yCoordinate.GotFocus += RemovePlaceholder;
            yCoordinate.LostFocus += AddPlaceholder;
        }

        bool isPlaceHolderX = true;
        bool isPlaceHolderY = true;

        public void RemovePlaceholder(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "1234.56")
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
                ((TextBox)sender).Text = "1234.56";
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

        public Vector3 Result { get; private set; } = null;
        public bool Done { get; private set; }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(xCoordinate.Text) && !string.IsNullOrWhiteSpace(yCoordinate.Text) && !isPlaceHolderX && !isPlaceHolderY)
                {
                    Result = new Vector3()
                    {
                        X = float.Parse(xCoordinate.Text),
                        Y = float.Parse(yCoordinate.Text)
                    };
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
                if (!string.IsNullOrWhiteSpace(xCoordinate.Text) && !string.IsNullOrWhiteSpace(yCoordinate.Text) && !isPlaceHolderX && !isPlaceHolderY)
                {
                    Result = new Vector3()
                    {
                        X = float.Parse(xCoordinate.Text),
                        Y = float.Parse(yCoordinate.Text)
                    };
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
