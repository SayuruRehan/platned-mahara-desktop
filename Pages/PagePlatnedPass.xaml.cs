using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PlatnedMahara.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PlatnedMahara.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PagePlatnedPass : Page
    {
        public PagePlatnedPass()
        {
            this.InitializeComponent();

            // Mahara-85
            AccessCheck();
        }

        private void NavigationView_SelectionChanged8(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            // Get the sample number
            string sampleNum = (sender.Name).Substring(8);

            /*if (args.IsSettingsSelected)
            {
                contentFrame8.Navigate(typeof(SampleSettingsPage));
            }
            else
            {*/
                var selectedItem = (Microsoft.UI.Xaml.Controls.NavigationViewItem)args.SelectedItem;
                string selectedItemTag = ((string)selectedItem.Tag);
                sender.Header = selectedItem.Content;
                string pageName = "PlatnedMahara.Pages.PlatnedPassPages." + selectedItemTag;
                Type pageType = Type.GetType(pageName);
                contentFramePlatnedPass.Navigate(pageType);
           /* }*/
        }

        #region Mahara-85 - Access Check

        private void AccessCheck()
        {
            if(AccessControl.IsGranted("PGE_READ_COMPANY", "R"))
            { pagePassCompany.Visibility = Visibility.Visible; }
            else { pagePassCompany.Visibility = Visibility.Collapsed; }

            if (AccessControl.IsGranted("PGE_READ_USER", "R"))
            { pagePassUserManagement.Visibility = Visibility.Visible; }
            else { pagePassUserManagement.Visibility = Visibility.Collapsed; }

        }

        #endregion
    }
}
