namespace ContentService.Application.Services
{
    public interface IModerationResultHandler
    {
        Task HandleModerationResultAsync(ContentModeratedDto dto);
    }
}