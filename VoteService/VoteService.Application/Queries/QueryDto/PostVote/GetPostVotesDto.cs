namespace VoteService.Application.Queries.QueryDto.PostVote;

public class GetPostVotesDto
{
    public int PostId { get; set; }
    public List<PostVoteDto> PostVotes { get; set; }
}