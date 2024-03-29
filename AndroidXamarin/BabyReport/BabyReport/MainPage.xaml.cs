﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace BabyReport
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            view_web.Source = AppConstants.chartsEmbed;

            txt_title.GestureRecognizers.Add(new TapGestureRecognizer((arg1, arg2) => view_web.Source = (view_web.Source as UrlWebViewSource).Url ));
            btn_poo.GestureRecognizers.Add(new TapGestureRecognizer(btn_poo_Click));
            btn_pee.GestureRecognizers.Add(new TapGestureRecognizer(btn_pee_Click));
            btn_form.GestureRecognizers.Add(new TapGestureRecognizer(openFormula));
            btn_milk.GestureRecognizers.Add(new TapGestureRecognizer(openMilk));


        }

        private async Task MakeRequest(Newtonsoft.Json.Linq.JObject jobj)
        {
            stk_form.IsVisible = false;
            stk_milk.IsVisible = false;
            view_web.IsVisible = true;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(AppConstants.newEventEndpoint);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string url = "newEvent?secret=" + AppConstants.eventEndpointSecret;

            StringContent sc = new StringContent(jobj.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, sc);

            response.EnsureSuccessStatusCode();

            string r = await response.Content.ReadAsStringAsync();
            view_web.Source = (view_web.Source as UrlWebViewSource).Url;
        }

        private void openFormula(View arg1, object arg2)
        {
            stk_form.IsVisible = true;
            stk_milk.IsVisible = false;
            view_web.IsVisible = false;
        }

        private void openMilk(View arg1, object arg2)
        {
            stk_form.IsVisible = false;
            stk_milk.IsVisible = true;
            view_web.IsVisible = false;
        }

        private async void btnSaveMilk_Clicked(object sender, System.EventArgs e)
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "bottle");
            jobj.Add("eventSubType", "milk");
            double val = Convert.ToDouble(txt_milk.Text);
            jobj.Add("qty", val);
            await MakeRequest(jobj);
        }

        private async void btnSaveFormula_Clicked(object sender, System.EventArgs e)
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "bottle");
            jobj.Add("eventSubType", "formula");
            double val = Convert.ToDouble(txt_form.Text);
            jobj.Add("qty", val);
            await MakeRequest(jobj);
        }

        private async void btn_pee_Click(View arg1, object arg2)
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "diaper");
            jobj.Add("eventSubType", "pee");
            await MakeRequest(jobj);
        }

        private async void btn_poo_Click(View arg1, object arg2)
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "diaper");
            jobj.Add("eventSubType", "poo");
            await MakeRequest(jobj);
        }

    }
}
