namespace ContentService.Infrastructure.Helpers
{
    public static class ContentIdHelper
    {
        public static string FormatPostId(int forumId, int postId) => $"Post:{forumId}-{postId}";
        public static string FormatForumId(int forumId) => $"Forum:{forumId}";

        public static (string Type, int[] Ids) Parse(string contentId)
        {
            var split = contentId.Split(':');
            var ids = split[1].Split('-').Select(int.Parse).ToArray();
            return (split[0], ids);
        }
    }

}