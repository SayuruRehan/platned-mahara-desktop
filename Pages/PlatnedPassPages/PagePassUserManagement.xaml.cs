using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PlatnedMahara.Pages.PlatnedPassPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PagePassUserManagement : Page
    {
        public ObservableCollection<GridItem> GridItems { get; set; }

        public PagePassUserManagement()
        {
            this.InitializeComponent();
            LoadData();
            dataGrid.ItemsSource = GridItems;
            
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the DataContext of the clicked row
            var button = sender as Button;
            /*var rowData = button.DataContext as YourRowDataType; // Replace YourRowDataType with your actual data type

            if (rowData != null)
            {
                // Perform the edit action
                Debug.WriteLine($"Edit clicked for User ID: {rowData.TreeNodesUserId[0].Name}");
            }*/
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the DataContext of the clicked row
            var button = sender as Button;
            /*var rowData = button.DataContext as YourRowDataType; // Replace YourRowDataType with your actual data type

            if (rowData != null)
            {
                // Perform the delete action
                Debug.WriteLine($"Delete clicked for User ID: {rowData.TreeNodesUserId[0].Name}");
            }*/
        }

        private void LoadData()
        {
            GridItems = new ObservableCollection<GridItem>();
            for (int gi = 1; gi <= 1; gi++)
            {
                var item = new GridItem($"Item {gi}")
                {
                    // Initialize with default values
                    TreeNodesItemName = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesApi = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesDesc = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesStatC = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesRes = new ObservableCollection<TreeNode> { new TreeNode("") }
                };

                GridItems.Add(item);
            }
        }


    }
}
