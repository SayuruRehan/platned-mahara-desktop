using CommunityToolkit.WinUI.UI.Controls;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.UI;
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
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }

        public PagePassUserManagement()
        {
            this.InitializeComponent();
            LoadData();
            dataGrid.ItemsSource = GridItemsUser;

            // Mahara-85
            AccessCheck();
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
                    UserRole = selectedItem.TreeNodesUserRole[0].Name.Trim(),
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
                        ValidFrom = dialogUser.ValidFrom ?? DateTime.Now,
                        ValidTo = dialogUser.ValidTo ?? DateTime.Now.AddDays(365),
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
                        // Mahara-85
                        AccessCheck();

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
                // Mahara-92 - Added cancel info messge - START
                else
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Info", "User cancelled the dialog.", InfoBarSeverity.Informational);
                    }
                }
                // Mahara-92 - END
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
            // Mahara-86 - START
            if (Array.Exists(GlobalData.AccessRoleArraySuper, superRole => superRole == GlobalData.UserRole.Trim()))
            {
                pass_Users = new AuthPlatnedPass().GetPass_Users();
            }
            else if (Array.Exists(GlobalData.AccessRoleArrayUserAdmin, superRole => superRole == GlobalData.UserRole.Trim()))
            {
                Pass_Users_Company userDet = new Pass_Users_Company
                {
                    CompanyID = GlobalData.CompanyId
                };

                pass_Users = new AuthPlatnedPass().GetPass_Users_Per_Company(userDet);
            }
            else if (Array.Exists(GlobalData.AccessRoleArrayUser, superRole => superRole == GlobalData.UserRole.Trim()))
            {
                Pass_Users_Company userDet = new Pass_Users_Company
                {
                    CompanyID = GlobalData.CompanyId,
                    UserID = GlobalData.UserId,
                };

                var singleResult = new AuthPlatnedPass().GetPass_User_Per_Company(userDet);
                pass_Users = new List<Pass_Users_Company> { singleResult };
            }
            // Mahara-86 - END

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
                // Mahara-92 - START
                AccessCheck();
                // Mahara-92 - END
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

                //Null validation 
                if (dialogUser.UserCompanyId == "" || dialogUser.UserId == "" || dialogUser.UserName == "" || !dialogUser.ValidFrom.HasValue || dialogUser.UserEmail == "" || dialogUser.UserRole == "" )
                {

                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please check the details.", InfoBarSeverity.Warning);
                    }
                    //Creae Dialog content with field value
                    var userDialogPage = new DialogUser(false)
                    {
                        UserCompanyId = dialogUser.UserCompanyId,
                        UserId = dialogUser.UserId,
                        UserName = dialogUser.UserName,
                        ValidTo = dialogUser.ValidTo,
                        ValidFrom = dialogUser.ValidFrom,
                        UserEmail = dialogUser.UserEmail,
                    };

                    //refill value that added
                    var resultNew = await ShowAddUserDialog(userDialogPage);
                    await HandleAddUserDialogResultAsync(resultNew, userDialogPage);

                }
                else {
                    string generatedTempPassword = GenerateRandomString(8);
                    Pass_Users_Company pass_User = new Pass_Users_Company
                    {
                        CompanyID = dialogUser.UserCompanyId,
                        UserID = dialogUser.UserId,
                        UserName = dialogUser.UserName,
                        UserEmail = dialogUser.UserEmail,
                        ValidFrom = dialogUser.ValidFrom ?? DateTime.Now,
                        ValidTo = dialogUser.ValidTo ?? DateTime.Now.AddDays(365),
                        UserRole = dialogUser.UserRole.Trim(),
                        Password = Encrypt.EncryptPassword(generatedTempPassword),
                        LicenseKey = GenerateRandomString(),
                        RowState = "Active",
                        CreatedBy = GlobalData.UserId == null ? "User1" : GlobalData.UserId
                    };
                    bool authResponse = new AuthPlatnedPass().CreateNewUser(pass_User);
                    if (authResponse)
                    {

                        //Create Email content
                        string emailContent = $@"
<table
     style=""width: 100%; margin-top: 50px; table-layout: fixed; border-collapse: collapse; font-family: 'Open Sans', Helvetica, Arial, sans-serif;""
     width=""100%""
     border=""0""
     cellpadding=""0""
     cellspacing=""0""
>
     <tbody>
          <tr>
               <td align=""center"">
                    <div style=""min-width: 318px; max-width: 520px; margin: auto"">
                         <!-- Header Section -->
                         <div
                              style=""font-family: 'Open Sans', Helvetica, Arial, sans-serif; border-bottom: thin solid #dadce0; color: rgba(0, 0, 0, 0.87); line-height: 32px; padding-bottom: 24px; text-align: center;""
                         >
                              <div style=""font-size: 44px; font-weight: 600;"">Welcome</div>
                              <div style=""font-size: 14px; color: rgba(0, 0, 0, 0.87); margin-top: 8px"">
                                   <a href=""mailto:{pass_User.UserEmail}"" style=""color: inherit; text-decoration: none;"">
                                        akila.madurangana@platned.com
                                   </a>
                              </div>
                         </div>

                         <!-- Image Section -->
                         <div style=""margin-top: 16px"">
                              <img
                                   src=""https://res.cloudinary.com/dqmgkczgk/image/upload/v1734420452/Platned_mahara/5568706_lw26go.png""
                                   style=""border-radius: 28px; max-width: 100%; display: block;""
                                   alt=""Welcome Image""
                              />
                         </div>

                         <!-- Body Content -->
                         <div style=""text-align: left; font-family: 'Open Sans', Helvetica, Arial, sans-serif; margin: 20px;"">
                              <p style=""color: #3c4043; font-size: 14px; line-height: 20px;"">Hi{pass_User.UserName},</p>
                              <p style=""color: #3c4043; font-size: 14px; line-height: 20px;"">
                                   Welcome to <strong>Platned Mahara</strong>! <br> We are excited to have you on board. To get started, please log in to your
                                   account and complete the initial setup.
                              </p>

                              <div style=""margin-top: 20px; background-color: #fafbff; padding: 20px; border-radius: 8px;"">
                                   <p style=""color: #212121; font-size: 22px; font-weight: 400;"">Here are your login details:</p>
                                   <ul style=""color: #3c4043; font-size: 14px;"">
                                        <li><strong>Username:</strong> {pass_User.UserName}</li>
                                        <li><strong>Temporary Password:</strong> {generatedTempPassword}</li>
                                        <li><strong>License Key:</strong> {pass_User.LicenseKey}</li>
                                   </ul>
                                   <p style=""color: #3c4043; font-size: 14px; line-height: 20px;"">
                                        For security purposes, please reset your password during your first login.
                                   </p>
                                   <ol style=""color: #3c4043; font-size: 14px;"">
                                        <li>Go to Platned Mahara</li>
                                        <li>Enter your username and temporary password.</li>
                                        <li>Follow the instructions to reset your password.</li>
                                   </ol>
                              </div>
                              <p style=""color: #3c4043; font-size: 14px;"">
                                   If the issue persists, contact us at 
                                   <a href=""mailto:mahara@platned.com"" style=""color: inherit;"">
                                        mahara@platned.com
                                   </a> for further assistance.
                              </p>
                         </div>

                         <!-- Footer -->
                         <div style=""margin: 16px 0; text-align: center; font-size: 12px; color: #70757a;"">
                              <img
                                   src=""https://res.cloudinary.com/dqmgkczgk/image/upload/v1734331807/Platned_mahara/PlatnedLogo_v2rddw.png""
                                   style=""width: 80px; margin-bottom: 8px;""
                                   alt=""Platned Logo""
                              />
                              <div>Platned Mahara</div>
                         </div>
                    </div>
               </td>
          </tr>
     </tbody>
</table>";

                        //send mail as asynconyce 
                        bool emailSent = await EmailSender.MailSenderAsync(pass_User.UserEmail, "Mahara New User Account Create", emailContent, true);

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

        #region Mahara-85 - Access Check

        private async Task AccessCheck()
        {
            if (AccessControl.IsGranted("BTN_NEW_USER", "C"))
            { btnNewUser.Visibility = Visibility.Visible; }
            else { btnNewUser.Visibility = Visibility.Collapsed; }

            // Mahara-88 - Making Access check for Edit, Delete via BG thread - START
            await Task.Run(() =>
            {
                foreach (var user in GridItemsUser)
                {
                    user.CanEdit = AccessControl.IsGranted("BTN_EDIT_USER", "U");
                    user.CanDelete = AccessControl.IsGranted("BTN_DELETE_USER ", "D");
                }
            });
            // Mahara-88 - END
            
            dataGrid.ItemsSource = null; // Refresh binding
            dataGrid.ItemsSource = GridItemsUser;
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
        public bool CanDelete { get; internal set; }
        public bool CanEdit { get; internal set; }
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
