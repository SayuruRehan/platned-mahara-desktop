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
    public sealed partial class PagePassCompanyContact : Microsoft.UI.Xaml.Controls.Page
    {
        public ObservableCollection<GridItemCompanyContact> GridItemCompanyContact { get; set; }
        public PagePassCompanyContact()
        {
            this.InitializeComponent();
            LoadData();
            dataGrid.ItemsSource = GridItemCompanyContact;
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
            GridItemCompanyContact = new ObservableCollection<GridItemCompanyContact>();
            for (int gi = 1; gi <= 1; gi++)
            {
                // Pass values needed to display in grid
                var item = new GridItemCompanyContact($"CompanyID", "Userid", "CompanyContactTitle", "CompanyContactNumber", "companyContactEmail")
                {
                    // Initialize with default values
                    TreeNodesCompanyId = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesUserId = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesCompanyContactTitle = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesCompanyContactNumber = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesCompanyContactEmail = new ObservableCollection<TreeNode> { new TreeNode("") }
                };

                GridItemCompanyContact.Add(item);
            }
        }

          
        private async void btnNewCompanyContact_Click(object sender, RoutedEventArgs e)
        {
            var pageaddcompanycontact = new DialogCompanyContact();
            //var result = ContentDialogResult.None;
            var resultDialogCompanyContactContent = await ShowAddCompanyContactDialog(pageaddcompanycontact);

            // Ensure PagePassUserManagementXamlRoot is loaded
            if (PagePassCompanyContactXamlRoot.XamlRoot == null)
            {
                PagePassCompanyContactXamlRoot.Loaded += async (s, e) =>
                {
                    //result = await ShowAddCompanyDialog(dialogCompanyContent);
                    await HandleAddCompanyContactDialogResultAsync(resultDialogCompanyContactContent, pageaddcompanycontact);
                };
            }
            else
            {
                //result = await ShowAddCompanyDialog(dialogCompanyContent);
                await HandleAddCompanyContactDialogResultAsync(resultDialogCompanyContactContent, pageaddcompanycontact);
            }
        }


        private async Task<ContentDialogResult> ShowAddCompanyContactDialog(DialogCompanyContact dialogCompanyContact)
        {
            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = PagePassCompanyContactXamlRoot.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                PrimaryButtonText = "Process",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = dialogCompanyContact // Set the same dialogCompany instance as the content
            };

            return await dialog.ShowAsync(); // Return the result of the dialog
        }


        private async Task HandleAddCompanyContactDialogResultAsync(ContentDialogResult result, DialogCompanyContact dialogCompanyContact)
        {
            if (result == ContentDialogResult.Primary)
            {
                // Access field data from dialogCompany
                string companyContactId = dialogCompanyContact.CompanyContactID;
                string companyCompanyContactUserID = dialogCompanyContact.CompanyContactUserID;
                string companyCompanyContactTitle = dialogCompanyContact.CompanyContactTitle;
                string companyCompanyContactNumber = dialogCompanyContact.CompanyContactNumber;
                string companyCompanyContactEmail = dialogCompanyContact.CompanyContactEmail;

                //null validation check of the company contact
                if (companyContactId == "" || companyCompanyContactUserID == "" || companyCompanyContactTitle == "" || companyCompanyContactNumber == "" || companyCompanyContactEmail == "")
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please ensure no fields are left empty..", InfoBarSeverity.Warning);

                    }

                   var CompanyContactdialogPage = new DialogCompanyContact();
                    var resultNew = await ShowAddCompanyContactDialog(CompanyContactdialogPage);
                   // await HandleAddCompanyContactDialogResultAsync(resultNew, CompanyContactdialogPage);
                }
                else {

                    bool authResponse = await AuthPlatnedPass.validateLogin(companyContactId, companyCompanyContactNumber);
                    if (authResponse)
                    {
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Success!", $"Operation Success for Company: {companyContactId}", InfoBarSeverity.Success);
                        }
                    }
                    else
                    {
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please check the details.", InfoBarSeverity.Warning);
                        }

                        var dialogPage = new DialogCompanyContact();
                        var resultNew = await ShowAddCompanyContactDialog(dialogPage);
                        await HandleAddCompanyContactDialogResultAsync(resultNew, dialogPage);

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

    }

    public class TreeNodeCompanyContact
    {
        public string Name { get; set; }
        public ObservableCollection<TreeNodeCompanyContact> Children { get; set; }

        public TreeNodeCompanyContact(string name)
        {
            Name = name;
            Children = new ObservableCollection<TreeNodeCompanyContact>();
        }
    }


    public class GridItemCompanyContact
    {
        public ObservableCollection<TreeNode> TreeNodesCompanyId { get; set; }
        public ObservableCollection<TreeNode> TreeNodesUserId { get; set; }
        public ObservableCollection<TreeNode> TreeNodesCompanyContactTitle { get; set; }
        public ObservableCollection<TreeNode> TreeNodesCompanyContactNumber { get; set; }
        public ObservableCollection<TreeNode> TreeNodesCompanyContactEmail { get; set; }

        public GridItemCompanyContact(string companyId, string userid, string companyContactTitle, string companyContactNumber, string companyContactEmail)
        {
            TreeNodesCompanyId = new ObservableCollection<TreeNode>();
            TreeNodesUserId = new ObservableCollection<TreeNode>();
            TreeNodesCompanyContactTitle = new ObservableCollection<TreeNode>();
            TreeNodesCompanyContactNumber = new ObservableCollection<TreeNode>();
            TreeNodesCompanyContactEmail = new ObservableCollection<TreeNode>();

            TreeNodesCompanyId.Add(new TreeNode(companyId));
            TreeNodesUserId.Add(new TreeNode(userid));
            TreeNodesCompanyContactTitle.Add(new TreeNode(companyContactTitle));
            TreeNodesCompanyContactNumber.Add(new TreeNode(companyContactNumber));
            TreeNodesCompanyContactEmail.Add(new TreeNode(companyContactEmail));
        }
    }
}
