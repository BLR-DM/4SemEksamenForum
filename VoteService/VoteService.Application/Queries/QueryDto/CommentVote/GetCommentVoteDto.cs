namespace VoteService.Application.Queries.QueryDto.CommentVote;

public class GetCommentVoteDto
{
    public int CommentId { get; set; }
    public List<CommentVoteDto> CommentVotes { get; set; }
}