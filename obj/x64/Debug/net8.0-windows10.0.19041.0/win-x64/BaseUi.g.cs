﻿#pragma checksum "C:\Users\NimeshEkanayakePlatn\source\repos\apt-desktop\BaseUi.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "137039BFBB32203909B9309A8490A9AFC19B2A8F81B3406B004D127D354E9C88"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PL_PlatnedTestMatic
{
    partial class BaseUi : 
        global::Microsoft.UI.Xaml.Window, 
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
            case 2: // BaseUi.xaml line 14
                {
                    this.AppTitleBar = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Border>(target);
                }
                break;
            case 3: // BaseUi.xaml line 53
                {
                    this.mnuItmSubHelpLicense = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                }
                break;
            case 4: // BaseUi.xaml line 30
                {
                    this.mnuItmExit = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                }
                break;
            case 5: // BaseUi.xaml line 23
                {
                    this.mnuItmSubConfAuth = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                }
                break;
            case 6: // BaseUi.xaml line 24
                {
                    this.mnuItmSubConfLogs = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                }
                break;
            case 7: // BaseUi.xaml line 16
                {
                    this.SlicesIcon = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.BitmapIcon>(target);
                }
                break;
            case 8: // BaseUi.xaml line 17
                {
                    this.AppTitle = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
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
