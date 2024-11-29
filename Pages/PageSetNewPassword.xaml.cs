using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PlatnedMahara.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageSetNewPassword : Page
    {
        private string companyIdVar;
        private string userIdVar;
        private string userEmailVar;

        public PageSetNewPassword()
        {
            this.InitializeComponent();
        }

        public PageSetNewPassword(string companyId, string userId, string userEmail)
        {
            this.InitializeComponent();

            this.companyIdVar = companyId;
            this.userIdVar = userId;
            this.userEmailVar = userEmail;

            txtResetCompanyId.Text = companyId;
            txtResetUserId.Text = userId;
            txtResetEmail.Text = userEmail;

        }

        public string companyId => this.companyIdVar;
        public string userId => this.userIdVar;
        public string userEmail => this.userEmailVar;
        public string newPassword => txtNewPassword.Password;
        public string confirmPassword => txtConfirmPassword.Password;

        
    }
}
