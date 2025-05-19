using ContentService.Domain.Entities;

namespace ContentService.Domain.Test
{
    public class ForumTests
    {
        [Fact]
        public void CreateForum__WhenSpacesInName_Throws()
        {
            var otherForums = new List<Forum>();

            var ex = Assert.Throws<InvalidOperationException>(() =>
                Forum.Create("Test Forum", "Test content", "userId1", otherForums));

            Assert.Equal("Forum name cannot have spaces", ex.Message);
        }

        [Fact]
        public void CreateForum__WhenNameIsNotUnique_Throws()
        {
            var otherForums = new List<Forum>
            {
                Forum.Create("NameIsNotUnique", "Test content", "userId1", [])
            };

            var ex = Assert.Throws<InvalidOperationException>(() =>
                Forum.Create("nameisnotunique", "Test content", "userId2", otherForums));

            Assert.Equal("Forum name already exists", ex.Message);
        }

        [Fact]
        public void CreateForum__WhenNoSpacesInName_Creates()
        {
            Assert.IsType<Forum>(Forum.Create("NoSpacesInName", "Random content", "userId1", []));
        }

        [Fact]
        public void CreateForum__WhenForumNameIsUnique_Creates()
        {
            var otherForums = new List<Forum>
            {
                Forum.Create("UniqueForum", "Random content", "userId1", [])
            };

            Assert.IsType<Forum>(Forum.Create("NoSpacesInName", "Random content", "userId2", otherForums));
        }
    }
}
