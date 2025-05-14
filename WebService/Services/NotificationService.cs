using WebService.Helpers;
using WebService.Proxies;
using WebService.Views;

namespace WebService.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationServiceProxy _proxy;

        public NotificationService(INotificationServiceProxy proxy)
        {
            _proxy = proxy;
        }
        async Task<List<NotificationView>> INotificationService.GetNotificationsByUserId(string userId)
        {
            try
            {
                var notifications = await _proxy.GetNotificationsByUserId(userId);

                if (notifications == null)
                    return new List<NotificationView>();

                var notificationViews = notifications.Select(MapDtoToView.MapNotificationToView).ToList();

                return notificationViews;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<NotificationView>();
            }
        }

        async Task INotificationService.MarkNotificationAsRead(int notificationId)
        {
            await _proxy.MarkNotificationAsRead(notificationId);
        }
    }

    public interface INotificationService
    {
        Task<List<NotificationView>> GetNotificationsByUserId(string userId);
        Task MarkNotificationAsRead(int notificationId);
    }
}