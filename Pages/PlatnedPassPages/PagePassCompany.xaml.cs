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
            GridItemsCompany = new ObservableCollection<GridItemCompany>();
            for (int gi = 1; gi <= 1; gi++)
            {
                // Pass values needed to display in grid
                var item = new GridItemCompany($"CompanyID", "CompanyName", "CompanyAddress", "LicenseLimit", "LiceseConsumed", "CompanyType", "RowState", "CreDate", "CreBy", "ModDate", "ModBy")
                {
                    // Initialize with default values
                    TreeNodesCompanyId = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesCompanyName = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesCompanyAddress = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesLicenseLimit = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesLiceseConsumed = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesCompanyType = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesRowState = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesCreatedDate = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesCreatedBy = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesModifiedDate = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesModifiedBy = new ObservableCollection<TreeNode> { new TreeNode("") }
                };

                GridItemsCompany.Add(item);
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
                string companyId = dialogCompany.CompanyId;
                string companyName = dialogCompany.CompanyName;

                bool authResponse = await AuthPlatnedPass.validateLogin(companyId, companyName);
                if (authResponse)
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Success!", $"Operation Success for Company: {companyId}", InfoBarSeverity.Success);
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
