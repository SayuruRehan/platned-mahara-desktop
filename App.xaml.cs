using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;
using Windows.Graphics.Display;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.



namespace PlatnedMahara
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }



        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();            
        }



        private Window? m_window;
        /// <summary>
        /// Main application window.
        /// </summary>
        public static Window MainWindow => (Application.Current as App)?.m_window;

        // Default aspect ratio: 16:9
        private static double aspectRatio = 16.0 / 9.0;

        /// <summary>
        /// Gets the minimum width based on the aspect ratio and height.
        /// </summary>
        public static int WinMinWidth => (int)(WinMinHeight * aspectRatio);

        /// <summary>
        /// Gets the minimum height with a default fallback.
        /// </summary>
        public static int WinMinHeight
        {
            get
            {
                var currentHeight = MainWindow?.Bounds.Height ?? 720; // Default height if window is not initialized
                return (int)(currentHeight > 720 ? currentHeight : 720);
            }
        }

        /// <summary>
        /// Sets a custom aspect ratio.
        /// </summary>
        /// <param name="width">Width for the aspect ratio.</param>
        /// <param name="height">Height for the aspect ratio.</param>
        public static void SetAspectRatio(double width, double height)
        {
            if (width > 0 && height > 0)
            {
                aspectRatio = width / height;
            }
            else
            {
                throw new ArgumentException("Width and height must be positive values.");
            }
        }



    }
}
