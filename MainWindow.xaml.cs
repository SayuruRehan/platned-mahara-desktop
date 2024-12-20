using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using PlatnedMahara.Pages.PlatnedPassPages;
using PlatnedMahara.Pages.PlatnedPassPages.DialogPages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using static PlatnedMahara.Pages.PageHome;
using Windows.Graphics;
using WinRT.Interop;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using System.Xml.Linq;
using Windows.ApplicationModel.Core;
using System.Net.Mail;
using System.Net;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using Style = Microsoft.UI.Xaml.Style;

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
        private bool centered;


        public static MainWindow Instance { get; private set; }
        public XamlRoot XamlRoot { get; private set; }

        private TabView tabView;

        private static void Center(Window window)
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(window);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);

            if (AppWindow.GetFromWindowId(windowId) is AppWindow appWindow &&
                DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Nearest) is DisplayArea displayArea)
            {
                PointInt32 CenteredPosition = appWindow.Position;
                CenteredPosition.X = (displayArea.WorkArea.Width - appWindow.Size.Width) / 2;
                CenteredPosition.Y = (displayArea.WorkArea.Height - appWindow.Size.Height) / 2;
                appWindow.Move(CenteredPosition);
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            this.Activated += MainWindow_Activated;
            this.Closed += MainWindow_Closed;  // Handle the window's Closed event

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
            // Mahara-97 - Template Manager - START
            mnuItmSubAssistTempManager.Click += baseUi.mnuItmTemplateManager_Click;
            // Mahara-97 - END
            if (GlobalData.IsLoggedIn)
            {
                mnuItmSubProfileLogin.Visibility = Visibility.Collapsed;
                mnuItmSubProfileLogout.Visibility = Visibility.Visible;
            }
            else
            {
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

        #region Key Board Mapping for Tab View

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

        #endregion

        #region Tab Handling (Load, New Tab)

        private void TabView_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i <= 1; i++)
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
            newItem.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Microsoft.UI.Xaml.Controls.Symbol.Document };

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
                // Mahara-97 - START
                case 104:
                    newItem.Header = "Assistant | Template Manager";
                    frame.Navigate(typeof(PageTemplateManager));
                    break;
                    break;
                // Mahara-97 - END
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

        #endregion

        #region Toast message for information

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

        #endregion

        #region Login Dialog handling

        private void MainWindow_Activated(object sender, Microsoft.UI.Xaml.WindowActivatedEventArgs e)
        {
            if (!GlobalData.IsLoggedIn)
            {
                AuthLogin();
            }
            if (this.centered is false)
            {
                Center(this);
                centered = true;
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
                //CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = loginPage // Set the same PageLogin instance as the content
            };

            return await dialog.ShowAsync(); // Return the result of the dialog
        }

        //Login Form Result Handling
        private async Task HandleLoginDialogResultAsync(ContentDialogResult result, PageLogin loginPage)
        {
            if (result == ContentDialogResult.Primary)
            {
                // Access Username and Password from PageLogin
                string username = loginPage.UserId;
                string password = loginPage.Password;
                //Mahara 87 - Login validation logic change - START
                if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                //Mahara 87 - Login validation logic change - END
                {
                    Pass_Users_Company pass_User_det = new Pass_Users_Company
                    {
                        UserID = loginPage.UserId
                    };

                    List<Pass_Users_Company> pass_Users = new List<Pass_Users_Company>();
                    pass_Users = new AuthPlatnedPass().GetLoginUser(pass_User_det);

                    if (pass_Users != null && pass_Users.Count > 0)
                    {
                        foreach (Pass_Users_Company pu in pass_Users)
                        {
                            if (Encrypt.VerifyPassword(password, pu.Password))
                            {
                                GlobalData.UserId = pu.UserID;
                                GlobalData.CompanyId = pu.CompanyID;
                                GlobalData.IsLoggedIn = true;
                                GlobalData.UserRole = pu.UserRole.Trim();
                                GlobalData.UserEmail = pu.UserEmail;
                                GlobalData.LicenseKey = pu.LicenseKey;
                                GlobalData.UserStatus = pu.RowState;

                                mnuItmSubProfileLogin.Visibility = Visibility.Collapsed;
                                mnuItmSubProfileLogout.Visibility = Visibility.Visible;

                                checkLicenseExpiry();

                                if (App.MainWindow is MainWindow mainWindow)
                                {
                                    for (int i = 1; i <= 1; i++)
                                    {
                                        var newTab = CreateNewTab(i);
                                        mainWindow.tabView.TabItems.Add(newTab);

                                        // Wait for 2 seconds and then set the selected tab
                                        SetSelectedTabWithDelay(mainWindow.tabView, newTab);
                                    }

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
                                    mainWindow.AuthLogin();
                                }

                            }

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
                            mainWindow.AuthLogin();
                        }
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
                        mainWindow.AuthLogin();
                    }
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

        #endregion

        #region Password Reset and Set New Password handling

        //Show Password Reset Page Dialog
        private async Task<ContentDialogResult> ShowPasswordResetDialog(PageResetPassword pageResetPassword)
        {
            ContentDialog dialogReset = new ContentDialog
            {
                XamlRoot = MainWindowXamlRoot.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                PrimaryButtonText = "Validate",
                SecondaryButtonText = "Back To Login",
                //CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = pageResetPassword
            };

            return await dialogReset.ShowAsync();
        }

        //OTP Generate Function
        public string GenerateOTP()
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] otp = new char[6];

            for (int i = 0; i < otp.Length; i++)
            {
                otp[i] = characters[random.Next(characters.Length)];
            }
            return new string(otp);
        }

        //OTP Sending Function


        //Reset Password Form Result Handling
        private async Task HandleResetPasswordDialogResultAsync(ContentDialogResult resultResetPassword, PageResetPassword pageResetPassword)
        {
            //If user validates reset password page
            if (resultResetPassword == ContentDialogResult.Primary)
            {

                // Access Username and Password from PageLogin
                string companyId = pageResetPassword.ResetCompanyId;
                string userId = pageResetPassword.ResetUserId;
                string userEmail = pageResetPassword.ResetEmail;

                if (companyId != "" && userId != "" && userEmail != "")
                {
                    Pass_Users_Company pass_User_reset = new Pass_Users_Company
                    {
                        UserID = pageResetPassword.ResetUserId
                    };

                    List<Pass_Users_Company> pass_User_Reset_details = new List<Pass_Users_Company>();
                    pass_User_Reset_details = new AuthPlatnedPass().GetPasswordResetUser(pass_User_reset);

                    if (pass_User_Reset_details != null && pass_User_Reset_details.Count > 0)
                    {
                        foreach (Pass_Users_Company pu in pass_User_Reset_details)
                        {

                            if (pu.CompanyID == companyId && pu.UserEmail == userEmail)
                            {

                                string otp = GenerateOTP();

                                try
                                {
                                    //bool emailSent = await SendEmail(pu.UserEmail, otp);
                                    //sending OTP Value via Email
                                    //OTP html body
                                    string emailContent = $@"
<div style=""display: flex; justify-content: center;width:100%; margin: margin: 50px 0;"">
    <table border=""0"" cellspacing=""0"" cellpadding=""0"" style=""padding-bottom: 20px; max-width: 516px; min-width: 220px"">
        <tbody>
            <tr>
                <td width=""8"" style=""width: 8px""></td>
                <td>
                    <div
                        style=""border-style: solid; border-width: thin; border-color: #dadce0; border-radius: 8px; padding: 40px 20px"" align=""center"">
                        <img
                            src=""https://res.cloudinary.com/dqmgkczgk/image/upload/v1734331807/Platned_mahara/PlatnedLogo_v2rddw.png""
                            width=""74"" height=""74""
                            aria-hidden=""true""
                            style=""margin-bottom: 16px""
                            alt=""MaharaPlatned""
                            class=""CToWUd""
                            data-bit=""iit""
                        />
                        <div
                            style=""
                                font-family: 'Google Sans', Roboto, RobotoDraft, Helvetica, Arial, sans-serif;
                                border-bottom: thin solid #dadce0;
                                color: rgba(0, 0, 0, 0.87);
                                line-height: 32px;
                                padding-bottom: 24px;
                                text-align: center;
                                word-break: break-word;
                            "">
                            <div style=""font-size: 24px"">Account Password Reset</div>
                            <table align=""center"" style=""margin-top: 8px"">
                                <tbody>
                                    <tr style=""line-height: normal"">
                                        <td>
                                            <a
                                                style=""
                                                    font-family: 'Google Sans', Roboto, RobotoDraft, Helvetica, Arial, sans-serif;
                                                    color: rgba(0, 0, 0, 0.87);
                                                    font-size: 14px;
                                                    line-height: 20px;
                                                ""
                                            >{pu.UserEmail}</a>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div
                            style=""
                                font-family: Roboto-Regular, Helvetica, Arial, sans-serif;
                                font-size: 14px;
                                color: rgba(0, 0, 0, 0.87);
                                line-height: 20px;
                                padding-top: 20px;
                                text-align: left;
                            "">
                            <h3>Platned Mahara Password Reset OTP</h3>
                            <p>Keep your OTP (One-Time Password) confidential and do not share it with anyone.</p>
                            <h4>The OTP code is: {otp}</h4>
                            If the issue persists, contact us at mahara@platned.com for further assistance.
                        </div>
                    </div>
                </td>
                <td width=""8"" style=""width: 8px""></td>
            </tr>
        </tbody>
    </table>
</div>";
                                    bool emailSent = EmailSender.MailSender(pu.UserEmail, "Mahara Password Reset OTP Code", emailContent, true);
                                    if (emailSent)
                                    {
                                        // Notify the user
                                        if (App.MainWindow is MainWindow mainWindowSuccess)
                                        {
                                            mainWindowSuccess.ShowInfoBar("Success!", $"OTP Sent Successfully", InfoBarSeverity.Success);
                                        }

                                        for (global::System.Int32 i = 0; i < 4; i++)
                                        {
                                            var pageOtpValidation = new PageOtpValidation();
                                            var resultOtpValidation = await ShowOtpValidationDialog(pageOtpValidation);
                                            await HandleSetOtpDialogResultAsync(resultOtpValidation, pageOtpValidation);

                                            // Check OTP validation
                                            if (pageOtpValidation.Otp == otp)
                                            {
                                                if (App.MainWindow is MainWindow mainWindow)
                                                {
                                                    mainWindow.ShowInfoBar("Success!!", "OTP Verified successfully.", InfoBarSeverity.Success);
                                                }

                                                var pageSetNewPassword = new PageSetNewPassword(companyId, userId, userEmail);
                                                var resultSetNewPassword = await ShowSetNewPasswordDialog(pageSetNewPassword);
                                                await HandleSetNewPasswordDialogResultAsync(resultSetNewPassword, pageSetNewPassword);
                                                break;
                                            }
                                            else if (i == 3)
                                            {
                                                var resetPasswordPage = new PageResetPassword(); // Create the PageResetPassword instance
                                                var resetResult = await ShowPasswordResetDialog(resetPasswordPage);
                                                await HandleResetPasswordDialogResultAsync(resetResult, resetPasswordPage);
                                            }
                                            else if (resultOtpValidation == ContentDialogResult.Secondary) {
                                                break;
                                            }
                                            else
                                            {
                                                if (App.MainWindow is MainWindow mainWindow)
                                                {
                                                    mainWindow.ShowInfoBar("Attention!", "OTP doesn't match.", InfoBarSeverity.Warning);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (App.MainWindow is MainWindow mainWindowAttention)
                                        {
                                            mainWindowAttention.ShowInfoBar("Attention!", $"OTO sending unsuccessfull.", InfoBarSeverity.Warning);
                                        }
                                    }

                                }   
                                catch (Exception ex)
                                {
                                    // Handle errors
                                    if (App.MainWindow is MainWindow mainWindowAttention)
                                    {
                                        mainWindowAttention.ShowInfoBar("Attention!", $"OTO sending unsuccessfull.", InfoBarSeverity.Warning);
                                    }
                                }

                            }
                            else
                            {
                                mnuItmSubProfileLogin.Visibility = Visibility.Visible;
                                mnuItmSubProfileLogout.Visibility = Visibility.Collapsed;

                                if (App.MainWindow is MainWindow mainWindow)
                                {
                                    mainWindow.ShowInfoBar("Attention!", $"Validation Unsuccessful! Please check user details.", InfoBarSeverity.Warning);
                                    mainWindow.AuthLogin();
                                }
                            }

                        }
                    }
                }
                else
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", $"Validation Unsuccessful! Please check user details.", InfoBarSeverity.Warning);
                        // Show the Password Reset Dialog
                        var resetPasswordPage = new PageResetPassword(); // Create the PageResetPassword instance
                        var resetResult = await ShowPasswordResetDialog(resetPasswordPage);
                        await HandleResetPasswordDialogResultAsync(resetResult, resetPasswordPage);
                    };


                }

            }
            else if (resultResetPassword == ContentDialogResult.Secondary)
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

        #region Task 91 -  OTP Dialog Box Handeling
        //Show Otp validation Form
        private async Task<ContentDialogResult> ShowOtpValidationDialog(PageOtpValidation pageOtpValidation)
        {
            ContentDialog dialogSetNew = new ContentDialog
            {
                XamlRoot = MainWindowXamlRoot.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                PrimaryButtonText = "Submit",
                SecondaryButtonText = "Resend",
                //CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = pageOtpValidation
            };
            return await dialogSetNew.ShowAsync();
        }

        //Set OTP Form Result Handling
        private async Task HandleSetOtpDialogResultAsync(ContentDialogResult resultSetOtp, PageOtpValidation pageOtpValidation)
        {
            //if user confirm set new password dialog
            if (resultSetOtp == ContentDialogResult.Primary)
            {
                if (pageOtpValidation.Otp == "")
                {
                    //Validation process
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", "OTP cannot be empty.", InfoBarSeverity.Warning);
                    }

                    pageOtpValidation = new PageOtpValidation(); // Create the PageResetPassword instance
                    var setNewPassword = await ShowOtpValidationDialog(pageOtpValidation);
                    await HandleSetOtpDialogResultAsync(setNewPassword, pageOtpValidation);
                }
            }
            //If user clicked back to login set new password dialog
            else
            {
                var resetPasswordPage = new PageResetPassword(); // Create the PageResetPassword instance
                var resetResult = await ShowPasswordResetDialog(resetPasswordPage);
                await HandleResetPasswordDialogResultAsync(resetResult, resetPasswordPage);
            }
        }
        #endregion

        //Show Set New Password Page Dialog
        private async Task<ContentDialogResult> ShowSetNewPasswordDialog(PageSetNewPassword pageSetNewPassword)
        {
            ContentDialog dialogSetNew = new ContentDialog
            {
                XamlRoot = MainWindowXamlRoot.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                PrimaryButtonText = "Submit",
                SecondaryButtonText = "Back To Login",
                //CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = pageSetNewPassword
            };
            return await dialogSetNew.ShowAsync();
        }

        //Set New Password Form Result Handling
        private async Task HandleSetNewPasswordDialogResultAsync(ContentDialogResult resultSetNewPassword, PageSetNewPassword pageSetNewPassword)
        {
            //if user confirm set new password dialog
            if (resultSetNewPassword == ContentDialogResult.Primary)
            {
                if (pageSetNewPassword.newPassword != "" && pageSetNewPassword.confirmPassword != "")
                {
                    //Validation process
                    if (pageSetNewPassword.newPassword == pageSetNewPassword.confirmPassword)
                    {
                        string companyId = pageSetNewPassword.companyId;
                        string userId = pageSetNewPassword.userId;
                        string newPassword = Encrypt.EncryptPassword(pageSetNewPassword.newPassword);
                        string userEmail = pageSetNewPassword.userEmail;

                        Pass_Users_Company pass_User_reset = new Pass_Users_Company
                        {
                            CompanyID = companyId,
                            UserID = userId,
                            Password = newPassword,
                            UserEmail = userEmail,
                        };

                        List<Pass_Users_Company> pass_User_Reset_details = new List<Pass_Users_Company>();
                        bool isUpdated = new AuthPlatnedPass().EditUserPassword(pass_User_reset);

                        if (isUpdated)
                        {
                            //Notification process
                            if (App.MainWindow is MainWindow mainWindow)
                            {
                                mainWindow.ShowInfoBar("Success!", "Password changed successfully.", InfoBarSeverity.Success);
                            }

                            // Show the Login Dialog again
                            var loginPage = new PageLogin();
                            var loginResult = await ShowLoginDialog(loginPage);
                            await HandleLoginDialogResultAsync(loginResult, loginPage);


                        }
                        else
                        {
                            mnuItmSubProfileLogin.Visibility = Visibility.Visible;
                            mnuItmSubProfileLogout.Visibility = Visibility.Collapsed;

                            if (App.MainWindow is MainWindow mainWindow)
                            {
                                mainWindow.ShowInfoBar("Error!", $"Password reset unsuccessful!", InfoBarSeverity.Error);
                                mainWindow.AuthLogin();
                            }
                        }

                    }
                    else
                    {
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Attention!", "Password doesn't match.", InfoBarSeverity.Warning);
                        }

                        pageSetNewPassword = new PageSetNewPassword(pageSetNewPassword.companyId, pageSetNewPassword.userId, pageSetNewPassword.userEmail); // Create the PageResetPassword instance
                        var setNewPassword = await ShowSetNewPasswordDialog(pageSetNewPassword);
                        await HandleSetNewPasswordDialogResultAsync(setNewPassword, pageSetNewPassword);
                    }
                }
                else
                {
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Attention!", "New Password cannot be empty.", InfoBarSeverity.Warning);
                    }

                    pageSetNewPassword = new PageSetNewPassword(pageSetNewPassword.companyId, pageSetNewPassword.userId, pageSetNewPassword.userEmail); // Create the PageResetPassword instance
                    var setNewPassword = await ShowSetNewPasswordDialog(pageSetNewPassword);
                    await HandleSetNewPasswordDialogResultAsync(setNewPassword, pageSetNewPassword);
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

        #endregion

        #region Mahara-80 License Validation at Login

        private void checkLicenseExpiry()
        {
            string configFilePath = GlobalData.configFilePath;
            string accessTokenUrl = "";
            string clientId = "";
            string clientSecret = "";
            string scope = "";
            Boolean appLoggingEnabled = false;
            string licenseKey = "";

            try
            {
                if (File.Exists(configFilePath))
                {
                    Logger.Log("Saved basic data configurations found!");

                    try
                    {
                        Logger.Log("Reading saved configuration started...");
                        Logger.Log($"Configuration path: {configFilePath}");
                        var configXml = XDocument.Load(configFilePath);
                        Logger.Log($"Configuration reading completed: {configXml}");

                        accessTokenUrl = configXml.Root.Element("AccessTokenUrl")?.Value ?? string.Empty;
                        clientId = configXml.Root.Element("ClientId")?.Value ?? string.Empty;
                        clientSecret = configXml.Root.Element("ClientSecret")?.Value ?? string.Empty;
                        scope = configXml.Root.Element("Scope")?.Value ?? string.Empty;
                        licenseKey = configXml.Root.Element("licenseKey")?.Value ?? string.Empty;
                        appLoggingEnabled = Convert.ToBoolean(configXml.Root.Element("LoggingEnabled")?.Value ?? bool.FalseString);
                        Logger.Log("Configuration retrieval completed!");

                        Logger.Log("Changing application registration state...");

                        if (licenseKey != "")
                        {
                            Pass_Users_Company pass_User_det = new Pass_Users_Company
                            {
                                CompanyID = GlobalData.CompanyId,
                                UserID = GlobalData.UserId
                            };

                            Pass_Users_Company pass_User = new Pass_Users_Company();
                            pass_User = new AuthPlatnedPass().GetPass_User_Per_Company(pass_User_det);

                            if (pass_User != null)
                            {
                                if (pass_User.LicenseKey == licenseKey)
                                {
                                    if (pass_User.ValidTo >= DateTime.Now)
                                    {
                                        Logger.Log("License Key is not expired. Ignoring config changes.");
                                    }
                                    else
                                    {
                                        Logger.Log("License Key is expired. Removing from configurations.");

                                        Logger.Log("Saving configuration started...");
                                        PageConfig pageConfig = new PageConfig();
                                        pageConfig.SaveConfigData(accessTokenUrl, clientId, clientSecret, scope, appLoggingEnabled, "");
                                        Logger.Log("Saving configuration completed!");
                                    }
                                }
                                else
                                {
                                    Logger.Log("License Key is changed on Platned Pass. Removing from configurations.");

                                    Logger.Log("Saving configuration started...");
                                    PageConfig pageConfig = new PageConfig();
                                    pageConfig.SaveConfigData(accessTokenUrl, clientId, clientSecret, scope, appLoggingEnabled, "");
                                    Logger.Log("Saving configuration completed!");
                                }
                            }
                            else
                            {
                                Logger.Log("User record for License Key check is not found on Platned Pass. Removing from configurations.");

                                Logger.Log("Saving configuration started...");
                                PageConfig pageConfig = new PageConfig();
                                pageConfig.SaveConfigData(accessTokenUrl, clientId, clientSecret, scope, appLoggingEnabled, "");
                                Logger.Log("Saving configuration completed!");
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Logger.Log($"Error loading configuration: {ex.Message}", "Error");
                    }
                }
                else
                {
                    Logger.Log("No saved basic data configurations found!");
                }

            }
            catch (System.Exception ex)
            {
                Logger.Log($"Authentication failed: {ex.Message}", "Error");
            }
        }

        #endregion

        #region  Mahara-84 Handle window closed event
        private async void MainWindow_Closed(object sender, Microsoft.UI.Xaml.WindowEventArgs e)
        {
            Application.Current.Exit();
        }
        #endregion

        #region Mahara-66 - Set selected tab after a delay allowing to load JSON data

        private async void SetSelectedTabWithDelay(TabView tabView, TabViewItem newTab)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            tabView.SelectedItem = newTab;
        }

        #endregion

    }
}
