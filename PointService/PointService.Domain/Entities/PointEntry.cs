namespace PointService.Domain.Entities
{
    public class PointEntry
    {
        public int Id { get; protected set; }
        public string UserId { get; protected set; }
        public PointAction PointAction { get; protected set; }
        public string PointActionId { get; protected set; }
        public int Points { get; protected set; }
        public int SourceId { get; protected set; }
        public string SourceType { get; protected set; }
        public int ContextId { get; protected set; }
        public string ContextType { get; protected set; }
        public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow.AddHours(2);

        protected PointEntry() { }
        private PointEntry(string userId, string pointAction, int points, int sourceId, string sourceType, int contextId, string contextType)
        {
            UserId = userId;
            PointActionId = pointAction;
            Points = points;
            SourceId = sourceId;
            SourceType = sourceType;
            ContextId = contextId;
            ContextType = contextType;
        }

        public static PointEntry Create(string userId, string pointAction, int points, int sourceId, string sourceType,
            int contextId, string contextType)
        {
            return new PointEntry(userId, pointAction, points, sourceId, sourceType, contextId, contextType);
        }
    }
}