using FabricWCF.Common.Objects;
using Microsoft.Win32;
using Reader.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Reader.UI
{
    /// <summary>
    /// Interaction logic for LandingPage.xaml
    /// </summary>
    public partial class LandingPage : Window
    {
        LandingPageViewModel viewModel;

        static LandingPage()
        {
            var appName = Process.GetCurrentProcess().ProcessName + ".exe";
            SetIE8KeyforWebBrowserControl(appName);
        }

        public LandingPage()
        {
            InitializeComponent();
            this.DataContext = viewModel = new LandingPageViewModel();

            this.Loaded += LandingPage_Loaded;
            this.Closing += LandingPage_Closing;
            slider1.MouseDoubleClick += new MouseButtonEventHandler(RestoreScalingFactor);
        }
        void RestoreScalingFactor(object sender, MouseButtonEventArgs args)
        {

            ((Slider)sender).Value = 1.0;
        }
        private void LandingPage_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Closing -= LandingPage_Closing;

            if (viewModel?.SelectedLocale != null)
            {
                var userLocale = viewModel.SelectedLocale.RegionCode;
                var userLang = viewModel.SelectedLocale.LangCode;

                Properties.Settings.Default.userLocale = userLocale;
                Properties.Settings.Default.userLang = userLang;
                Properties.Settings.Default.Save();
            }
        }

        private void LandingPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= LandingPage_Loaded;
            var userLocale = Properties.Settings.Default.userLocale;
            var userLang = Properties.Settings.Default.userLang;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(this.RefreshSampleData);
            timer.Interval = new TimeSpan(0, 15, 0);
            timer.Start();

            viewModel.SelectedCategory = NewsCategory.Headlines;
            viewModel.SelectedLocale = LocaleInfo.Locations.FirstOrDefault(li => li.RegionCode == userLocale && li.LangCode == userLang);

            viewModel.LoadItems(NewsCategory.Headlines);


        }

        private void RefreshSampleData(object state, EventArgs e)
        {
            if (viewModel.AutoRefresh)
                viewModel.UpdateFeedCommand.Execute(cbCategory.SelectedItem);
        }

        private void wb_LoadCompleted(object sender, NavigationEventArgs e)
        {
            string script = @"document.body.style.overflow = 'auto'; 
                              document.body.style.fontFamily = 'Lato, Roboto, serif'; 
                              document.body.style.wordWrap = 'break-word'";
            WebBrowser wb = (WebBrowser)sender;
            browserPane.Header = ((dynamic)wb.Document).Title;
            wb.InvokeScript("execScript", new object[] { script, "JavaScript" });
            viewModel.IsBusy = false;
        }

        private static bool SetIE8KeyforWebBrowserControl(string appName)
        {
            RegistryKey Regkey = null;
            try
            {
                if (Environment.Is64BitOperatingSystem)
                    Regkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", true);
                else  //For 32 bit machine
                    Regkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);

                string FindAppkey = string.Empty;
                if (Regkey != null)
                {
                    FindAppkey = Convert.ToString(Regkey.GetValue(appName));

                    if (string.IsNullOrEmpty(FindAppkey))
                        Regkey.SetValue(appName, unchecked((int)0x2AF8), RegistryValueKind.DWord);

                    FindAppkey = Convert.ToString(Regkey.GetValue(appName));
                }
                return (FindAppkey == "11000");
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (Regkey != null) Regkey.Close();
            }
        }

        private void slider1_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            if (Keyboard.IsKeyDown(Key.LeftCtrl) ||

                Keyboard.IsKeyDown(Key.RightCtrl))

            {
                if (e.MiddleButton == MouseButtonState.Pressed)
                {

                    RestoreScalingFactor(slider1, e);

                }
            }
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var scaler = ListView.LayoutTransform as ScaleTransform;

            if (scaler == null)
            {
                ListView.LayoutTransform = new ScaleTransform(slider1.Value, slider1.Value);
            }
            else if (scaler.HasAnimatedProperties)
            {
            }
            else
            {
                scaler.ScaleX = slider1.Value;
                scaler.ScaleY = slider1.Value;
            }
        }

        private void slider1_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                slider1.Value += (e.Delta > 0) ? 0.1 : -0.1;
            }
        }


    }
}
