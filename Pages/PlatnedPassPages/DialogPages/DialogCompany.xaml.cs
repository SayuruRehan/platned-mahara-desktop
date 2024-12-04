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

    
    public sealed partial class DialogCompany : Page
    {
        bool _isEdit = false;
        public DialogCompany()
        {
            this.InitializeComponent();
        }

        public DialogCompany(bool isedit)
        {
            _isEdit = isedit;
            this.InitializeComponent();
            if(_isEdit)
            {
                txtCompanyId.IsEnabled = false;                
            }
        }

        //public string CompanyId => txtCompanyId.Text;
        //public string CompanyName => txtCompanyName.Text;
        //public string CompanyAddress => txtCompanyAddress.Text;
        //public string LicenseLimit => txtLicenseLimit.Text;
        //public string CompanyType => cmbCompanyType.SelectedItem.ToString();

        public string CompanyId
        {
            get => txtCompanyId.Text;
            set => txtCompanyId.Text = value;
        }

        public string CompanyName
        {
            get => txtCompanyName.Text;
            set => txtCompanyName.Text = value;
        }

        public string CompanyAddress
        {
            get => txtCompanyAddress.Text;
            set => txtCompanyAddress.Text = value;
        }
        public string LicenseLimit
        {
            get => txtLicenseLimit.Text;
            set => txtLicenseLimit.Text = value;
        }
        public string CompanyType
        {
            get => cmbCompanyType.SelectedItem.ToString();
            set => cmbCompanyType.SelectedItem = value;
        }

        private void txtLicenseLimit_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            // Restrict input to numeric characters
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }
    }
}
