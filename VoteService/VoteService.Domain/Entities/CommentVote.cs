using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteService.Domain.Entities
{
    public class CommentVote
    {
        public string UserId { get; protected set; }
        public string CommentId { get; protected set; }
        public bool VoteType { get; protected set; }
        public DateTime VotedAt { get; protected set; } = DateTime.UtcNow.AddHours(2);

        protected CommentVote() { }

        private CommentVote(string userId, string commentId, bool voteType)
        {
            UserId = userId;
            CommentId = commentId;
            VoteType = voteType;
        }

        public static CommentVote Create(string userId, string commentId, bool voteType)
        {
            return new CommentVote(userId, commentId, voteType);
        }

        public void Update(bool voteType)
        {
            VoteType = voteType;
            VotedAt = DateTime.UtcNow.AddHours(2);
        }

    }
}
