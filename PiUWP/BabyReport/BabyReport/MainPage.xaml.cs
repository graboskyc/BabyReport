using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.Devices.Gpio;
using System.Diagnostics;

namespace BabyReport
{
    public sealed partial class MainPage : Page
    {
        DispatcherTimer _refTimer;
        DispatcherTimer _debounceTimer;
        bool _canPress= true;

        GpioController _gpio = null;
        WebRequestHandler _wrh;

        public MainPage()
        {
            this.InitializeComponent();

            _wrh = new WebRequestHandler();

            var view = ApplicationView.GetForCurrentView();

            view.TitleBar.BackgroundColor = Color.FromArgb(255, 81, 191, 8);
            view.TitleBar.ButtonBackgroundColor = Color.FromArgb(255, 81, 191, 8);
            view.TitleBar.ButtonForegroundColor = Colors.White;
            view.TitleBar.ButtonPressedForegroundColor = Color.FromArgb(255, 81, 191, 8);
            view.TitleBar.ButtonPressedBackgroundColor = Colors.White;
            view.TitleBar.ButtonHoverBackgroundColor = Colors.White;
            view.TitleBar.ButtonHoverForegroundColor = Color.FromArgb(255, 81, 191, 8);

            view_web.Source = new Uri(AppConstants.chartsEmbed);

            _refTimer = new DispatcherTimer();
            _refTimer.Tick += _refTimer_Tick;
            _refTimer.Interval = new TimeSpan(0, 0, AppConstants.refreshSecDelay);
            _refTimer.Start();

            _gpio = GpioController.GetDefault();

            if (_gpio != null)
            {
                GpioPin hotPin = _gpio.OpenPin(AppConstants.pinAlwaysOn);

                hotPin.Write(GpioPinValue.High);
                hotPin.SetDriveMode(GpioPinDriveMode.Output);
                hotPin.Write(GpioPinValue.High);

                foreach(int i in AppConstants.pinList)
                {
                    try
                    {
                        GpioController gc = GpioController.GetDefault();
                        GpioPin pin = gc.OpenPin(i);
                        pin.Write(GpioPinValue.Low);
                        pin.SetDriveMode(GpioPinDriveMode.Input);
                        pin.DebounceTimeout = TimeSpan.FromMilliseconds(500);
                        pin.ValueChanged += asyncGenericPinValueChanged;
                    } catch (Exception ex)
                    {
                        Debug.WriteLine("Pin " + i.ToString() + " not supported");
                    }
                }

            }
        }

        private async void asyncGenericPinValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            Debug.WriteLine(sender.PinNumber.ToString() + " " + sender.Read().ToString());

            if (args.Edge == GpioPinEdge.RisingEdge)
            {
                int index = Array.IndexOf(AppConstants.pinList, sender.PinNumber);
                Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();

                switch (index)
                {
                    case 0:
                        jobj.Add("eventType", "diaper");
                        jobj.Add("eventSubType", "pee");
                        await _wrh.MakeRequest(jobj);
                        break;
                    case 1:
                        jobj.Add("eventType", "diaper");
                        jobj.Add("eventSubType", "poo");
                        await _wrh.MakeRequest(jobj);
                        break;
                    case 2:
                        jobj.Add("eventType", "bottle");
                        jobj.Add("eventSubType", "milk");
                        await _wrh.MakeRequest(jobj);
                        break;
                    case 3:
                        jobj.Add("eventType", "bottle");
                        jobj.Add("eventSubType", "formula");
                        await _wrh.MakeRequest(jobj);   
                        break;
                }
            }

        }

        private void _refTimer_Tick(object sender, object e)
        {
            view_web.Refresh();
            _refTimer.Interval = new TimeSpan(0, 0, AppConstants.refreshSecDelay);
            _refTimer.Start();
            view_web.Refresh();
        }

        private async void view_web_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            await view_web.InvokeScriptAsync("setBody", new string[] { AppConstants.chartsEmbed });
        }

        private async void btn_fed_Click(object sender, RoutedEventArgs e)
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "bottle");
            jobj.Add("eventSubType", "formula");
            await _wrh.MakeRequest(jobj);
            view_web.Refresh();
        }

        private async void btn_poo_Click(object sender, RoutedEventArgs e)
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "diaper");
            jobj.Add("eventSubType", "poo");
            await _wrh.MakeRequest(jobj);
            view_web.Refresh();
        }

        private async void btn_pee_Click(object sender, RoutedEventArgs e)
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "diaper");
            jobj.Add("eventSubType", "pee");
            await _wrh.MakeRequest(jobj);
            view_web.Refresh();
        }

        private async void btn_milk_Click(object sender, RoutedEventArgs e)
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "bottle");
            jobj.Add("eventSubType", "milk");
            await _wrh.MakeRequest(jobj);
            view_web.Refresh();
        }
    }
}
