using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PlatnedMahara.Classes;
using PlatnedMahara.Pages.PlatnedPassPages.DialogPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

        private async void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            var result = ContentDialogResult.None;
            var dialogUser = new DialogUser(); // Create DialogUser instance once

            // Ensure PagePassUserManagementXamlRoot is loaded
            if (PagePassUserManagementXamlRoot.XamlRoot == null)
            {
                PagePassUserManagementXamlRoot.Loaded += async (s, e) =>
                {
                    result = await ShowAddUserDialog(dialogUser);
                    await HandleAddUserDialogResultAsync(result, dialogUser);
                };
            }
            else
            {
                result = await ShowAddUserDialog(dialogUser);
                await HandleAddUserDialogResultAsync(result, dialogUser);
            }
        }

        private async Task<ContentDialogResult> ShowAddUserDialog(DialogUser dialogUser)
        {
            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = PagePassUserManagementXamlRoot.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                PrimaryButtonText = "Process",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = dialogUser // Set the same DialogUser instance as the content
            };

            return await dialog.ShowAsync(); // Return the result of the dialog
        }

        private async Task HandleAddUserDialogResultAsync(ContentDialogResult result, DialogUser dialogUser)
        {
            if (result == ContentDialogResult.Primary)
            {
                // Access field data from DialogUser
                string userId = dialogUser.UserId;
                string userName = dialogUser.UserName;

                bool authResponse = await AuthPlatnedPass.validateLogin(userId, userName);
                if (authResponse)
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Success!", $"Operation Success for User: {userId}", InfoBarSeverity.Success);
                    }
                }
                else
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please check the details.", InfoBarSeverity.Warning);
                    }

                    var resultNew = ContentDialogResult.None;
                    resultNew = await ShowAddUserDialog(dialogUser);
                    await HandleAddUserDialogResultAsync(resultNew, dialogUser);

                }

            }
            else
            {
                if (App.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowInfoBar("Info", "User cancelled the dialog.", InfoBarSeverity.Informational);
                }
            }
        }


    }
}
