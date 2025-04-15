namespace PointService.Application.Command.CommandDto
{
    public record CreatePointEntryDto(string PointActionId, int SourceId, string SourceType, int ContextId, string ContextType);
}