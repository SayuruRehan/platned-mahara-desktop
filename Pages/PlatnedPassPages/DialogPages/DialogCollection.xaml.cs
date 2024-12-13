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
    public sealed partial class DialogCollection : Page
    {
        bool _isEdit = false;
        // Mahara-95 - START
        bool _isShare = false;
        // Mahara-95 - END
        public DialogCollection()
        {
            this.InitializeComponent();
            txtCompanyId.IsEnabled = false;
            txtUserId.IsEnabled = false;
            txtCollectionId.IsEnabled = false;
        }

        public DialogCollection(bool isedit)
        {
            _isEdit = isedit;
            this.InitializeComponent();
            txtCompanyId.IsEnabled = false;
            txtUserId.IsEnabled = false;
            txtCollectionId.IsEnabled = false;
            txtCollectionName.IsEnabled = true;
        }

        // Mahara-95 - Addign a constructor for Share - START
        public DialogCollection(bool isedit, bool isShare)
        {
            _isEdit = isedit;
            _isShare = isShare;
            this.InitializeComponent();
            txtCompanyId.IsEnabled = false;
            txtUserId.IsEnabled = true;
            txtCollectionId.IsEnabled = false;
            txtCollectionName.IsEnabled = false;
        }
        // Mahara-95 - END

        public string CompanyId
        {
            get => txtCompanyId.Text;
            set => txtCompanyId.Text = value;
        }

        public string UserId
        {
            get => txtUserId.Text;
            set => txtUserId.Text = value;
        }

        public string CollectionId
        {
            get => txtCollectionId.Text;
            set => txtCollectionId.Text = value;
        }
        public string CollectionName
        {
            get => txtCollectionName.Text;
            set => txtCollectionName.Text = value;
        }
    }
}
