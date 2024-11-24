using CommunityToolkit.WinUI.UI.Controls;
using DocumentFormat.OpenXml.Spreadsheet;
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
    public sealed partial class PagePassUserManagement : Microsoft.UI.Xaml.Controls.Page
    {
        public ObservableCollection<GridItemUser> GridItemsUser { get; set; }

        public PagePassUserManagement()
        {
            this.InitializeComponent();
            LoadData();
            dataGrid.ItemsSource = GridItemsUser;

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
            GridItemsUser = new ObservableCollection<GridItemUser>();
            for (int gi = 1; gi <= 1; gi++)
            {
                // Pass values needed to display in grid
                var item = new GridItemUser($"Company ID {gi}","UserId","UserName","UserEmail","LicKey","ValidFrom","ValidTo","RowState","CreDate","CreBy","ModDate","ModBy","UserRole")
                {
                    // Initialize with default values
                    TreeNodesCompanyId = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesUserId = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesUserName = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesUserEmail = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesLicenseKey = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesValidFrom = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesValidTo = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesRowState = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesCreatedDate = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesCreatedBy = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesModifiedDate = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesModifiedBy = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesUserRole = new ObservableCollection<TreeNode> { new TreeNode("") }
                };

                GridItemsUser.Add(item);
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

    /// <summary>
    /// Other Classes
    /// </summary>

    public class TreeNodeUser
    {
        public string Name { get; set; }
        public ObservableCollection<TreeNodeUser> Children { get; set; }

        public TreeNodeUser(string name)
        {
            Name = name;
            Children = new ObservableCollection<TreeNodeUser>();
        }
    }

    public class GridItemUser
    {
        public ObservableCollection<TreeNode> TreeNodesCompanyId { get; set; }
        public ObservableCollection<TreeNode> TreeNodesUserId { get; set; }
        public ObservableCollection<TreeNode> TreeNodesUserName { get; set; }
        public ObservableCollection<TreeNode> TreeNodesUserEmail { get; set; }
        public ObservableCollection<TreeNode> TreeNodesLicenseKey { get; set; }
        public ObservableCollection<TreeNode> TreeNodesValidFrom { get; set; }
        public ObservableCollection<TreeNode> TreeNodesValidTo { get; set; }
        public ObservableCollection<TreeNode> TreeNodesRowState { get; set; }
        public ObservableCollection<TreeNode> TreeNodesCreatedDate { get; set; }
        public ObservableCollection<TreeNode> TreeNodesCreatedBy { get; set; }
        public ObservableCollection<TreeNode> TreeNodesModifiedDate { get; set; }
        public ObservableCollection<TreeNode> TreeNodesModifiedBy { get; set; }
        public ObservableCollection<TreeNode> TreeNodesUserRole { get; set; }

        public GridItemUser(string companyId, string userId, string userName, string userEmail, string licenseKey, string validFrom, string validTo, string rowState, string createdDate, string createdBy, string modifiedDate, string modifiedBy, string userRole)
        {
            TreeNodesCompanyId = new ObservableCollection<TreeNode>();
            TreeNodesUserId = new ObservableCollection<TreeNode>();
            TreeNodesUserName = new ObservableCollection<TreeNode>();
            TreeNodesUserEmail = new ObservableCollection<TreeNode>();
            TreeNodesLicenseKey = new ObservableCollection<TreeNode>();
            TreeNodesValidFrom = new ObservableCollection<TreeNode>();
            TreeNodesValidTo = new ObservableCollection<TreeNode>();
            TreeNodesRowState = new ObservableCollection<TreeNode>();
            TreeNodesCreatedDate = new ObservableCollection<TreeNode>();
            TreeNodesCreatedBy = new ObservableCollection<TreeNode>();
            TreeNodesModifiedDate = new ObservableCollection<TreeNode>();
            TreeNodesModifiedBy = new ObservableCollection<TreeNode>();
            TreeNodesUserRole = new ObservableCollection<TreeNode>();

            TreeNodesCompanyId.Add(new TreeNode(companyId));
            TreeNodesCompanyId.Add(new TreeNode(userId));
            TreeNodesCompanyId.Add(new TreeNode(userName));
            TreeNodesCompanyId.Add(new TreeNode(userEmail));
            TreeNodesCompanyId.Add(new TreeNode(licenseKey));
            TreeNodesCompanyId.Add(new TreeNode(validFrom));
            TreeNodesCompanyId.Add(new TreeNode(validTo));
            TreeNodesCompanyId.Add(new TreeNode(rowState));
            TreeNodesCompanyId.Add(new TreeNode(createdDate));
            TreeNodesCompanyId.Add(new TreeNode(createdBy));
            TreeNodesCompanyId.Add(new TreeNode(modifiedDate));
            TreeNodesCompanyId.Add(new TreeNode(modifiedBy));
            TreeNodesCompanyId.Add(new TreeNode(userRole));
        }
    }
}
