using System.Data;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using WebService.Layout;
using WebService.Views;

namespace WebService.Services
{
    public class UserSessionService
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ISubscriptionService _subscriptionService;
        private TaskCompletionSource _readyTcs = new();

        public bool IsLoggedIn { get; private set; }
        public string? UserId { get; private set; }
        public string? Username { get; private set; }
        public List<string> Roles { get; private set; } = [];
        public List<int> SubscribedForumIds { get; set; } = [];

        public List<ForumView>? SubscribedForums { get; set; } = [];

        public event Action? OnSubscriptionsChanged;

        public UserSessionService(AuthenticationStateProvider authStateProvider, ISubscriptionService subscriptionService)
        {
            _authStateProvider = authStateProvider;
            _subscriptionService = subscriptionService;
        }

        public async Task InitializeAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is { IsAuthenticated: true })
            {
                IsLoggedIn = true;
                UserId = user.FindFirst(c => c.Type == "sub")?.Value;
                Username = user.FindFirst(c => c.Type == "preferred_username")?.Value;
                var rawRoles = user.FindFirst(c => c.Type == "role")?.Value;

                if(rawRoles != null)    
                    Roles = JsonConvert.DeserializeObject<List<string>>(rawRoles) ?? [];

                await GetSubscribedForumIds();
            }
            else
            {
                IsLoggedIn = false;
                UserId = null;
                Roles.Clear();
            }

            _readyTcs.TrySetResult();
        }

        private async Task GetSubscribedForumIds()
        {
            if(UserId != null)
                SubscribedForumIds = await _subscriptionService.GetSubscribedForumIds(UserId);

        }

        public void AddForumSubscription(int forumId)
        {
            SubscribedForumIds.Add(forumId);
            OnSubscriptionsChanged?.Invoke();
        }
        public void RemoveForumSubscription(int forumId)
        {
            SubscribedForumIds.Remove(forumId);
            OnSubscriptionsChanged?.Invoke();
        }



        public bool HasRole(string role) => Roles.Contains(role);

        public Task WaitUntilReadyAsync() => _readyTcs.Task;
    }
}
