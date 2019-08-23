using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace BabyReport
{
    class WebRequestHandler
    {

        public async Task MakeRequest(Newtonsoft.Json.Linq.JObject jobj)
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
        }
    }
}
