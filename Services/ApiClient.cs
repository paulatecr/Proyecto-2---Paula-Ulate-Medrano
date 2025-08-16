using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Proyecto_2___Paula_Ulate_Medrano.Services
{
    /// <summary>
    /// Cliente HTTP para consumir Arca.Api desde MVC5.
    /// No usa EnsureSuccessStatusCode() para NO tirar excepción en 404/500.
    /// Devuelve default(T) en GET fallidos y deja que el controlador decida.
    /// </summary>
    public class ApiClient : IDisposable
    {
        private readonly HttpClient _http;

        public ApiClient()
        {
            var baseUrl = (ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "").Trim();
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("Falta configurar 'ApiBaseUrl' en Web.config (appSettings).");

            if (!baseUrl.EndsWith("/")) baseUrl += "/";

            var handler = new HttpClientHandler();

            // Aceptar certificado dev solo en localhost
            if (EsLocalhost(baseUrl))
            {
                handler.ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true;
            }

            _http = new HttpClient(handler)
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(60)
            };
            _http.DefaultRequestHeaders.Accept.Clear();
            _http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static bool EsLocalhost(string url)
        {
            try
            {
                var uri = new Uri(url);
                var host = (uri.Host ?? "").ToLowerInvariant();
                return host == "localhost" || host == "127.0.0.1";
            }
            catch { return false; }
        }

        // ---------------- GET (seguro, sin lanzar excepción) ----------------
        public async Task<T> GetAsync<T>(string relativeUrl)
        {
            // OJO: relativeUrl debe ser "api/ubicaciones", "api/semillas/1", etc.
            var resp = await _http.GetAsync(relativeUrl).ConfigureAwait(false);

            if (!resp.IsSuccessStatusCode)
            {
                // Devuelve default(T) en 404/500 para que el controlador decida qué hacer
                return default(T);
            }

            var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(json))
                return default(T);

            return JsonConvert.DeserializeObject<T>(json);
        }

        // ---------------- POST/PUT/DELETE (devuelven el HttpResponseMessage) ----------------
        public async Task<HttpResponseMessage> PostAsync<T>(string relativeUrl, T body)
        {
            var json = JsonConvert.SerializeObject(body, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            });

            using (var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json"))
            {
                return await _http.PostAsync(relativeUrl, content).ConfigureAwait(false);
            }
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string relativeUrl, T body)
        {
            var json = JsonConvert.SerializeObject(body, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            });

            using (var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json"))
            {
                return await _http.PutAsync(relativeUrl, content).ConfigureAwait(false);
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(string relativeUrl)
        {
            return await _http.DeleteAsync(relativeUrl).ConfigureAwait(false);
        }

        public void Dispose()
        {
            _http?.Dispose();
        }
    }
}