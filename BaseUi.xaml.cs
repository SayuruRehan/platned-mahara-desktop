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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static PlatnedMahara.Pages.PageHome;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PlatnedMahara
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>

    public partial class BaseUi : Window
    {

        // Refer from BaseUi - Start
        IntPtr hWnd = IntPtr.Zero;
        private SUBCLASSPROC SubClassDelegate;
        // Refer from BaseUi - End

        public BaseUi()
        {
            InitializeComponent();

            // Refer from BaseUi - Start
            /*hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            SubClassDelegate = new SUBCLASSPROC(WindowSubClass);
            bool bReturn = SetWindowSubclass(hWnd, SubClassDelegate, 0, 0);
            Window window = this;
            window.ExtendsContentIntoTitleBar = true;
            window.SetTitleBar(AppTitleBar);
            BaseUi baseUi = new BaseUi();
            mnuItmExit.Click += baseUi.mnuItmExit_Click;
            mnuItmSubConfAuth.Click += baseUi.mnuItmSubConfAuth_Click;
            mnuItmSubConfLogs.Click += baseUi.mnuItmSubConfLogs_Click;
            mnuItmSubHelpLicense.Click += baseUi.mnuItmSubHelpLicense_Click;
            mnuItmSubProfileLogout.Click += baseUi.mnuItmSubProfileLogout_Click;
            
            if (GlobalData.IsLoggedIn)
            {
                mnuItmSubProfileLogin.Visibility = Visibility.Collapsed;
                mnuItmSubProfileLogout.Visibility = Visibility.Visible;
            }
            else {
                mnuItmSubProfileLogin.Visibility = Visibility.Visible;
                mnuItmSubProfileLogout.Visibility = Visibility.Collapsed;
            }*/
            // Refer from BaseUi - End
        }

        // Refer from BaseUi - Start
        private int WindowSubClass(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData)
        {
            switch (uMsg)
            {
                case WM_GETMINMAXINFO:
                    {
                        MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
                        mmi.ptMinTrackSize.X = App.WinMinWidth;
                        mmi.ptMinTrackSize.Y = App.WinMinHeight;
                        Marshal.StructureToPtr(mmi, lParam, false);
                        return 0;
                    }
                    break;
            }
            return DefSubclassProc(hWnd, uMsg, wParam, lParam);
        }
        public delegate int SUBCLASSPROC(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData);
        [DllImport("Comctl32.dll", SetLastError = true)]
        public static extern bool SetWindowSubclass(IntPtr hWnd, SUBCLASSPROC pfnSubclass, uint uIdSubclass, uint dwRefData);
        [DllImport("Comctl32.dll", SetLastError = true)]
        public static extern int DefSubclassProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);
        public const int WM_GETMINMAXINFO = 0x0024;
        public struct MINMAXINFO
        {
            public System.Drawing.Point ptReserved;
            public System.Drawing.Point ptMaxSize;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Point ptMinTrackSize;
            public System.Drawing.Point ptMaxTrackSize;
        }
        // Refer from BaseUi - End

        // Codes for Menu Items
        public void mnuItmExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
        public void mnuItmSubConfAuth_Click(object sender, RoutedEventArgs e)
        {
            // Access the current instance of MainWindow
            if (MainWindow.Instance != null)
            {
                // Call the method to add a new tab for PageConfig
                MainWindow.Instance.AddNewTabForMainWindow(100);
            }
        }
        public void mnuItmSubConfLogs_Click(object sender, RoutedEventArgs e)
        {
            // Access the current instance of MainWindow
            if (MainWindow.Instance != null)
            {
                // Call the method to add a new tab for PageConfig
                MainWindow.Instance.AddNewTabForMainWindow(101);
            }
        }
        public void mnuItmSubHelpLicense_Click(object sender, RoutedEventArgs e)
        {
            // Access the current instance of MainWindow
            if (MainWindow.Instance != null)
            {
                // Call the method to add a new tab for PageConfig
                MainWindow.Instance.AddNewTabForMainWindow(102);
            }
        }

        public void mnuItmSubProfileLogout_Click(object sender, RoutedEventArgs e)
        {
            GlobalData.IsLoggedIn = false;
            GlobalData.UserId = "";
            GlobalData.UserName = "";
            GlobalData.CompanyId = "";

            // Close the current main window instance
            if (App.MainWindow is MainWindow mainWindow)
            {
                mainWindow.Close();
            }

            // Recreate a new main window instance and set it as the current window
            MainWindow mainWindowNew = new MainWindow();
            mainWindowNew.Activate();
        }

        public void mnuItmSubProfileLogin_Click(object sender, RoutedEventArgs e)
        {
            GlobalData.IsLoggedIn = false;
            GlobalData.UserId = "";
            GlobalData.UserName = "";
            GlobalData.CompanyId = "";

            // Close the current main window instance
            if (App.MainWindow is MainWindow mainWindow)
            {
                mainWindow.Close();
            }

            // Recreate a new main window instance and set it as the current window
            MainWindow mainWindowNew = new MainWindow();
            mainWindowNew.Activate();
        }

        internal void mnuItmPlatnedPass_Click(object sender, RoutedEventArgs e)
        {
            // Access the current instance of MainWindow
            if (MainWindow.Instance != null)
            {
                // Call the method to add a new tab for PageConfig
                MainWindow.Instance.AddNewTabForMainWindow(103);
            }
        }
    }
}
