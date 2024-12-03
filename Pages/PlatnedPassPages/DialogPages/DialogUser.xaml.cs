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
        bool _isEdit = false;

        public DialogUser()
        {
            this.InitializeComponent();

            if (_isEdit)
            {
                txtCompanyId.IsEnabled = false;
                txtUserId.IsEnabled = false;
                calValidTo.Visibility = Visibility.Visible;
                cmbUserState.Visibility = Visibility.Visible;
            }
            else
            {
                txtCompanyId.IsEnabled = true;
                txtUserId.IsEnabled = true;
                calValidTo.Visibility = Visibility.Collapsed;
                cmbUserState.Visibility = Visibility.Collapsed;
            }
        }

        public DialogUser(bool isedit)
        {
            _isEdit = isedit;
            this.InitializeComponent();

            if (_isEdit)
            {
                txtCompanyId.IsEnabled = false;
                txtUserId.IsEnabled = false;
                calValidTo.Visibility = Visibility.Visible;
                cmbUserState.Visibility = Visibility.Visible;
            }
            else
            {
                txtCompanyId.IsEnabled = true;
                txtUserId.IsEnabled = true;
                calValidTo.Visibility = Visibility.Collapsed;
                cmbUserState.Visibility = Visibility.Collapsed;
            }
        }

        public string UserCompanyId
        {
            get => txtCompanyId.Text;
            set => txtCompanyId.Text = value;
        }

        public string UserId
        {
            get => txtUserId.Text;
            set => txtUserId.Text = value;
        }

        public string UserName
        {
            get => txtUserName.Text;
            set => txtUserName.Text = value;
        }

        public string UserEmail
        {
            get => txtUserEmail.Text;
            set => txtUserEmail.Text = value;
        }

        public DateTime? ValidFrom
        {
            get => calValidFrom.Date?.DateTime;
            set => calValidFrom.Date = value.HasValue ? new DateTimeOffset(value.Value) : (DateTimeOffset?)null;
        }

        public DateTime ValidTo
        {
            get => calValidTo.Date.Value.DateTime;
            set => calValidTo.Date = new DateTimeOffset(value);
        }

        public string UserRole
        {
            get => cmbUserRole.SelectedItem.ToString();
            set => cmbUserRole.SelectedItem = value;
        }

        public string RowState
        {
            get => cmbUserState.SelectedItem.ToString();
            set => cmbUserState.SelectedItem = value;
        }

    }
}
