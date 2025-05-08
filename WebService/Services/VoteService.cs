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

        async Task IVoteService.HandlePostVote(HandlePostVoteDto dto, int postId)
        {
            try
            {
                await _proxy.HandlePostVote(dto, postId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public interface IVoteService
    {
        Task HandlePostVote(HandlePostVoteDto dto, int postId);
    }
}