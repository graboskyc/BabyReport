using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace BabyReport
{
    public class StitchCalls
    {
        public StitchCalls()
        {
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
        }

        public async void didFormula()
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "bottle");
            jobj.Add("eventSubType", "formula");
            await MakeRequest(jobj);
        }

        public async void didPoo()
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "diaper");
            jobj.Add("eventSubType", "poo");
            await MakeRequest(jobj);
        }

        public async void didPee()
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "diaper");
            jobj.Add("eventSubType", "pee");
            await MakeRequest(jobj);
        }

        public async void didMilk()
        {
            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("eventType", "bottle");
            jobj.Add("eventSubType", "milk");
            await MakeRequest(jobj);
        }
    }
}
