using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Whyvra.Tunnel.Presentation.Configuration;

namespace Whyvra.Tunnel.Presentation.Authentication
{
    public class IdTokenProvider : IAccessTokenProvider
    {
        private readonly AuthenticationOptions _auth;
        private readonly IJSRuntime _runtime;

        public IdTokenProvider(IOptions<AuthenticationOptions> auth, IJSRuntime runtime)
        {
            _auth = auth.Value;
            _runtime = runtime;
        }

        public ValueTask<AccessTokenResult> RequestAccessToken()
        {
            var key = $"oidc.user:{_auth.Authority}:{_auth.ClientId}";
            var token = GetIdTokenAsync(key);

            return new ValueTask<AccessTokenResult>(token);
        }

        public async Task<AccessTokenResult> GetIdTokenAsync(string key)
        {
            var value = await _runtime.InvokeAsync<string>("sessionStorage.getItem", key);
            var data = JsonSerializer.Deserialize<UserData>(value);
            var token = new AccessToken
            {
                Expires = DateTimeOffset.FromUnixTimeSeconds(data.ExpiresAt),
                GrantedScopes = data.Scope.Split(" "),
                Value = data.IdToken
            };

            return new AccessTokenResult(AccessTokenResultStatus.Success, token, null);
        }

        public ValueTask<AccessTokenResult> RequestAccessToken(AccessTokenRequestOptions options)
        {
            return RequestAccessToken();
        }
    }

    internal class UserData
    {
        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }

        [JsonPropertyName("expires_at")]
        public int ExpiresAt { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}