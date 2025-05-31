using System.Data;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Newtonsoft.Json;
using WebService.Components;
using WebService.Layout;
using WebService.Views;

namespace WebService.Services
{
    public class UserSessionService
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IAccessTokenProvider _tokenProvider;
        private readonly IPointService _pointService;
        private readonly INotificationService _notificationService;
        private readonly IForumService _forumService;
        private TaskCompletionSource _readyTcs = new();

        public bool IsLoggedIn { get; private set; }
        public bool IsInitialized { get; set; }
        public string? UserId { get; private set; }
        public string? Username { get; private set; }
        public string? Email { get; private set; }
        public string? Token { get; private set; }
        public string? Name { get; private set; }
        public int? Points { get; private set; }
        public List<string> Roles { get; private set; } = [];
        public List<int> SubscribedForumIds { get; set; } = [];

        public List<ForumView>? SubscribedForums { get; set; } = [];

        public List<NotificationView>? Notifications { get; set; } = [];
        public List<ForumViewWithPostIds>? Forums { get; set; }

        public event Action? OnSubscriptionsChanged;

        public UserSessionService(AuthenticationStateProvider authStateProvider, ISubscriptionService subscriptionService,
            IAccessTokenProvider tokenProvider, IPointService pointService, INotificationService notificationService, IForumService forumService)
        {
            _authStateProvider = authStateProvider;
            _subscriptionService = subscriptionService;
            _tokenProvider = tokenProvider;
            _pointService = pointService;
            _notificationService = notificationService;
            _forumService = forumService;
        }

        public async Task InitializeAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;


            if (user.Identity is { IsAuthenticated: true })
            {
                IsLoggedIn = true;

                var tokenResult = await _tokenProvider.RequestAccessToken();

                if (tokenResult.Status != AccessTokenResultStatus.Success)
                {

                    // Need to redirect to login / force relogin
                    return;

                }

                if (tokenResult.TryGetToken(out var token))
                {
                    Token = token.Value;
                }

                UserId = user.FindFirst(c => c.Type == "sub")?.Value;
                Username = user.FindFirst(c => c.Type == "preferred_username")?.Value;
                Email = user.FindFirst(c => c.Type == "email")?.Value;
                Name = user.FindFirst(c => c.Type == "name")?.Value;

                if(UserId != null)
                    try
                    {
                        var pointsTask = _pointService.GetPointsByUserId(UserId);
                        var notificationsTask = _notificationService.GetNotificationsByUserId(UserId);
                        var forumsTask = _forumService.GetForumsWithPostsIds();

                        await Task.WhenAll(pointsTask, notificationsTask, forumsTask);

                        Points = pointsTask.Result;
                        Notifications = notificationsTask.Result.OrderByDescending(n => n.CreatedAt).ToList();
                        Forums = forumsTask.Result.OrderBy(f => f.ForumName).ToList();
                        IsInitialized = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Points = -1;
                    }

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
