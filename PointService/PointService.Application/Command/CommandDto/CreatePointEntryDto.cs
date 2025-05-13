namespace PointService.Application.Command.CommandDto
{
    public record CreatePointEntryDto
    {
        public string PointActionId { get; set; }
        public int SourceId { get; set; }
        public string SourceType { get; set; }
        public int ContextId { get; set; }
        public string ContextType { get; set; }
    }
}