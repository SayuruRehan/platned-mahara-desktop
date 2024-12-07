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
using PlatnedMahara.Classes.Db;
using PlatnedMahara.DataAccess.Methods;
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
using Windows.Networking.Proximity;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PlatnedMahara.Pages.PlatnedPassPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PagePassAccessControl : Microsoft.UI.Xaml.Controls.Page
    {
        public ObservableCollection<GridItemAccessRole> GridItemsAccessRole { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }

        public PagePassAccessControl()
        {
            this.InitializeComponent();
            LoadData();
            dataGrid.ItemsSource = GridItemsAccessRole;

            // Mahara-85
            AccessCheck();
        }

        private void LoadData()
        {
            List<Pass_Access_Control> pass_Access_Roles = new List<Pass_Access_Control>();
            pass_Access_Roles = new AuthPlatnedPass().GetAccess_Records();
            GridItemsAccessRole = new ObservableCollection<GridItemAccessRole>();

            if (pass_Access_Roles != null && pass_Access_Roles.Count > 0)
            {
                foreach (Pass_Access_Control pa in pass_Access_Roles)
                {
                    var item = new GridItemAccessRole($"AppFunction", "AppFunctionDescription", "UserRole", "ReadAllowed", "CreateAllowed", "UpdateAllowed", "DeleteAllowed", "CreDate", "CreBy", "ModDate", "ModBy")
                    {
                        // Initialize with default values
                        TreeNodesAppFunction = new ObservableCollection<TreeNode> { new TreeNode(pa.AppFunction) },
                        TreeNodesAppFuncDescription = new ObservableCollection<TreeNode> { new TreeNode(pa.AppFunctionDescription) },
                        TreeNodesUserRole = new ObservableCollection<TreeNode> { new TreeNode(pa.UserRole.Trim()) },
                        TreeNodesReadAllowed = new ObservableCollection<TreeNode> { new TreeNode(pa.ReadAllowed.ToString()) },
                        TreeNodesCreateAllowed = new ObservableCollection<TreeNode> { new TreeNode(pa.CreateAllowed.ToString()) },
                        TreeNodesUpdateAllowed = new ObservableCollection<TreeNode> { new TreeNode(pa.UpdateAllowed.ToString()) },
                        TreeNodesDeleteAllowed = new ObservableCollection<TreeNode> { new TreeNode(pa.DeleteAllowed.ToString()) },
                        TreeNodesCreatedDate = new ObservableCollection<TreeNode> { new TreeNode(pa.CreatedDate.ToString()) },
                        TreeNodesCreatedBy = new ObservableCollection<TreeNode> { new TreeNode(pa.CreatedBy) },
                        TreeNodesModifiedDate = new ObservableCollection<TreeNode> { new TreeNode(pa.ModifiedDate.ToString()) },
                        TreeNodesModifiedBy = new ObservableCollection<TreeNode> { new TreeNode(pa.ModifiedBy) }

                    };

                    GridItemsAccessRole.Add(item);
                }
            }
        }
        
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the DataContext of the clicked row
            var button = sender as Button;
            var rowData = button.DataContext; // Replace YourRowDataType with your actual data type

            if (rowData != null)
            {
                var result = ContentDialogResult.None;
                if (button?.DataContext is GridItemAccessRole selectedItem)
                {
                    var dailogAccessControl = new DialogAccessControl(true)
                    {
                        AppFunction = selectedItem.TreeNodesAppFunction[0].Name,
                        AppFunctionDescription = selectedItem.TreeNodesAppFuncDescription[0].Name,
                        UserRole = selectedItem.TreeNodesUserRole[0].Name,
                        ReadAllowed = selectedItem.TreeNodesReadAllowed[0].Name,
                        CreateAllowed = selectedItem.TreeNodesCreateAllowed[0].Name,
                        UpdateAllowed = selectedItem.TreeNodesUpdateAllowed[0].Name,
                        DeleteAllowed = selectedItem.TreeNodesDeleteAllowed[0].Name,
                    };
                    if (PagePassAccessRoleXamlRoot.XamlRoot == null)
                    {
                        PagePassAccessRoleXamlRoot.Loaded += async (s, e) =>
                        {
                            result = await ShowAddAccessControlDialog(dailogAccessControl);
                            await HandleEditAccessControlDialogResultAsync(result, dailogAccessControl);
                        };
                    }
                    else
                    {
                        result = await ShowAddAccessControlDialog(dailogAccessControl);
                        await HandleEditAccessControlDialogResultAsync(result, dailogAccessControl);
                        LoadData();
                        dataGrid.ItemsSource = null; // Clear the existing binding
                        dataGrid.ItemsSource = GridItemsAccessRole;
                    }
                }
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var rowData = button.DataContext; // Replace YourRowDataType with your actual data type

            if (rowData != null)
            {
                var result = ContentDialogResult.None;
                if (button?.DataContext is GridItemAccessRole selectedItem)
                {
                    var dailogAccessControl = new DialogAccessControl(true)
                    {
                        AppFunction = selectedItem.TreeNodesAppFunction[0].Name,
                        AppFunctionDescription = selectedItem.TreeNodesAppFuncDescription[0].Name,
                        UserRole = selectedItem.TreeNodesUserRole[0].Name,
                        ReadAllowed = selectedItem.TreeNodesReadAllowed[0].Name,
                        CreateAllowed = selectedItem.TreeNodesCreateAllowed[0].Name,
                        UpdateAllowed = selectedItem.TreeNodesUpdateAllowed[0].Name,
                        DeleteAllowed = selectedItem.TreeNodesDeleteAllowed[0].Name,
                    };
                    if (PagePassAccessRoleXamlRoot.XamlRoot == null)
                    {
                        PagePassAccessRoleXamlRoot.Loaded += async (s, e) =>
                        {
                            result = await ShowAddAccessControlDialog(dailogAccessControl);
                            await HandleDeleteAccessControlDialogResultAsync(result, dailogAccessControl);
                        };
                    }
                    else
                    {
                        result = await ShowAddAccessControlDialog(dailogAccessControl);
                        await HandleDeleteAccessControlDialogResultAsync(result, dailogAccessControl);
                        LoadData();
                        dataGrid.ItemsSource = null; // Clear the existing binding
                        dataGrid.ItemsSource = GridItemsAccessRole;
                    }
                }
            }
        }
        


        private async void btnNewAccessRole_Click(object sender, RoutedEventArgs e)
        {
            var result = ContentDialogResult.None;
            var dialogAccess = new DialogAccessControl(false); // Create dialogCompany instance once

            // Ensure PagePassUserManagementXamlRoot is loaded
            if (PagePassAccessRoleXamlRoot.XamlRoot == null)
            {
                PagePassAccessRoleXamlRoot.Loaded += async (s, e) =>
                {
                    result = await ShowAddAccessControlDialog(dialogAccess);
                    await HandleAddAccessControlDialogResultAsync(result, dialogAccess);
                };
            }
            else
            {
                result = await ShowAddAccessControlDialog(dialogAccess);
                await HandleAddAccessControlDialogResultAsync(result, dialogAccess);
                LoadData();
                dataGrid.ItemsSource = null; // Clear the existing binding
                dataGrid.ItemsSource = GridItemsAccessRole;
            }
        }

        private async Task<ContentDialogResult> ShowAddAccessControlDialog(DialogAccessControl dialogAccess)
        {
            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = PagePassAccessRoleXamlRoot.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                PrimaryButtonText = "Process",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = dialogAccess // Set the same dialogCompany instance as the content
            };

            return await dialog.ShowAsync(); // Return the result of the dialog
        }

        private async Task HandleAddAccessControlDialogResultAsync(ContentDialogResult result, DialogAccessControl dialogAccess)
        {
            if (result == ContentDialogResult.Primary)
            {
                //Null validation
                if (dialogAccess.AppFunction == "" || dialogAccess.AppFunctionDescription == "" || dialogAccess.UserRole == "" || dialogAccess.ReadAllowed == "" || dialogAccess.CreateAllowed == "" || dialogAccess.UpdateAllowed == "" || dialogAccess.DeleteAllowed == "")
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please check the details.", InfoBarSeverity.Warning);
                    }
                    //Create Dialog content with field value
                    var AccessControlDialogPage = new DialogAccessControl()
                    {
                        AppFunction = dialogAccess.AppFunction,
                        AppFunctionDescription = dialogAccess.AppFunctionDescription,
                        UserRole = dialogAccess.UserRole,
                        ReadAllowed = dialogAccess.ReadAllowed,
                        CreateAllowed = dialogAccess.CreateAllowed,
                        UpdateAllowed = dialogAccess.UpdateAllowed,
                        DeleteAllowed = dialogAccess.DeleteAllowed,

                    };

                    //refill value that added
                    var resultNew = await ShowAddAccessControlDialog(AccessControlDialogPage);
                    await HandleAddAccessControlDialogResultAsync(resultNew, AccessControlDialogPage);
                }
                else
                {
                    var ReadAllowed = Convert.ToBoolean(dialogAccess.ReadAllowed);
                    Pass_Access_Control pass_Access_Control = new Pass_Access_Control
                    {
                        AppFunction = dialogAccess.AppFunction,
                        AppFunctionDescription = dialogAccess.AppFunctionDescription,
                        UserRole = dialogAccess.UserRole,
                        ReadAllowed = dialogAccess.ReadAllowed.ToString(),
                        CreateAllowed = dialogAccess.CreateAllowed.ToString(),
                        UpdateAllowed = dialogAccess.UpdateAllowed.ToString(),
                        DeleteAllowed = dialogAccess.DeleteAllowed.ToString(),
                        CreatedBy = GlobalData.UserId,
                    };
                    bool authResponse = new AuthPlatnedPass().CreateNewAccessControl(pass_Access_Control);
                    if (authResponse)
                    {
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Success!", $"Operation Success for Function: {pass_Access_Control.AppFunction} for Role: {dialogAccess.UserRole}", InfoBarSeverity.Success);
                        }
                    }
                    else
                    {
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please check the details.", InfoBarSeverity.Warning);
                        }

                        dialogAccess = new DialogAccessControl()
                        {
                            AppFunction = dialogAccess.AppFunction,
                            AppFunctionDescription = dialogAccess.AppFunctionDescription,
                            UserRole = dialogAccess.UserRole,
                            ReadAllowed = dialogAccess.ReadAllowed,
                            CreateAllowed = dialogAccess.CreateAllowed,
                            UpdateAllowed = dialogAccess.UpdateAllowed,
                            DeleteAllowed = dialogAccess.DeleteAllowed,
                        };
                        var resultNew = await ShowAddAccessControlDialog(dialogAccess);
                        await HandleAddAccessControlDialogResultAsync(resultNew, dialogAccess);
                    }
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

        private async Task HandleEditAccessControlDialogResultAsync(ContentDialogResult result, DialogAccessControl dialogAccess)
        {
            if (result == ContentDialogResult.Primary)
            {
                Pass_Access_Control pass_Access_Control = new Pass_Access_Control
                {
                    AppFunction = dialogAccess.AppFunction,
                    AppFunctionDescription = dialogAccess.AppFunctionDescription,
                    UserRole = dialogAccess.UserRole,
                    ReadAllowed = dialogAccess.ReadAllowed.ToString(),
                    CreateAllowed = dialogAccess.CreateAllowed.ToString(),
                    UpdateAllowed = dialogAccess.UpdateAllowed.ToString(),
                    DeleteAllowed = dialogAccess.DeleteAllowed.ToString(),
                    ModifiedBy = GlobalData.UserId == null ? "No_User" : GlobalData.UserId
                };

                bool authResponse = new AuthPlatnedPass().EditAccessControl(pass_Access_Control);
                if (authResponse)
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Success!", $"Operation Success for Function: {pass_Access_Control.AppFunction} for Role: {dialogAccess.UserRole}", InfoBarSeverity.Success);
                    }
                }
                else
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please check the details.", InfoBarSeverity.Warning);
                    }

                    dialogAccess = new DialogAccessControl()
                    {
                        AppFunction = dialogAccess.AppFunction,
                        AppFunctionDescription = dialogAccess.AppFunctionDescription,
                        UserRole = dialogAccess.UserRole,
                        ReadAllowed = dialogAccess.ReadAllowed,
                        CreateAllowed = dialogAccess.CreateAllowed,
                        UpdateAllowed = dialogAccess.UpdateAllowed,
                        DeleteAllowed = dialogAccess.DeleteAllowed,

                    };
                    var resultNew = await ShowAddAccessControlDialog(dialogAccess);
                    await HandleAddAccessControlDialogResultAsync(resultNew, dialogAccess);
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

        private async Task HandleDeleteAccessControlDialogResultAsync(ContentDialogResult result, DialogAccessControl dialogAccess)
        {
            if (result == ContentDialogResult.Primary)
            {
                Pass_Access_Control pass_Access_Control = new Pass_Access_Control
                {
                    AppFunction = dialogAccess.AppFunction,
                    UserRole = dialogAccess.UserRole
                };
                bool authResponse = new AuthPlatnedPass().DeleteAccessControl(pass_Access_Control);
                if (authResponse)
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Success!", $"Operation Success for Function: {pass_Access_Control.AppFunction} for Role: {dialogAccess.UserRole}", InfoBarSeverity.Success);
                    }
                }
                else
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please check the details.", InfoBarSeverity.Warning);
                    }

                    dialogAccess = new DialogAccessControl()
                    {
                        AppFunction = dialogAccess.AppFunction,
                        AppFunctionDescription = dialogAccess.AppFunctionDescription,
                        UserRole = dialogAccess.UserRole,
                        ReadAllowed = dialogAccess.ReadAllowed,
                        CreateAllowed = dialogAccess.CreateAllowed,
                        UpdateAllowed = dialogAccess.UpdateAllowed,
                        DeleteAllowed = dialogAccess.DeleteAllowed,

                    };
                    var resultNew = await ShowAddAccessControlDialog(dialogAccess);
                    await HandleAddAccessControlDialogResultAsync(resultNew, dialogAccess);
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


        #region Mahara-85 - Access Check

        private void AccessCheck()
        {
            if (AccessControl.IsGranted("BTN_NEW_ACCESS_CONTROL", "C"))
            { btnNewAccessRole.Visibility = Visibility.Visible; }
            else { btnNewAccessRole.Visibility = Visibility.Collapsed; }

            foreach (var user in GridItemsAccessRole)
            {
                user.CanEdit = AccessControl.IsGranted("BTN_EDIT_ACCESS_CONTROL", "U");
                user.CanDelete = AccessControl.IsGranted("BTN_DELETE_ACCESS_CONTROL ", "D");
            }
            dataGrid.ItemsSource = null; // Refresh binding
            dataGrid.ItemsSource = GridItemsAccessRole;
        }

        #endregion
    }

    /// <summary>
    /// Other Classes
    /// </summary>

    public class TreeNodeAccessRole
    {
        public string Name { get; set; }
        public ObservableCollection<TreeNodeAccessRole> Children { get; set; }

        public TreeNodeAccessRole(string name)
        {
            Name = name;
            Children = new ObservableCollection<TreeNodeAccessRole>();
        }
    }

    public class GridItemAccessRole
    {
        public bool CanDelete { get; internal set; }
        public bool CanEdit { get; internal set; }
        public ObservableCollection<TreeNode> TreeNodesAppFunction { get; set; }
        public ObservableCollection<TreeNode> TreeNodesAppFuncDescription { get; set; }
        public ObservableCollection<TreeNode> TreeNodesUserRole { get; set; }
        public ObservableCollection<TreeNode> TreeNodesReadAllowed { get; set; }
        public ObservableCollection<TreeNode> TreeNodesCreateAllowed { get; set; }
        public ObservableCollection<TreeNode> TreeNodesUpdateAllowed { get; set; }
        public ObservableCollection<TreeNode> TreeNodesDeleteAllowed { get; set; }
        public ObservableCollection<TreeNode> TreeNodesCreatedDate { get; set; }
        public ObservableCollection<TreeNode> TreeNodesCreatedBy { get; set; }
        public ObservableCollection<TreeNode> TreeNodesModifiedDate { get; set; }
        public ObservableCollection<TreeNode> TreeNodesModifiedBy { get; set; }

        public GridItemAccessRole(string appFunc, string funcDesc, string userRole, string r, string c, string u, string d, string createdDate, string createdBy, string modifiedDate, string modifiedBy)
        {
            TreeNodesAppFunction = new ObservableCollection<TreeNode>();
            TreeNodesAppFuncDescription = new ObservableCollection<TreeNode>();
            TreeNodesUserRole = new ObservableCollection<TreeNode>();
            TreeNodesReadAllowed = new ObservableCollection<TreeNode>();
            TreeNodesCreateAllowed = new ObservableCollection<TreeNode>();
            TreeNodesUpdateAllowed = new ObservableCollection<TreeNode>();
            TreeNodesDeleteAllowed = new ObservableCollection<TreeNode>();
            TreeNodesCreatedDate = new ObservableCollection<TreeNode>();
            TreeNodesCreatedBy = new ObservableCollection<TreeNode>();
            TreeNodesModifiedDate = new ObservableCollection<TreeNode>();
            TreeNodesModifiedBy = new ObservableCollection<TreeNode>();

            TreeNodesAppFunction.Add(new TreeNode(appFunc));
            TreeNodesAppFuncDescription.Add(new TreeNode(funcDesc));
            TreeNodesUserRole.Add(new TreeNode(userRole));
            TreeNodesReadAllowed.Add(new TreeNode(r));
            TreeNodesCreateAllowed.Add(new TreeNode(c));
            TreeNodesUpdateAllowed.Add(new TreeNode(u));
            TreeNodesDeleteAllowed.Add(new TreeNode(d));
            TreeNodesCreatedDate.Add(new TreeNode(createdDate));
            TreeNodesCreatedBy.Add(new TreeNode(createdBy));
            TreeNodesModifiedDate.Add(new TreeNode(modifiedDate));
            TreeNodesModifiedBy.Add(new TreeNode(modifiedBy));
        }
    }
}
