using SatisfactorySaveParser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var save = new SatisfactorySave(@"%userprofile%\Documents\My Games\FactoryGame\SaveGame\test_090319-140542.sav");

            var rootNode = new SaveNodeItem("Root");
            var saveTree = new SaveTreeNode("Root");

            foreach (var entry in save.SaveEntries)
            {
                var parts = entry.Str1.TrimStart('/').Split('/');
                saveTree.AddChild(parts, entry);
            }

            BuildNode(rootNode.Items, saveTree);
            SaveNodeTreeView.Items.Add(rootNode);
        }

        private void BuildNode(ObservableCollection<SaveNodeItem> items, SaveTreeNode node)
        {
            foreach(var child in node.Children)
            {
                var childItem = new SaveNodeItem(child.Value.Name);
                BuildNode(childItem.Items, child.Value);
                items.Add(childItem);
            }

            foreach(var entry in node.Content)
            {
                items.Add(new SaveNodeItem(entry.Str3));
            }
        }
    }
}
