using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PlatnedMahara.Classes;
using PlatnedMahara.DataAccess.Methods;
using PlatnedMahara.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using static PlatnedMahara.Pages.PageHome;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PlatnedMahara
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Refer from BaseUi - Start
        IntPtr hWnd = IntPtr.Zero;
        private SUBCLASSPROC SubClassDelegate;
        // Refer from BaseUi - End

        public static MainWindow Instance { get; private set; }
        public XamlRoot XamlRoot { get; private set; }

        private TabView tabView;

        public MainWindow()
        {
            InitializeComponent();

            this.Activated += MainWindow_Activated;

            // Refer from BaseUi - Start
            hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
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
            mnuItmSubProfileLogin.Click += baseUi.mnuItmSubProfileLogin_Click;
            mnuItmSubProfileLogout.Click += baseUi.mnuItmSubProfileLogout_Click;
            mnuItmPlatnedPass.Click += baseUi.mnuItmPlatnedPass_Click;

            if (GlobalData.IsLoggedIn)
            {
                mnuItmSubProfileLogin.Visibility = Visibility.Collapsed;
                mnuItmSubProfileLogout.Visibility = Visibility.Visible;
            }
            else {
                mnuItmSubProfileLogin.Visibility = Visibility.Visible;
                mnuItmSubProfileLogout.Visibility = Visibility.Collapsed;
            }
            // Refer from BaseUi - End

            Instance = this;
            tabView = TabViewMain;
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

        private void NewTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var senderTabView = args.Element as TabView;
            senderTabView.TabItems.Add(CreateNewTab(senderTabView.TabItems.Count));

            args.Handled = true;
        }

        private void CloseSelectedTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var InvokedTabView = (args.Element as TabView);

            // Only close the selected tab if it is closeable
            if (((TabViewItem)InvokedTabView.SelectedItem).IsClosable)
            {
                InvokedTabView.TabItems.Remove(InvokedTabView.SelectedItem);
            }

            args.Handled = true;
        }

        private void NavigateToNumberedTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var InvokedTabView = (args.Element as TabView);

            int tabToSelect = 0;

            switch (sender.Key)
            {
                case Windows.System.VirtualKey.Number1:
                    tabToSelect = 0;
                    break;
                case Windows.System.VirtualKey.Number2:
                    tabToSelect = 1;
                    break;
                case Windows.System.VirtualKey.Number3:
                    tabToSelect = 2;
                    break;
                case Windows.System.VirtualKey.Number4:
                    tabToSelect = 3;
                    break;
                case Windows.System.VirtualKey.Number5:
                    tabToSelect = 4;
                    break;
                case Windows.System.VirtualKey.Number6:
                    tabToSelect = 5;
                    break;
                case Windows.System.VirtualKey.Number7:
                    tabToSelect = 6;
                    break;
                case Windows.System.VirtualKey.Number8:
                    tabToSelect = 7;
                    break;
                case Windows.System.VirtualKey.Number9:
                    // Select the last tab
                    tabToSelect = InvokedTabView.TabItems.Count - 1;
                    break;
            }

            // Only select the tab if it is in the list
            if (tabToSelect < InvokedTabView.TabItems.Count)
            {
                InvokedTabView.SelectedIndex = tabToSelect;
            }

            args.Handled = true;
        }

        private void TabView_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i <= 3; i++)
            {
                (sender as TabView).TabItems.Add(CreateNewTab(i));
            }
        }

        private void TabView_AddButtonClick(TabView sender, object args)
        {
            sender.TabItems.Add(CreateNewTab(sender.TabItems.Count + 1));
        }

        private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }

        public TabViewItem CreateNewTab(int index)
        {
            TabViewItem newItem = new TabViewItem();

            newItem.Header = $"Execution Window {index}";
            newItem.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document };

            // The content of the tab is often a frame that contains a page, though it could be any UIElement.
            Frame frame = new Frame();

            switch (index)
            {
                case 0:
                    frame.Navigate(typeof(PageHome));
                    break;
                case 1:
                    frame.Navigate(typeof(PageHome));
                    break;
                case 2:
                    frame.Navigate(typeof(PageHome));
                    break;
                case 100:
                    newItem.Header = "Configuration Window | Authentication";
                    frame.Navigate(typeof(PageConfig));
                    break;
                case 101:
                    newItem.Header = "Configuration Window | Application Logs";
                    frame.Navigate(typeof(PageAppLogs));
                    break;
                case 102:
                    newItem.Header = "Help Window | Application License";
                    frame.Navigate(typeof(PageLicense));
                    break;
                case 103:
                    newItem.Header = "Administration | Platned Pass";
                    frame.Navigate(typeof(PagePlatnedPass));
                    break;
                default:
                    frame.Navigate(typeof(PageHome));
                    break;
            }

            newItem.Content = frame;

            return newItem;
        }

        public void AddNewTabForMainWindow(int indexValue)
        {
            var newTab = CreateNewTab(indexValue);
            tabView.TabItems.Add(newTab);
            tabView.SelectedItem = newTab;
        }

        public void ShowInfoBar(string title, string message, InfoBarSeverity severity)
        {
            infoBar.Title = title;
            infoBar.Message = message;
            infoBar.Severity = severity;
            infoBar.IsOpen = true;

            PlayNotificationSound();

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += (sender, args) =>
            {
                infoBar.IsOpen = false;
                timer.Stop();
            };

            timer.Start();

        }

        private async void PlayNotificationSound()
        {
            ElementSoundPlayer.State = ElementSoundPlayerState.On;
            ElementSoundPlayer.Play(ElementSoundKind.Show);

            var timerSound = new DispatcherTimer();
            timerSound.Interval = TimeSpan.FromSeconds(1);
            timerSound.Tick += (sender, args) =>
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.Off;
                timerSound.Stop();
            };
            timerSound.Start();
        }

        private void MainWindow_Activated(object sender, Microsoft.UI.Xaml.WindowActivatedEventArgs e)
        {
            if (!GlobalData.IsLoggedIn)
            {
                AuthLogin();
            }
            // Unsubscribe from the Activated event to avoid calling it again
            this.Activated -= MainWindow_Activated;
        }

        public async void AuthLogin()
        {
            var result = ContentDialogResult.None;
            var loginPage = new PageLogin(); // Create PageLogin instance once

            // Ensure MainWindowXamlRoot is loaded
            if (MainWindowXamlRoot.XamlRoot == null)
            {
                MainWindowXamlRoot.Loaded += async (s, e) =>
                {
                    result = await ShowLoginDialog(loginPage);
                    await HandleLoginDialogResultAsync(result, loginPage);
                };
            }
            else
            {
                result = await ShowLoginDialog(loginPage);
                await HandleLoginDialogResultAsync(result, loginPage);
            }
        }

        //Show Login Page Dialog
        private async Task<ContentDialogResult> ShowLoginDialog(PageLogin loginPage)
        {
            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = MainWindowXamlRoot.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                PrimaryButtonText = "Login",
                SecondaryButtonText = "Password Reset",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = loginPage // Set the same PageLogin instance as the content
            };

            return await dialog.ShowAsync(); // Return the result of the dialog
        }

        //Show Password Reset Page Dialog
        private async Task<ContentDialogResult> ShowPasswordResetDialog(PageResetPassword pageResetPassword)
        {
            ContentDialog dialogReset = new ContentDialog
            {
                XamlRoot = MainWindowXamlRoot.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                PrimaryButtonText = "Validate",
                SecondaryButtonText = "Back To Login",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = pageResetPassword
            };

            return await dialogReset.ShowAsync();
        }

        //Show Set New Password Page Dialog
        private async Task<ContentDialogResult> ShowSetNewPasswordDialog(PageSetNewPassword pageSetNewPassword)
        {
            ContentDialog dialogSetNew = new ContentDialog
            {
                XamlRoot = MainWindowXamlRoot.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                PrimaryButtonText = "Submit",
                SecondaryButtonText = "Back To Login",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = pageSetNewPassword
            };
            return await dialogSetNew.ShowAsync();
        }

        //Login Form Result Handling
        private async Task HandleLoginDialogResultAsync(ContentDialogResult result, PageLogin loginPage)
        {
            if (result == ContentDialogResult.Primary)
            {                
                // Access Username and Password from PageLogin
                string username = loginPage.UserId;
                string password = loginPage.Password;

                bool authResponse = await AuthPlatnedPass.validateLogin(username, password);
                if (authResponse)
                {
                    GlobalData.IsLoggedIn = true;

                    mnuItmSubProfileLogin.Visibility = Visibility.Collapsed;
                    mnuItmSubProfileLogout.Visibility = Visibility.Visible;

                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Success!", $"Login Success for User: {username}", InfoBarSeverity.Success);
                    }
                }
                else
                {
                    GlobalData.IsLoggedIn = false;

                    mnuItmSubProfileLogin.Visibility = Visibility.Visible;
                    mnuItmSubProfileLogout.Visibility = Visibility.Collapsed;

                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", $"Login Unsuccessful! Please check login credentials.", InfoBarSeverity.Warning);
                    }

                    var resultNew = ContentDialogResult.None;
                    resultNew = await ShowLoginDialog(loginPage);
                    await HandleLoginDialogResultAsync(resultNew, loginPage);

                }

            }
            else if (result == ContentDialogResult.Secondary)
            {
                // Show the Password Reset Dialog
                var resetPasswordPage = new PageResetPassword(); // Create the PageResetPassword instance
                var resetResult = await ShowPasswordResetDialog(resetPasswordPage);
                await HandleResetPasswordDialogResultAsync(resetResult, resetPasswordPage);
            }
            else
            {
                if (App.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowInfoBar("Info", "User cancelled the dialog.", InfoBarSeverity.Informational);
                }
            }
        }

        //Reset Password Form Result Handling
        private async Task HandleResetPasswordDialogResultAsync(ContentDialogResult resultResetPassword, PageResetPassword pageResetPassword)
        {
            //If user validates reset password page
            if(resultResetPassword == ContentDialogResult.Primary)
            {
                //Set New Password Page Execution
                var pageSetNewPassword = new PageSetNewPassword();
                var resultSetNewPassword = await ShowSetNewPasswordDialog(pageSetNewPassword);
                await HandleSetNewPasswordDialogResultAsync(resultSetNewPassword, pageSetNewPassword);

                //Validation Process

                //Notification Process
                if (App.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowInfoBar("Success!", "Password Reset request validated successfully.", InfoBarSeverity.Success);
                }
            }
            else if(resultResetPassword == ContentDialogResult.Secondary)
            {
                // Show the Login Dialog again
                var loginPage = new PageLogin();
                var loginResult = await ShowLoginDialog(loginPage);
                await HandleLoginDialogResultAsync(loginResult, loginPage);
            }
            else
            {
                if (App.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowInfoBar("Info", "User cancelled the dialog.", InfoBarSeverity.Informational);
                }
            }
        }

        //Set New Password Form Result Handling
        private async Task HandleSetNewPasswordDialogResultAsync(ContentDialogResult resultSetNewPassword, PageSetNewPassword pageSetNewPassword)
        {
            //if user confirm set new password dialog
            if (resultSetNewPassword == ContentDialogResult.Primary)
            {
                //Validation process
                //Notification process
                if (App.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowInfoBar("Success!", "Password changed successfully.", InfoBarSeverity.Success);
                }

            }
            //If user canceled the set new password dialog
            else if (resultSetNewPassword == ContentDialogResult.Secondary)
            {
                // Show the Login Dialog again
                var loginPage = new PageLogin();
                var loginResult = await ShowLoginDialog(loginPage);
                await HandleLoginDialogResultAsync(loginResult, loginPage);
                
            }
            //If user clicked back to login set new password dialog
            else
            {
                if (App.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowInfoBar("Info", "User canceled new password set dialog!", InfoBarSeverity.Informational);
                }
            }
        }


    }
}
