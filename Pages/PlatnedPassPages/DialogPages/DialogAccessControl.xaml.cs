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
    public sealed partial class DialogAccessControl : Page
    {
        bool _isEdit = false;
        public DialogAccessControl()
        {
            this.InitializeComponent();
        }

        public DialogAccessControl(bool isedit)
        {
            _isEdit = isedit;
            this.InitializeComponent();
            if (_isEdit)
            {
                txtAppFunction.IsEnabled = false;
            }
        }

        public string AppFunction
        {
            get => txtAppFunction.Text;
            set => txtAppFunction.Text = value;
        }

        public string AppFunctionDescription
        {
            get => txtAppFuncDesc.Text;
            set => txtAppFuncDesc.Text = value;
        }

        public string UserRole
        {
            get => cmbUserRole.SelectedItem.ToString();
            set => cmbUserRole.SelectedItem = value;
        }
        public string ReadAllowed
        {
            get => chkRead.IsChecked.ToString();
            set => chkRead.IsChecked = Convert.ToBoolean(value);
        }
        public string CreateAllowed
        {
            get => chkCreate.IsChecked.ToString();
            set => chkCreate.IsChecked = Convert.ToBoolean(value);
        }
        public string UpdateAllowed
        {
            get => chkUpdate.IsChecked.ToString();
            set => chkUpdate.IsChecked = Convert.ToBoolean(value);
        }
        public string DeleteAllowed
        {
            get => chkDelete.IsChecked.ToString();
            set => chkDelete.IsChecked = Convert.ToBoolean(value);
        }
    }
}
