namespace ContentService.Infrastructure.Interfaces
{
    public interface IModerationResultHandler
    {
        Task HandleModerationResultAsync(ContentModeratedDto dto);
    }
}