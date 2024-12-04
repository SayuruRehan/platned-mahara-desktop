using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PlatnedMahara.Pages.PlatnedPassPages.DialogPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DialogCompanyContact : Page
    {
        bool _isEdit = false;
        public DialogCompanyContact()
        {
            this.InitializeComponent();

            if (_isEdit)
            {
                txtCompContactId.IsEnabled = false;
                txtCompanyContactUserID.IsEnabled = false;
            }
            else
            {
                txtCompContactId.IsEnabled = true;
                txtCompanyContactUserID.IsEnabled = true;
            }
        }

        public DialogCompanyContact(bool isedit)
        {
            _isEdit = isedit;
            this.InitializeComponent();

            if (_isEdit)
            {
                txtCompContactId.IsEnabled = false;
                txtCompanyContactUserID.IsEnabled = false;
            }
            else
            {
                txtCompContactId.IsEnabled = true;
                txtCompanyContactUserID.IsEnabled = true;
            }

        }



        public string CompanyContactID
        {
            get => txtCompContactId.Text;
            set => txtCompContactId.Text = value;
        }
        public string CompanyContactUserID
        {
            get => txtCompanyContactUserID.Text;
            set => txtCompanyContactUserID.Text = value;
        }
        public string CompanyContactTitle
        {
            get => txtCompanyContactTitle.Text;
            set => txtCompanyContactTitle.Text = value;
        }
        public string CompanyContactNumber
        {
            get => txtCompanyContactNumber.Text;
            set => txtCompanyContactNumber.Text = value;
        }
        public string CompanyContactEmail
        {
            get => txtCompanyContactEmail.Text;
            set => txtCompanyContactEmail.Text = value;
        }
    }
}
