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

namespace PlatnedMahara.Pages.PlatnedPassPages.DialogPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DialogUser : Page
    {
        public DialogUser()
        {
            this.InitializeComponent();
        }

        public string UserCompanyId => txtCompanyId.Text;
        public string UserId => txtUserId.Text;
        public string UserName => txtUserName.Text;
        public DateTime ValidFrom => calValidFrom.Date.Value.DateTime;
        public string UserRole => cmbUserRole.SelectedItem.ToString();

    }
}
