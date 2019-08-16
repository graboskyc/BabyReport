﻿using System;
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
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;
using Windows.UI.ViewManagement;
using Windows.UI;

namespace BabyReport
{
    public sealed partial class MainPage : Page
    {
        DispatcherTimer _disTimer;

        public MainPage()
        {
            this.InitializeComponent();

            var view = ApplicationView.GetForCurrentView();

            view.TitleBar.BackgroundColor = Color.FromArgb(255, 81, 191, 8);
            view.TitleBar.ButtonBackgroundColor = Color.FromArgb(255, 81, 191, 8);
            view.TitleBar.ButtonForegroundColor = Colors.White;
            view.TitleBar.ButtonPressedForegroundColor = Color.FromArgb(255, 81, 191, 8);
            view.TitleBar.ButtonPressedBackgroundColor = Colors.White;
            view.TitleBar.ButtonHoverBackgroundColor = Colors.White;
            view.TitleBar.ButtonHoverForegroundColor = Color.FromArgb(255, 81, 191, 8);

            view_web.Source = new Uri(AppConstants.chartsEmbed);

            _disTimer = new DispatcherTimer();
            _disTimer.Tick += _disTimer_Tick;
            _disTimer.Interval = new TimeSpan(0, 0, AppConstants.refreshSecDelay);
            _disTimer.Start();
        }

        private void _disTimer_Tick(object sender, object e)
        {
            view_web.Refresh();
            _disTimer.Interval = new TimeSpan(0, 0, AppConstants.refreshSecDelay);
            _disTimer.Start();
            view_web.Refresh();
        }

        private async void view_web_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            await view_web.InvokeScriptAsync("setBody", new string[] { AppConstants.chartsEmbed });
        }

        private async Task MakeRequest(Newtonsoft.Json.Linq.JObject jobj)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(AppConstants.newEventEndpoint);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string url = "newEvent?secret=" + AppConstants.eventEndpointSecret;

            StringContent sc = new StringContent(jobj.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, sc);

            response.EnsureSuccessStatusCode();

            string r = await response.Content.ReadAsStringAsync();
            view_web.Refresh();
        }

        private async void btn_fed_Click(object sender, RoutedEventArgs e)
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "bottle");
            jobj.Add("eventSubType", "formula");
            await MakeRequest(jobj);
        }

        private async void btn_poo_Click(object sender, RoutedEventArgs e)
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "diaper");
            jobj.Add("eventSubType", "poo");
            await MakeRequest(jobj);
        }

        private async void btn_pee_Click(object sender, RoutedEventArgs e)
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "diaper");
            jobj.Add("eventSubType", "pee");
            await MakeRequest(jobj);
        }

        private async void btn_milk_Click(object sender, RoutedEventArgs e)
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "bottle");
            jobj.Add("eventSubType", "milk");
            await MakeRequest(jobj);
        }
    }
}
