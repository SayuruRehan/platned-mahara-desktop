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
    public sealed partial class PasePassAccessControl : Microsoft.UI.Xaml.Controls.Page
    {
        public ObservableCollection<GridItemAccessRole> GridItemsAccessRole { get; set; }

        public PasePassAccessControl()
        {
            this.InitializeComponent();
            LoadData();
            dataGrid.ItemsSource = GridItemsAccessRole;
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
                        TreeNodesUserRole = new ObservableCollection<TreeNode> { new TreeNode(pa.UserRole) },
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
                if (button?.DataContext is GridItemCompany selectedItem)
                {
                    var dailogcompany = new DialogCompany(true)
                    {
                        CompanyId = selectedItem.TreeNodesCompanyId[0].Name,
                        CompanyName = selectedItem.TreeNodesCompanyName[0].Name,
                        CompanyAddress = selectedItem.TreeNodesCompanyAddress[0].Name,
                        LicenseLimit = selectedItem.TreeNodesLicenseLimit[0].Name,
                        CompanyType = selectedItem.TreeNodesCompanyType[0].Name,
                    };
                    if (PagePassAccessRoleXamlRoot.XamlRoot == null)
                    {
                        PagePassAccessRoleXamlRoot.Loaded += async (s, e) =>
                        {
                            result = await ShowAddCompanyDialog(dailogcompany);
                            await HandleEditCompanyDialogResultAsync(result, dailogcompany);
                        };
                    }
                    else
                    {
                        result = await ShowAddCompanyDialog(dailogcompany);
                        await HandleEditCompanyDialogResultAsync(result, dailogcompany);
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
                if (button?.DataContext is GridItemCompany selectedItem)
                {
                    var dailogcompany = new DialogCompany(true)
                    {
                        CompanyId = selectedItem.TreeNodesCompanyId[0].Name,
                        CompanyName = selectedItem.TreeNodesCompanyName[0].Name,
                        CompanyAddress = selectedItem.TreeNodesCompanyAddress[0].Name,
                        LicenseLimit = selectedItem.TreeNodesLicenseLimit[0].Name,
                        CompanyType = selectedItem.TreeNodesCompanyType[0].Name,
                    };
                    if (PagePassAccessRoleXamlRoot.XamlRoot == null)
                    {
                        PagePassAccessRoleXamlRoot.Loaded += async (s, e) =>
                        {
                            result = await ShowAddCompanyDialog(dailogcompany);
                            await HandleDeleteCompanyDialogResultAsync(result, dailogcompany);
                        };
                    }
                    else
                    {
                        result = await ShowAddCompanyDialog(dailogcompany);
                        await HandleDeleteCompanyDialogResultAsync(result, dailogcompany);
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
            var dialogCompany = new DialogCompany(false); // Create dialogCompany instance once

            // Ensure PagePassUserManagementXamlRoot is loaded
            if (PagePassAccessRoleXamlRoot.XamlRoot == null)
            {
                PagePassAccessRoleXamlRoot.Loaded += async (s, e) =>
                {
                    result = await ShowAddCompanyDialog(dialogCompany);
                    await HandleAddCompanyDialogResultAsync(result, dialogCompany);
                };
            }
            else
            {
                result = await ShowAddCompanyDialog(dialogCompany);
                await HandleAddCompanyDialogResultAsync(result, dialogCompany);
                LoadData();
                dataGrid.ItemsSource = null; // Clear the existing binding
                dataGrid.ItemsSource = GridItemsAccessRole;
            }
        }

        private async Task<ContentDialogResult> ShowAddCompanyDialog(DialogCompany dialogCompany)
        {
            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = PagePassAccessRoleXamlRoot.XamlRoot,
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
                //Null validation
                if (dialogCompany.CompanyId == "" || dialogCompany.CompanyName == "" || dialogCompany.CompanyAddress == "" || dialogCompany.LicenseLimit == "" || dialogCompany.CompanyType == "")
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please check the details.", InfoBarSeverity.Warning);
                    }
                    //Create Dialog content with field value
                    var CompanyDialogPage = new DialogCompany()
                    {
                        CompanyId = dialogCompany.CompanyId,
                        CompanyName = dialogCompany.CompanyName,
                        CompanyAddress = dialogCompany.CompanyAddress,
                        LicenseLimit = dialogCompany.LicenseLimit,
                        CompanyType = dialogCompany.CompanyType,

                    };

                    //refill value that added
                    var resultNew = await ShowAddCompanyDialog(CompanyDialogPage);
                    await HandleAddCompanyDialogResultAsync(resultNew, CompanyDialogPage);
                }
                else
                {
                    Pass_Company pass_Company = new Pass_Company
                    {
                        CompanyID = dialogCompany.CompanyId,
                        CompanyName = dialogCompany.CompanyName,
                        CompanyType = dialogCompany.CompanyType,
                        CompanyAddress = dialogCompany.CompanyAddress,
                        LicenseLimit = Convert.ToInt32(dialogCompany.LicenseLimit),
                        RowState = "Active",
                        CreatedBy = GlobalData.UserId
                    };
                    bool authResponse = new AuthPlatnedPass().CreateNewCompany(pass_Company);
                    if (authResponse)
                    {
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Success!", $"Operation Success for Company: {pass_Company.CompanyID}", InfoBarSeverity.Success);
                        }
                    }
                    else
                    {
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please check the details.", InfoBarSeverity.Warning);
                        }

                        dialogCompany = new DialogCompany()
                        {
                            CompanyId = pass_Company.CompanyID,
                            CompanyName = pass_Company.CompanyName,
                            CompanyAddress = pass_Company.CompanyAddress,
                            LicenseLimit = Convert.ToString(pass_Company.LicenseLimit),
                            CompanyType = pass_Company.CompanyType,
                        };
                        var resultNew = await ShowAddCompanyDialog(dialogCompany);
                        await HandleAddCompanyDialogResultAsync(resultNew, dialogCompany);
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

        private async Task HandleEditCompanyDialogResultAsync(ContentDialogResult result, DialogCompany dialogCompany)
        {
            if (result == ContentDialogResult.Primary)
            {
                Pass_Company pass_Company = new Pass_Company
                {
                    CompanyID = dialogCompany.CompanyId,
                    CompanyName = dialogCompany.CompanyName,
                    CompanyType = dialogCompany.CompanyType,
                    CompanyAddress = dialogCompany.CompanyAddress,
                    LicenseLimit = Convert.ToInt32(dialogCompany.LicenseLimit),
                    RowState = "Active",
                    ModifiedBy = GlobalData.UserId == null ? "No_User" : GlobalData.UserId
                };
                
                bool authResponse = new AuthPlatnedPass().EditCompany(pass_Company);
                if (authResponse)
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Success!", $"Operation Success for Company: {pass_Company.CompanyID}", InfoBarSeverity.Success);
                    }
                }
                else
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please check the details.", InfoBarSeverity.Warning);
                    }

                    dialogCompany = new DialogCompany(true)
                    {
                        CompanyId = pass_Company.CompanyID,
                        CompanyName = pass_Company.CompanyName,
                        CompanyAddress = pass_Company.CompanyAddress,
                        LicenseLimit = Convert.ToString(pass_Company.LicenseLimit),
                        CompanyType = pass_Company.CompanyType,
                    };
                    var resultNew = await ShowAddCompanyDialog(dialogCompany);
                    await HandleEditCompanyDialogResultAsync(resultNew, dialogCompany);
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

        private async Task HandleDeleteCompanyDialogResultAsync(ContentDialogResult result, DialogCompany dialogCompany)
        {
            if (result == ContentDialogResult.Primary)
            {
                Pass_Company pass_Company = new Pass_Company
                {
                    CompanyID = dialogCompany.CompanyId,
                    CompanyName = dialogCompany.CompanyName,
                    CompanyType = dialogCompany.CompanyType,
                    CompanyAddress = dialogCompany.CompanyAddress,
                    LicenseLimit = Convert.ToInt32(dialogCompany.LicenseLimit),
                    RowState = "Inactive",
                    CreatedBy = GlobalData.UserId
                };
                // Access field data from dialogCompany
                //string companyId = dialogCompany.CompanyId;
                //string companyName = dialogCompany.CompanyName;
                //
                bool authResponse = new AuthPlatnedPass().DeleteCompany(pass_Company);
                if (authResponse)
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Success!", $"Operation Success for Company: {pass_Company.CompanyID}", InfoBarSeverity.Success);
                    }
                }
                else
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please check the details.", InfoBarSeverity.Warning);
                    }

                    dialogCompany = new DialogCompany(true)
                    {
                        CompanyId = pass_Company.CompanyID,
                        CompanyName = pass_Company.CompanyName,
                        CompanyAddress = pass_Company.CompanyAddress,
                        LicenseLimit = Convert.ToString(pass_Company.LicenseLimit),
                        CompanyType = pass_Company.CompanyType,
                    };
                    var resultNew = await ShowAddCompanyDialog(dialogCompany);
                    await HandleDeleteCompanyDialogResultAsync(resultNew, dialogCompany);
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
