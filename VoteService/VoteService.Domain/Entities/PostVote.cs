using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteService.Domain.Entities
{
    public class PostVote
    {
        public string UserId { get; protected set; }
        public int PostId { get; protected set; }
        public bool VoteType { get; protected set; }
        public DateTime VotedAt { get; protected set; } = DateTime.UtcNow.AddHours(2);

        protected PostVote() {}

        private PostVote(string userId, int postId, bool voteType)
        {
            UserId = userId;
            PostId = postId;
            VoteType = voteType;
        }

        public static PostVote Create(string userId, int postId, bool voteType)
        {
            return new PostVote(userId, postId, voteType);
        }

        public void Update(bool voteType)
        {
            VoteType = voteType;
            VotedAt = DateTime.UtcNow.AddHours(2);
        }

    }
}
