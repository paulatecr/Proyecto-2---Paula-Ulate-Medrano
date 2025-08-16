using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class ApiClient : IDisposable
{
    private readonly HttpClient _http;

    public ApiClient()
    {
        _http = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44384/")//Ojo, siempre usa mismo puerto que API
        };
    }

    public async Task<T> GetAsync<T>(string uri)
    {
        var response = await _http.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json);
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string uri, T data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _http.PostAsync(uri, content);
    }

    public async Task<HttpResponseMessage> PutAsync<T>(string uri, T data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _http.PutAsync(uri, content);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string uri)
    {
        return await _http.DeleteAsync(uri);
    }

    public void Dispose()
    {
        _http?.Dispose();
    }
}
