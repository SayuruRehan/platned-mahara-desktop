﻿#pragma checksum "C:\Users\NimeshEkanayakePlatn\source\repos\PL-PlatnedTestMatic\Pages\PageConfig.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "624C91D1C14980788290C627099AD12E96EEF0C490DA0D6D29020DFEF5629CE2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PL_PlatnedTestMatic.Pages
{
    partial class PageConfig : 
        global::Microsoft.UI.Xaml.Controls.Page, 
        global::Microsoft.UI.Xaml.Markup.IComponentConnector
    {

        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2408")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // Pages\PageConfig.xaml line 64
                {
                    this.progAuth = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ProgressBar>(target);
                }
                break;
            case 3: // Pages\PageConfig.xaml line 65
                {
                    this.btnAuthenticate = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.btnAuthenticate).Click += this.btnAuthenticate_Click;
                }
                break;
            case 4: // Pages\PageConfig.xaml line 66
                {
                    this.btnResetAuth = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.btnResetAuth).Click += this.btnResetAuthBasicData_Click;
                }
                break;
            case 5: // Pages\PageConfig.xaml line 61
                {
                    this.txtScope = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBox>(target);
                }
                break;
            case 6: // Pages\PageConfig.xaml line 49
                {
                    this.txtClientSecret = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.PasswordBox>(target);
                }
                break;
            case 7: // Pages\PageConfig.xaml line 51
                {
                    this.revealModeCheckBox = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.CheckBox>(target);
                    ((global::Microsoft.UI.Xaml.Controls.CheckBox)this.revealModeCheckBox).Checked += this.RevealModeCheckbox_Changed;
                    ((global::Microsoft.UI.Xaml.Controls.CheckBox)this.revealModeCheckBox).Unchecked += this.RevealModeCheckbox_Changed;
                }
                break;
            case 8: // Pages\PageConfig.xaml line 40
                {
                    this.txtClientId = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBox>(target);
                }
                break;
            case 9: // Pages\PageConfig.xaml line 32
                {
                    this.txtAccessTokenUrl = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBox>(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }


        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2408")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Microsoft.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

