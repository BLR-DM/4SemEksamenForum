using System.Net.Http.Json;
using WebService.Dtos.CommandDtos;

namespace WebService.Proxies
{
    public class VoteServiceProxy : IVoteServiceProxy
    {
        private readonly HttpClient _httpClient;

        public VoteServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        async Task IVoteServiceProxy.HandlePostVote(HandleVoteDto dto, int postId)
        {
            try
            {
                var uri = $"api/vote/Post/{postId}/Vote";

                var response = await _httpClient.PostAsJsonAsync(uri, dto);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to handle vote");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Failed to handle vote");
            }
        }
        
        async Task IVoteServiceProxy.HandleCommentVote(HandleVoteDto dto, int commentId)
        {
            try
            {
                var uri = $"api/vote/Comment/{commentId}/Vote";

                var response = await _httpClient.PostAsJsonAsync(uri, dto);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to handle vote");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Failed to handle vote");
            }
        }
    }

    public interface IVoteServiceProxy
    {
        Task HandlePostVote(HandleVoteDto dto, int postId);
        Task HandleCommentVote(HandleVoteDto dto, int commentId);
    }
}