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

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the DataContext of the clicked row
            var button = sender as Button;

            if (button?.DataContext is GridItemUser selectedItem)
            {
                // Load selected record data into DialogUser
                var dialogUser = new DialogUser(true)
                {
                    UserCompanyId = selectedItem.TreeNodesCompanyId[0].Name,
                    UserId = selectedItem.TreeNodesUserId[0].Name,
                    UserName = selectedItem.TreeNodesUserName[0].Name,
                    UserEmail = selectedItem.TreeNodesUserEmail[0].Name,
                    UserRole = selectedItem.TreeNodesUserRole[0].Name,
                    ValidFrom = DateTime.Parse(selectedItem.TreeNodesValidFrom[0].Name),
                    ValidTo = DateTime.Parse(selectedItem.TreeNodesValidTo[0].Name),
                    RowState = selectedItem.TreeNodesRowState[0].Name
                };

                var result = await ShowAddUserDialog(dialogUser);

                if (result == ContentDialogResult.Primary)
                {
                    // Update the record in the database
                    Pass_Users_Company updatedUser = new Pass_Users_Company
                    {
                        CompanyID = dialogUser.UserCompanyId,
                        UserID = dialogUser.UserId,
                        UserName = dialogUser.UserName,
                        UserEmail = dialogUser.UserEmail,
                        ValidFrom = (DateTime)dialogUser.ValidFrom,
                        ValidTo = dialogUser.ValidTo,
                        UserRole = dialogUser.UserRole,
                        RowState = dialogUser.RowState
                    };

                    bool isUpdated = new AuthPlatnedPass().EditUser(updatedUser);

                    if (isUpdated)
                    {
                        // Update the GridItemsUser collection
                        selectedItem.TreeNodesUserName[0].Name = dialogUser.UserName;
                        selectedItem.TreeNodesUserEmail[0].Name = dialogUser.UserEmail;
                        selectedItem.TreeNodesUserRole[0].Name = dialogUser.UserRole;
                        selectedItem.TreeNodesValidFrom[0].Name = dialogUser.ValidFrom.ToString();
                        selectedItem.TreeNodesValidTo[0].Name = dialogUser.ValidTo.ToString();
                        selectedItem.TreeNodesRowState[0].Name = dialogUser.RowState;

                        // Refresh DataGrid
                        dataGrid.ItemsSource = null;
                        dataGrid.ItemsSource = GridItemsUser;

                        // Show success message
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Success!", $"User updated successfully.", InfoBarSeverity.Success);
                        }
                    }
                    else
                    {
                        // Show error message
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Error", $"Failed to update user.", InfoBarSeverity.Error);
                        }
                    }
                }
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the DataContext of the clicked row
            var button = sender as Button;

            if (button?.DataContext is GridItemUser selectedItem)
            {
                // Confirm delete action
                var dialog = new ContentDialog
                {
                    Title = "Delete Confirmation",
                    Content = $"Are you sure you want to delete user {selectedItem.TreeNodesUserName[0].Name}?",
                    PrimaryButtonText = "Yes",
                    CloseButtonText = "No",
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = PagePassUserManagementXamlRoot.XamlRoot
                };

                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    Pass_Users_Company pass_User = new Pass_Users_Company
                    {
                        CompanyID = selectedItem.TreeNodesCompanyId[0].Name,
                        UserID = selectedItem.TreeNodesUserId[0].Name
                    };

                    // Call your delete method
                    bool isDeleted = new AuthPlatnedPass().DeleteUser(pass_User);

                    if (isDeleted)
                    {
                        // Remove the item from the ObservableCollection
                        GridItemsUser.Remove(selectedItem);

                        // Show success message
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Success!", $"User deleted successfully.", InfoBarSeverity.Success);
                        }
                    }
                    else
                    {
                        // Show error message
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Error", $"Failed to delete user.", InfoBarSeverity.Error);
                        }
                    }
                }
            }
        }

        private void LoadData()
        {
            List<Pass_Users_Company> pass_Users = new List<Pass_Users_Company>();
            pass_Users = new AuthPlatnedPass().GetPass_Users();

            GridItemsUser = new ObservableCollection<GridItemUser>();
            if (pass_Users != null && pass_Users.Count > 0)
            {
                foreach (Pass_Users_Company pu in pass_Users)
                {
                    // Pass values needed to display in grid
                    var item = new GridItemUser($"Company ID", "UserId", "UserName", "UserEmail", "LicKey", "ValidFrom", "ValidTo", "RowState", "CreDate", "CreBy", "ModDate", "ModBy", "UserRole")
                    {
                        // Initialize with default values
                        TreeNodesCompanyId = new ObservableCollection<TreeNode> { new TreeNode(pu.CompanyID) },
                        TreeNodesUserId = new ObservableCollection<TreeNode> { new TreeNode(pu.UserID) },
                        TreeNodesUserName = new ObservableCollection<TreeNode> { new TreeNode(pu.UserName) },
                        TreeNodesUserEmail = new ObservableCollection<TreeNode> { new TreeNode(pu.UserEmail) },
                        TreeNodesLicenseKey = new ObservableCollection<TreeNode> { new TreeNode(pu.LicenseKey) },
                        TreeNodesValidFrom = new ObservableCollection<TreeNode> { new TreeNode(pu.ValidFrom.ToString()) },
                        TreeNodesValidTo = new ObservableCollection<TreeNode> { new TreeNode(pu.ValidTo.ToString()) },
                        TreeNodesRowState = new ObservableCollection<TreeNode> { new TreeNode(pu.RowState) },
                        TreeNodesCreatedDate = new ObservableCollection<TreeNode> { new TreeNode(pu.CreatedDate.ToString()) },
                        TreeNodesCreatedBy = new ObservableCollection<TreeNode> { new TreeNode(pu.CreatedBy) },
                        TreeNodesModifiedDate = new ObservableCollection<TreeNode> { new TreeNode(pu.ModifiedDate.ToString()) },
                        TreeNodesModifiedBy = new ObservableCollection<TreeNode> { new TreeNode(pu.ModifiedBy) },
                        TreeNodesUserRole = new ObservableCollection<TreeNode> { new TreeNode(pu.UserRole) }
                    };

                    GridItemsUser.Add(item);
                }
            }
        }

        private async void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            var result = ContentDialogResult.None;
            var dialogUser = new DialogUser(false); // Create DialogUser instance once

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
                LoadData();
                dataGrid.ItemsSource = null; // Clear the existing binding
                dataGrid.ItemsSource = GridItemsUser;
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
                /*string userId = dialogUser.UserId;
                string userName = dialogUser.UserName;*/
                //Null validation 

                //Null validation
                DateTime? validFrom = dialogUser.ValidFrom;
                if (dialogUser.UserCompanyId == "" || dialogUser.UserId == "" || dialogUser.UserName == "" || !validFrom.HasValue || dialogUser.UserEmail == "" || dialogUser.UserRole == "" )
                {

                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please ensure no fields are left empty..", InfoBarSeverity.Warning);
                    }
                    //Creae Dialog content with field value
                    var userDialogPage = new DialogUser(false)
                    {
                        UserCompanyId = dialogUser.UserCompanyId,
                        UserId = dialogUser.UserId,
                        UserName = dialogUser.UserName,
                        ValidFrom = dialogUser.ValidFrom,
                        UserEmail = dialogUser.UserEmail,
                    };

                    //refill value that added
                    var resultNew = await ShowAddUserDialog(userDialogPage);
                    await HandleAddUserDialogResultAsync(resultNew, userDialogPage);

                }
                else {
                    Pass_Users_Company pass_User = new Pass_Users_Company
                    {
                        CompanyID = dialogUser.UserCompanyId,
                        UserID = dialogUser.UserId,
                        UserName = dialogUser.UserName,
                        UserEmail = dialogUser.UserEmail,
                        ValidFrom = (DateTime)dialogUser.ValidFrom,
                        ValidTo = DateTime.Now.AddDays(365),
                        UserRole = dialogUser.UserRole,
                        Password = Encrypt.EncryptPassword("defaultpass1234"),
                        LicenseKey = GenerateRandomString(),
                        RowState = "Active",
                        CreatedBy = GlobalData.UserId
                    };
                    bool authResponse = new AuthPlatnedPass().CreateNewUser(pass_User);
                    if (authResponse)
                    {
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Success!", $"Operation Success for User: {pass_User.UserID}", InfoBarSeverity.Success);
                        }

                    }
                    else
                    {
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please check the details.", InfoBarSeverity.Warning);
                        }

                        dialogUser = new DialogUser();
                        var resultNew = await ShowAddUserDialog(dialogUser);
                        await HandleAddUserDialogResultAsync(resultNew, dialogUser);

                    }
                }
                //bool authResponse = await AuthPlatnedPass.validateLogin(userId, userName);

            }
            else
            {
                if (App.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowInfoBar("Info", "User cancelled the dialog.", InfoBarSeverity.Informational);
                }
            }
        }

        #region Mahara-79

        private static string GenerateRandomString(int length = 20)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion


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
            TreeNodesUserId.Add(new TreeNode(userId));
            TreeNodesUserName.Add(new TreeNode(userName));
            TreeNodesUserEmail.Add(new TreeNode(userEmail));
            TreeNodesLicenseKey.Add(new TreeNode(licenseKey));
            TreeNodesValidFrom.Add(new TreeNode(validFrom));
            TreeNodesValidTo.Add(new TreeNode(validTo));
            TreeNodesRowState.Add(new TreeNode(rowState));
            TreeNodesCreatedDate.Add(new TreeNode(createdDate));
            TreeNodesCreatedBy.Add(new TreeNode(createdBy));
            TreeNodesModifiedDate.Add(new TreeNode(modifiedDate));
            TreeNodesModifiedBy.Add(new TreeNode(modifiedBy));
            TreeNodesUserRole.Add(new TreeNode(userRole));
        }
    }
}
