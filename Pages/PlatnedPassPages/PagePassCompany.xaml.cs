using CommunityToolkit.WinUI.UI.Controls;
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
    public sealed partial class PagePassCompany : Page
    {
        public ObservableCollection<GridItem> GridItems { get; set; }

        public PagePassCompany()
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

        private async void btnNewCompany_Click(object sender, RoutedEventArgs e)
        {
            var result = ContentDialogResult.None;
            var dialogCompany = new DialogCompany(); // Create dialogCompany instance once

            // Ensure PagePassUserManagementXamlRoot is loaded
            if (PagePassCompanyXamlRoot.XamlRoot == null)
            {
                PagePassCompanyXamlRoot.Loaded += async (s, e) =>
                {
                    result = await ShowAddCompanyDialog(dialogCompany);
                    await HandleAddCompanyDialogResultAsync(result, dialogCompany);
                };
            }
            else
            {
                result = await ShowAddCompanyDialog(dialogCompany);
                await HandleAddCompanyDialogResultAsync(result, dialogCompany);
            }
        }

        private async Task<ContentDialogResult> ShowAddCompanyDialog(DialogCompany dialogCompany)
        {
            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = PagePassCompanyXamlRoot.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                PrimaryButtonText = "Process",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = dialogCompany // Set the same dialogCompany instance as the content
            };

            return await dialog.ShowAsync(); // Return the result of the dialog
        }

        private async Task HandleAddCompanyDialogResultAsync(ContentDialogResult result, DialogCompany dialogCompany)
        {
            if (result == ContentDialogResult.Primary)
            {
                // Access field data from dialogCompany
                string userId = dialogCompany.UserId;
                string userName = dialogCompany.UserName;

                bool authResponse = await AuthPlatnedPass.validateLogin(userId, userName);
                if (authResponse)
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Success!", $"Operation Success for Company: {userId}", InfoBarSeverity.Success);
                    }
                }
                else
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please check the details.", InfoBarSeverity.Warning);
                    }

                    var resultNew = ContentDialogResult.None;
                    resultNew = await ShowAddCompanyDialog(dialogCompany);
                    await HandleAddCompanyDialogResultAsync(resultNew, dialogCompany);

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
