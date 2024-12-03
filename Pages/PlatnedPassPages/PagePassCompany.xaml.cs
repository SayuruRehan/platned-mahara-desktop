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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PlatnedMahara.Pages.PlatnedPassPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PagePassCompany : Microsoft.UI.Xaml.Controls.Page
    {
        public ObservableCollection<GridItemCompany> GridItemsCompany { get; set; }

        public PagePassCompany()
        {
            this.InitializeComponent();
            LoadData();
            dataGrid.ItemsSource = GridItemsCompany;
        }

        private void LoadData()
        {
            List<Pass_Company> pass_Companies = new List<Pass_Company>();
            pass_Companies = new AuthPlatnedPass().GetPass_Companies();
            GridItemsCompany = new ObservableCollection<GridItemCompany>();

            if (pass_Companies != null && pass_Companies.Count > 0)
            {
                foreach (Pass_Company pc in pass_Companies)
                {
                    var item = new GridItemCompany($"CompanyID", "CompanyName", "CompanyAddress", "LicenseLimit", "LiceseConsumed", "CompanyType", "RowState", "CreDate", "CreBy", "ModDate", "ModBy")
                    {
                        // Initialize with default values
                        TreeNodesCompanyId = new ObservableCollection<TreeNode> { new TreeNode(pc.CompanyID) },
                        TreeNodesCompanyName = new ObservableCollection<TreeNode> { new TreeNode(pc.CompanyName) },
                        TreeNodesCompanyAddress = new ObservableCollection<TreeNode> { new TreeNode(pc.CompanyAddress) },
                        TreeNodesLicenseLimit = new ObservableCollection<TreeNode> { new TreeNode(pc.LicenseLimit.ToString()) },
                        TreeNodesLiceseConsumed = new ObservableCollection<TreeNode> { new TreeNode(pc.LicenseConsumed.ToString()) },
                        TreeNodesCompanyType = new ObservableCollection<TreeNode> { new TreeNode(pc.CompanyType) },
                        TreeNodesRowState = new ObservableCollection<TreeNode> { new TreeNode(pc.RowState) },
                        TreeNodesCreatedDate = new ObservableCollection<TreeNode> { new TreeNode(pc.CreatedDate.ToString()) },
                        TreeNodesCreatedBy = new ObservableCollection<TreeNode> { new TreeNode(pc.CreatedBy) },
                        TreeNodesModifiedDate = new ObservableCollection<TreeNode> { new TreeNode(pc.ModifiedDate.ToString()) },
                        TreeNodesModifiedBy = new ObservableCollection<TreeNode> { new TreeNode(pc.ModifiedBy) }
                    };

                    GridItemsCompany.Add(item);
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
                    if (PagePassCompanyXamlRoot.XamlRoot == null)
                    {
                        PagePassCompanyXamlRoot.Loaded += async (s, e) =>
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
                        dataGrid.ItemsSource = GridItemsCompany;
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
                    if (PagePassCompanyXamlRoot.XamlRoot == null)
                    {
                        PagePassCompanyXamlRoot.Loaded += async (s, e) =>
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
                        dataGrid.ItemsSource = GridItemsCompany;
                    }
                }
            }
        }        

        private async void btnNewCompany_Click(object sender, RoutedEventArgs e)
        {
            var result = ContentDialogResult.None;
            var dialogCompany = new DialogCompany(false); // Create dialogCompany instance once

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
                LoadData();
                dataGrid.ItemsSource = null; // Clear the existing binding
                dataGrid.ItemsSource = GridItemsCompany;
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
                Pass_Company pass_Company = new Pass_Company
                {
                    CompanyID = dialogCompany.CompanyId,
                    CompanyName = dialogCompany.CompanyName,
                    CompanyType = dialogCompany.CompanyType,
                    CompanyAddress = dialogCompany.CompanyAddress,
                    LicenseLimit = Convert.ToInt32(dialogCompany.LicenseLimit),
                    RowState = "ACTIVE",
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

                    //var resultNew = ContentDialogResult.None;
                    //resultNew = await ShowAddCompanyDialog(dialogCompany);
                    //await HandleAddCompanyDialogResultAsync(resultNew, dialogCompany);

                    var dailogcompanypage = new DialogCompany();
                    dailogcompanypage = dialogCompany;
                    var resultNew = await ShowAddCompanyDialog(dailogcompanypage);
                    await HandleAddCompanyDialogResultAsync(resultNew, dailogcompanypage);
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
                    RowState = "ACTIVE",
                    ModifiedBy = GlobalData.UserId == null ? "User1" : GlobalData.UserId
                };
                // Access field data from dialogCompany
                //string companyId = dialogCompany.CompanyId;
                //string companyName = dialogCompany.CompanyName;
                //
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

                    //var resultNew = ContentDialogResult.None;
                    //resultNew = await ShowAddCompanyDialog(dialogCompany);
                    //await HandleAddCompanyDialogResultAsync(resultNew, dialogCompany);

                    //var dailogcompanypage = new DialogCompany();
                    //dailogcompanypage = dialogCompany;
                    //var resultNew = await ShowAddCompanyDialog(dailogcompanypage);
                    //await HandleEditCompanyDialogResultAsync(resultNew, dailogcompanypage);
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
                    RowState = "INACTIVE",
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

                    //var dailogcompanypage = new DialogCompany();
                    //dailogcompanypage = dialogCompany;
                    //var resultNew = await ShowAddCompanyDialog(dailogcompanypage);
                    //await HandleDeleteCompanyDialogResultAsync(resultNew, dailogcompanypage);
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

    public class TreeNodeCompany
    {
        public string Name { get; set; }
        public ObservableCollection<TreeNodeCompany> Children { get; set; }

        public TreeNodeCompany(string name)
        {
            Name = name;
            Children = new ObservableCollection<TreeNodeCompany>();
        }
    }

    public class GridItemCompany
    {
        public ObservableCollection<TreeNode> TreeNodesCompanyId { get; set; }
        public ObservableCollection<TreeNode> TreeNodesCompanyName { get; set; }
        public ObservableCollection<TreeNode> TreeNodesCompanyAddress { get; set; }
        public ObservableCollection<TreeNode> TreeNodesLicenseLimit { get; set; }
        public ObservableCollection<TreeNode> TreeNodesLiceseConsumed { get; set; }
        public ObservableCollection<TreeNode> TreeNodesCompanyType { get; set; }
        public ObservableCollection<TreeNode> TreeNodesRowState { get; set; }
        public ObservableCollection<TreeNode> TreeNodesCreatedDate { get; set; }
        public ObservableCollection<TreeNode> TreeNodesCreatedBy { get; set; }
        public ObservableCollection<TreeNode> TreeNodesModifiedDate { get; set; }
        public ObservableCollection<TreeNode> TreeNodesModifiedBy { get; set; }

        public GridItemCompany(string companyId, string companyName, string companyAddress, string licenseLimit, string licenseConsumed, string companyType, string rowState, string createdDate, string createdBy, string modifiedDate, string modifiedBy)
        {
            TreeNodesCompanyId = new ObservableCollection<TreeNode>();
            TreeNodesCompanyName = new ObservableCollection<TreeNode>();
            TreeNodesCompanyAddress = new ObservableCollection<TreeNode>();
            TreeNodesLicenseLimit = new ObservableCollection<TreeNode>();
            TreeNodesLiceseConsumed = new ObservableCollection<TreeNode>();
            TreeNodesCompanyType = new ObservableCollection<TreeNode>();
            TreeNodesRowState = new ObservableCollection<TreeNode>();
            TreeNodesCreatedDate = new ObservableCollection<TreeNode>();
            TreeNodesCreatedBy = new ObservableCollection<TreeNode>();
            TreeNodesModifiedDate = new ObservableCollection<TreeNode>();
            TreeNodesModifiedBy = new ObservableCollection<TreeNode>();

            TreeNodesCompanyId.Add(new TreeNode(companyId));
            TreeNodesCompanyName.Add(new TreeNode(companyName));
            TreeNodesCompanyAddress.Add(new TreeNode(companyAddress));
            TreeNodesLicenseLimit.Add(new TreeNode(licenseLimit));
            TreeNodesLiceseConsumed.Add(new TreeNode(licenseConsumed));
            TreeNodesCompanyType.Add(new TreeNode(companyType));
            TreeNodesRowState.Add(new TreeNode(rowState));
            TreeNodesCreatedDate.Add(new TreeNode(createdDate));
            TreeNodesCreatedBy.Add(new TreeNode(createdBy));
            TreeNodesModifiedDate.Add(new TreeNode(modifiedDate));
            TreeNodesModifiedBy.Add(new TreeNode(modifiedBy));
        }
    }

}
