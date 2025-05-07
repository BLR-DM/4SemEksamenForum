using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.JSInterop;
using Blazored.SessionStorage;
using System.Text.Json;

namespace WebService.Services
{
    public class UserSessionService
    {
        private readonly ISessionStorageService _sessionStorage;

        private JsonElement? _cachedUser;

        public UserSessionService(ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        private async Task<JsonElement?> GetUserObjectAsync()
        {
            if (_cachedUser != null)
                return _cachedUser;

            var raw = await _sessionStorage.GetItemAsStringAsync("oidc.user:https://keycloak.blrforum.dk/realms/4SemForumProjekt:webservice-client");
            if (string.IsNullOrEmpty(raw))
                return null;

            var doc = JsonDocument.Parse(raw);
            _cachedUser = doc.RootElement.Clone(); // Clone fordi JsonDocument bliver disposed
            return _cachedUser;
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var user = await GetUserObjectAsync();
            return user != null;
        }

        public async Task<string?> GetUsernameAsync()
        {
            var user = await GetUserObjectAsync();
            return user?.GetProperty("profile").GetProperty("preferred_username").GetString();
        }

        public async Task<string?> GetUserIdAsync()
        {
            var user = await GetUserObjectAsync();
            return user?.GetProperty("profile").GetProperty("sub").GetString();
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            var user = await GetUserObjectAsync();
            return user?.GetProperty("access_token").GetString();
        }

        public async Task<string?> GetIdTokenAsync()
        {
            var user = await GetUserObjectAsync();
            return user?.GetProperty("id_token").GetString();
        }
    }
}
