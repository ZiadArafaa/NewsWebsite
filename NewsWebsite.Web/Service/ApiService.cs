using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace NewsWebsite.Web.Service
{
    public class ApiService : IApiService
    {
        public HttpClient CreateHttpClient(HttpClient _httpClient)
        {
            const string BaseUrl = "https://localhost:44338/";
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return _httpClient;
        }
        public T ConvertToObject<T>(HttpResponseMessage Response) where T : class
        {
            string EmpResponse1 = Response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(EmpResponse1);
        }
        public List<T> ConvertToList<T>(HttpResponseMessage Response) where T : class
        {
            string EmpResponse1 = Response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<T>>(EmpResponse1);
        }
        public StringContent ConvertToJson(object model)
        {

            return new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        }
    }
}
