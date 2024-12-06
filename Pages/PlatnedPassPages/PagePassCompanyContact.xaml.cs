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
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }

        public PagePassCompanyContact()
        {
            this.InitializeComponent();
            LoadData();
            dataGrid.ItemsSource = GridItemCompanyContact;
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the DataContext of the clicked row
            var button = sender as Button;

            if (button?.DataContext is GridItemCompanyContact selectedItem)
            {
                var dialogCompanyContact = new DialogCompanyContact(true)
                {
                    CompanyContactID = selectedItem.TreeNodesCompanyId[0].Name,
                    CompanyContactUserID = selectedItem.TreeNodesUserId[0].Name,
                    CompanyContactEmail = selectedItem.TreeNodesCompanyContactEmail[0].Name,
                    CompanyContactNumber = selectedItem.TreeNodesCompanyContactNumber[0].Name,
                    CompanyContactTitle = selectedItem.TreeNodesCompanyContactTitle[0].Name
                };

                //result dialog Pass Company Contact
                var resutlDialogPCC =  await ShowAddCompanyContactDialog(dialogCompanyContact);
                if(resutlDialogPCC == ContentDialogResult.Primary)
                {
                    Pass_Company_Contact pass_Company_Contact = new Pass_Company_Contact();
                    pass_Company_Contact.CompanyID = dialogCompanyContact.CompanyContactID;
                    pass_Company_Contact.UserID = dialogCompanyContact.CompanyContactUserID;
                    pass_Company_Contact.ContactEmail = dialogCompanyContact.CompanyContactEmail;
                    pass_Company_Contact.ContactNumber = dialogCompanyContact.CompanyContactNumber;
                    pass_Company_Contact.ContactTitle = dialogCompanyContact.CompanyContactTitle;

                    bool isUpdated = new AuthPlatnedPass().EditPassCompanyContact(pass_Company_Contact);
                    if(isUpdated)
                    {
                        selectedItem.TreeNodesCompanyId[0].Name = dialogCompanyContact.CompanyContactID;
                        selectedItem.TreeNodesUserId[0].Name = dialogCompanyContact.CompanyContactUserID;
                        selectedItem.TreeNodesCompanyContactEmail[0].Name = dialogCompanyContact.CompanyContactEmail;
                        selectedItem.TreeNodesCompanyContactNumber[0].Name = dialogCompanyContact.CompanyContactNumber;
                        selectedItem.TreeNodesCompanyContactTitle[0].Name = dialogCompanyContact.CompanyContactTitle;

                        // Refresh DataGrid
                        LoadData();
                        dataGrid.ItemsSource = null;
                        dataGrid.ItemsSource = GridItemCompanyContact;

                        // Show success message
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Success!", $"Company Contact updated successfully.", InfoBarSeverity.Success);
                        }
                    }
                    else
                    {
                        // Show error message
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Error", $"Failed to update company contact.", InfoBarSeverity.Error);
                        }
                    }
                    
                }
            }


        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the DataContext of the clicked row
            var button = sender as Button;

            if (button?.DataContext is GridItemCompanyContact selectedItem)
            {
                // Confirm delete action
                var dialogCompanyContactDelete = new ContentDialog
                {
                    Title = "Delete Confirmation",
                    Content = $"Are you sure you want to delete user {selectedItem.TreeNodesUserId[0].Name}?",
                    PrimaryButtonText = "Yes",
                    CloseButtonText = "No",
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = PagePassCompanyContactXamlRoot.XamlRoot
                };

                var resultCompanyContactDel =  await dialogCompanyContactDelete.ShowAsync();

                if (resultCompanyContactDel == ContentDialogResult.Primary)
                {
                    
                    Pass_Company_Contact pass_Company_Contact = new Pass_Company_Contact
                    {
                        CompanyID = selectedItem.TreeNodesCompanyId[0].Name,
                        UserID = selectedItem.TreeNodesUserId[0].Name,
                        ContactEmail = selectedItem.TreeNodesCompanyContactEmail[0].Name,
                        ContactNumber = selectedItem.TreeNodesCompanyContactNumber[0].Name,
                        ContactTitle = selectedItem.TreeNodesCompanyContactTitle[0].Name,
                    };

                    // Call your delete method
                    bool isDeleted = new AuthPlatnedPass().DeletePassCompanyContact(pass_Company_Contact);

                    if (isDeleted)
                    {
                        // Remove the item from the ObservableCollection
                        GridItemCompanyContact.Remove(selectedItem);

                        // Show success message
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Success!", $"Company Contact deleted successfully.", InfoBarSeverity.Success);
                        }
                    }
                    else
                    {
                        // Show error message
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Error", $"Failed to delete company contact.", InfoBarSeverity.Error);
                        }
                    }
                }
            }
        }

        private void LoadData()
        {
            List<Pass_Company_Contact> pass_Company_Contacts = new List<Pass_Company_Contact>();
            pass_Company_Contacts = new AuthPlatnedPass().GetPass_Company_Contacts();
            GridItemCompanyContact = new ObservableCollection<GridItemCompanyContact>();
            if (pass_Company_Contacts != null && pass_Company_Contacts.Count > 0)
            {
                foreach( Pass_Company_Contact pcc in pass_Company_Contacts )
                {
                    // Pass values needed to display in grid
                    var item = new GridItemCompanyContact($"CompanyID", "Userid", "CompanyContactTitle", "CompanyContactNumber", "companyContactEmail")
                    {
                        // Initialize with default values
                        TreeNodesCompanyId = new ObservableCollection<TreeNode> { new TreeNode(pcc.CompanyID) },
                        TreeNodesUserId = new ObservableCollection<TreeNode> { new TreeNode(pcc.UserID) },
                        TreeNodesCompanyContactTitle = new ObservableCollection<TreeNode> { new TreeNode(pcc.ContactTitle) },
                        TreeNodesCompanyContactNumber = new ObservableCollection<TreeNode> { new TreeNode(pcc.ContactNumber) },
                        TreeNodesCompanyContactEmail = new ObservableCollection<TreeNode> { new TreeNode(pcc.ContactEmail) }
                    };

                    GridItemCompanyContact.Add(item);
                }
            }
        }

          
        private async void btnNewCompanyContact_Click(object sender, RoutedEventArgs e)
        {
            var pageaddcompanycontact = new DialogCompanyContact();
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
                LoadData();
                dataGrid.ItemsSource = null; // Clear the existing binding
                dataGrid.ItemsSource = GridItemCompanyContact;
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

                //null validation check of the company contact
                if (dialogCompanyContact.CompanyContactID == "" || dialogCompanyContact.CompanyContactUserID == "" || dialogCompanyContact.CompanyContactTitle == "" || dialogCompanyContact.CompanyContactNumber == "" || dialogCompanyContact.CompanyContactEmail == "")
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please check the details.", InfoBarSeverity.Warning);

                    }
                    var CompanyContactdialogPage = new DialogCompanyContact(false)
                    {
                        CompanyContactID = dialogCompanyContact.CompanyContactID,
                        CompanyContactUserID = dialogCompanyContact.CompanyContactUserID,
                        CompanyContactEmail = dialogCompanyContact.CompanyContactEmail,
                        CompanyContactNumber = dialogCompanyContact.CompanyContactNumber,
                        CompanyContactTitle = dialogCompanyContact.CompanyContactTitle
                    };
                    var resultNew = await ShowAddCompanyContactDialog(CompanyContactdialogPage);
                    await HandleAddCompanyContactDialogResultAsync(resultNew, CompanyContactdialogPage);
                }
                else {
                Pass_Company_Contact pass_Company_Contact = new Pass_Company_Contact
                {
                    CompanyID = dialogCompanyContact.CompanyContactID,
                    UserID = dialogCompanyContact.CompanyContactUserID,
                    ContactEmail = dialogCompanyContact.CompanyContactEmail,
                    ContactNumber = dialogCompanyContact.CompanyContactNumber,
                    ContactTitle = dialogCompanyContact.CompanyContactTitle
                };

                bool authResponse = new AuthPlatnedPass().CreateNewPassCompanyContact(pass_Company_Contact);
                if (authResponse)
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Success!", $"Operation Success for Company: {pass_Company_Contact.CompanyID}", InfoBarSeverity.Success);
                    }
                }
                else
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", $"Operation Unsuccessful! Please check the details.", InfoBarSeverity.Warning);
                    }

                    dialogCompanyContact = new DialogCompanyContact();
                    var resultCompanyContact = await ShowAddCompanyContactDialog(dialogCompanyContact);
                    await HandleAddCompanyContactDialogResultAsync(resultCompanyContact, dialogCompanyContact);

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
        public bool CanDelete { get; internal set; }
        public bool CanEdit { get; internal set; }
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
