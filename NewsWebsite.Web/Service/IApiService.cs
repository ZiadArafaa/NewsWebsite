namespace NewsWebsite.Web.Service
{
    public interface IApiService
    {
        public HttpClient CreateHttpClient(HttpClient _httpClient);
        public T ConvertToObject<T>(HttpResponseMessage Response) where T : class;
        public List<T> ConvertToList<T>(HttpResponseMessage Response) where T : class;
        public StringContent ConvertToJson(object model);
    }
}
