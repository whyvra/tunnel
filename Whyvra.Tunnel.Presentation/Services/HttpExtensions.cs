using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Presentation.Configuration;

namespace Whyvra.Tunnel.Presentation.Services
{
    public static class HttpExtensions
    {
        public static async Task EnsureSuccess(this HttpResponseMessage result)
        {
            if (result.IsSuccessStatusCode) return;

            var api = await result.Content.ReadFromJsonAsync<ApiMessage>();

            var message = api.InnerException == null
                ? $"{api.StatusCode} {api.Status} - {api.Message}"
                : $"{api.StatusCode} {api.Status} - {api.Message} ↳ {api.InnerException}";

            var formatted = api.InnerException == null
                ? $"<p><strong>{api.StatusCode} {api.Status}</strong> - {api.Message}</p>"
                : $"<p><strong>{api.StatusCode} {api.Status}</strong> - {api.Message} <br> ↳ {api.InnerException}</p>";

            throw new TunnelException(message, formatted, api);
        }

        public static async Task<T> GetFromJsonAsyncWithSuccess<T>(this HttpClient http, string url)
        {
            var result = await http.GetAsync(url);
            await result.EnsureSuccess();

            return await result.Content.ReadFromJsonAsync<T>();
        }
    }
}