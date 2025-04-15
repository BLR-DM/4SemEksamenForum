namespace PointService.Domain.Entities
{
    public class PointEntry
    {
        public int Id { get; protected set; }
        public string UserId { get; protected set; }
        public PointAction PointAction { get; protected set; }
        public int PointActionId { get; protected set; }
        public int SourceId { get; protected set; }
        public string SourceType { get; protected set; }
        public int ContextId { get; protected set; }
        public string ContextType { get; protected set; }

        protected PointEntry() { }
        private PointEntry(string userId, int pointAction, int sourceId, string sourceType, int contextId, string contextType)
        {
            UserId = userId;
            PointActionId = pointAction;
            SourceId = sourceId;
            SourceType = sourceType;
            ContextId = contextId;
            ContextType = contextType;
        }

        public static PointEntry Create(string userId, int pointAction, int sourceId, string sourceType,
            int contextId, string contextType)
        {
            return new PointEntry(userId, pointAction, sourceId, sourceType, contextId, contextType);
        }
    }
}