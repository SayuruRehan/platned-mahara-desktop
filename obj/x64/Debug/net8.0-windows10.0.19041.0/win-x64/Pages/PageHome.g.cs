﻿#pragma checksum "C:\Users\NimeshEkanayakePlatn\source\repos\PL-PlatnedTestMatic\Pages\PageHome.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F9E2000A1CF0E5E14AD17A9989DBADBE513D6C6947D331D6EE9D8E3641AC38B8"
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
    partial class PageHome : 
        global::Microsoft.UI.Xaml.Controls.Page, 
        global::Microsoft.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2408")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private static class XamlBindingSetters
        {
            public static void Set_CommunityToolkit_WinUI_UI_Controls_DataGrid_ItemsSource(global::CommunityToolkit.WinUI.UI.Controls.DataGrid obj, global::System.Collections.IEnumerable value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Collections.IEnumerable) global::Microsoft.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Collections.IEnumerable), targetNullValue);
                }
                obj.ItemsSource = value;
            }
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2408")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private partial class PageHome_obj1_Bindings :
            global::Microsoft.UI.Xaml.Markup.IDataTemplateComponent,
            global::Microsoft.UI.Xaml.Markup.IXamlBindScopeDiagnostics,
            global::Microsoft.UI.Xaml.Markup.IComponentConnector,
            IPageHome_Bindings
        {
            private global::PL_PlatnedTestMatic.Pages.PageHome dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::CommunityToolkit.WinUI.UI.Controls.DataGrid obj5;

            // Static fields for each binding's enabled/disabled state
            private static bool isobj5ItemsSourceDisabled = false;

            private PageHome_obj1_BindingsTracking bindingsTracking;

            public PageHome_obj1_Bindings()
            {
                this.bindingsTracking = new PageHome_obj1_BindingsTracking(this);
            }

            public void Disable(int lineNumber, int columnNumber)
            {
                if (lineNumber == 134 && columnNumber == 31)
                {
                    isobj5ItemsSourceDisabled = true;
                }
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 5: // Pages\PageHome.xaml line 132
                        this.obj5 = global::WinRT.CastExtensions.As<global::CommunityToolkit.WinUI.UI.Controls.DataGrid>(target);
                        break;
                    default:
                        break;
                }
            }
                        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2408")]
                        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target) 
                        {
                            return null;
                        }

            // IDataTemplateComponent

            public void ProcessBindings(global::System.Object item, int itemIndex, int phase, out int nextPhase)
            {
                nextPhase = -1;
            }

            public void Recycle()
            {
                return;
            }

            // IPageHome_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.initialized = false;
            }

            public void DisconnectUnloadedObject(int connectionId)
            {
                throw new global::System.ArgumentException("No unloadable elements to disconnect.");
            }

            public bool SetDataRoot(global::System.Object newDataRoot)
            {
                this.bindingsTracking.ReleaseAllListeners();
                if (newDataRoot != null)
                {
                    this.dataRoot = global::WinRT.CastExtensions.As<global::PL_PlatnedTestMatic.Pages.PageHome>(newDataRoot);
                    return true;
                }
                return false;
            }

            public void Activated(object obj, global::Microsoft.UI.Xaml.WindowActivatedEventArgs data)
            {
                this.Initialize();
            }

            public void Loading(global::Microsoft.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::PL_PlatnedTestMatic.Pages.PageHome obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_GridItems(obj.GridItems, phase);
                    }
                }
            }
            private void Update_GridItems(global::System.Collections.ObjectModel.ObservableCollection<global::PL_PlatnedTestMatic.Pages.GridItem> obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_GridItems(obj);
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Pages\PageHome.xaml line 132
                    if (!isobj5ItemsSourceDisabled)
                    {
                        XamlBindingSetters.Set_CommunityToolkit_WinUI_UI_Controls_DataGrid_ItemsSource(this.obj5, obj, null);
                    }
                }
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2408")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private class PageHome_obj1_BindingsTracking
            {
                private global::System.WeakReference<PageHome_obj1_Bindings> weakRefToBindingObj; 

                public PageHome_obj1_BindingsTracking(PageHome_obj1_Bindings obj)
                {
                    weakRefToBindingObj = new global::System.WeakReference<PageHome_obj1_Bindings>(obj);
                }

                public PageHome_obj1_Bindings TryGetBindingObject()
                {
                    PageHome_obj1_Bindings bindingObject = null;
                    if (weakRefToBindingObj != null)
                    {
                        weakRefToBindingObj.TryGetTarget(out bindingObject);
                        if (bindingObject == null)
                        {
                            weakRefToBindingObj = null;
                            ReleaseAllListeners();
                        }
                    }
                    return bindingObject;
                }

                public void ReleaseAllListeners()
                {
                    UpdateChildListeners_GridItems(null);
                }

                public void PropertyChanged_GridItems(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    PageHome_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        string propName = e.PropertyName;
                        global::System.Collections.ObjectModel.ObservableCollection<global::PL_PlatnedTestMatic.Pages.GridItem> obj = sender as global::System.Collections.ObjectModel.ObservableCollection<global::PL_PlatnedTestMatic.Pages.GridItem>;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                        }
                        else
                        {
                            switch (propName)
                            {
                                default:
                                    break;
                            }
                        }
                    }
                }
                public void CollectionChanged_GridItems(object sender, global::System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                {
                    PageHome_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        global::System.Collections.ObjectModel.ObservableCollection<global::PL_PlatnedTestMatic.Pages.GridItem> obj = sender as global::System.Collections.ObjectModel.ObservableCollection<global::PL_PlatnedTestMatic.Pages.GridItem>;
                    }
                }
                private global::System.Collections.ObjectModel.ObservableCollection<global::PL_PlatnedTestMatic.Pages.GridItem> cache_GridItems = null;
                public void UpdateChildListeners_GridItems(global::System.Collections.ObjectModel.ObservableCollection<global::PL_PlatnedTestMatic.Pages.GridItem> obj)
                {
                    if (obj != cache_GridItems)
                    {
                        if (cache_GridItems != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)cache_GridItems).PropertyChanged -= PropertyChanged_GridItems;
                            ((global::System.Collections.Specialized.INotifyCollectionChanged)cache_GridItems).CollectionChanged -= CollectionChanged_GridItems;
                            cache_GridItems = null;
                        }
                        if (obj != null)
                        {
                            cache_GridItems = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_GridItems;
                            ((global::System.Collections.Specialized.INotifyCollectionChanged)obj).CollectionChanged += CollectionChanged_GridItems;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2408")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // Pages\PageHome.xaml line 48
                {
                    this.SourceJsonElement = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Grid>(target);
                }
                break;
            case 3: // Pages\PageHome.xaml line 58
                {
                    this.SourceCsvElement = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Grid>(target);
                }
                break;
            case 4: // Pages\PageHome.xaml line 68
                {
                    this.SourceTestElement = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Grid>(target);
                }
                break;
            case 5: // Pages\PageHome.xaml line 132
                {
                    this.dataGrid = global::WinRT.CastExtensions.As<global::CommunityToolkit.WinUI.UI.Controls.DataGrid>(target);
                }
                break;
            case 6: // Pages\PageHome.xaml line 124
                {
                    this.lblExecFinished = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Documents.Run>(target);
                }
                break;
            case 7: // Pages\PageHome.xaml line 121
                {
                    this.lblExecStarted = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Documents.Run>(target);
                }
                break;
            case 8: // Pages\PageHome.xaml line 91
                {
                    this.drpShareResults = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.DropDownButton>(target);
                }
                break;
            case 9: // Pages\PageHome.xaml line 97
                {
                    global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem element9 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)element9).Click += this.btnExportResults_Click;
                }
                break;
            case 10: // Pages\PageHome.xaml line 85
                {
                    this.btnStart = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.btnStart).Click += this.btnStart_Click;
                }
                break;
            case 11: // Pages\PageHome.xaml line 86
                {
                    this.btnStop = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.btnStop).Click += this.btnStopExecution_Click;
                }
                break;
            case 12: // Pages\PageHome.xaml line 87
                {
                    this.btnRerun = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.btnRerun).Click += this.btnRunAgain_Click;
                }
                break;
            case 13: // Pages\PageHome.xaml line 88
                {
                    this.progExec = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ProgressBar>(target);
                }
                break;
            case 14: // Pages\PageHome.xaml line 61
                {
                    this.PickCsvFileButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.PickCsvFileButton).Click += this.PickCsvFileButton_Click;
                }
                break;
            case 15: // Pages\PageHome.xaml line 62
                {
                    this.PickCsvFileOutputTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 16: // Pages\PageHome.xaml line 51
                {
                    this.PickJsonFileButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.PickJsonFileButton).Click += this.PickJsonFileButton_Click;
                }
                break;
            case 17: // Pages\PageHome.xaml line 52
                {
                    this.PickJsonFileOutputTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
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
            switch(connectionId)
            {
            case 1: // Pages\PageHome.xaml line 2
                {                    
                    global::Microsoft.UI.Xaml.Controls.Page element1 = (global::Microsoft.UI.Xaml.Controls.Page)target;
                    PageHome_obj1_Bindings bindings = new PageHome_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                    global::Microsoft.UI.Xaml.Markup.XamlBindingHelper.SetDataTemplateComponent(element1, bindings);
                }
                break;
            }
            return returnValue;
        }
    }
}

