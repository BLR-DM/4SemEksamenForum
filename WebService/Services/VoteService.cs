using WebService.Dtos.CommandDtos;
using WebService.Proxies;

namespace WebService.Services
{
    public class VoteService : IVoteService
    {
        private readonly IVoteServiceProxy _proxy;

        public VoteService(IVoteServiceProxy proxy)
        {
            _proxy = proxy;
        }

        async Task IVoteService.HandlePostVote(HandleVoteDto dto, int postId)
        {
            try
            {
                await _proxy.HandlePostVote(dto, postId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        async Task IVoteService.HandleCommentVote(HandleVoteDto dto, int commentId)
        {
            try
            {
                await _proxy.HandleCommentVote(dto, commentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }

    public interface IVoteService
    {
        Task HandlePostVote(HandleVoteDto dto, int postId);
        Task HandleCommentVote(HandleVoteDto dto, int commentId);
    }
}