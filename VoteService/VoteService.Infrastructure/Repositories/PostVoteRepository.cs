using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoteService.Domain.Entities;
using VoteService.Domain.Interfaces;

namespace VoteService.Infrastructure.Repositories
{
    public class PostVoteRepository : IPostVoteRepository
    {
        private readonly VoteContext _context;

        public PostVoteRepository(VoteContext context)
        {
            _context = context;
        }

        async Task IPostVoteRepository.AddPostVoteAsync(PostVote postVote)
        {
            _context.PostVotes.Add(postVote);
            await _context.SaveChangesAsync();
        }

        async Task IPostVoteRepository.DeletePostVoteAsync(PostVote postVote)
        {
            _context.PostVotes.Remove(postVote);
            await _context.SaveChangesAsync();
        }

        async Task<PostVote?> IPostVoteRepository.GetVoteByUserIdAsync(string userId, string postId)
        {
            //Null tjek sker i application layer (Skal bruges ift. ToggleVote metoden til at styre votes)
            return await _context.PostVotes.FirstOrDefaultAsync(v => v.UserId == userId && v.PostId == postId);
        }

        async Task IPostVoteRepository.UpdateVoteAsync(PostVote postVote)
        { 
            _context.PostVotes.Update(postVote);
            await _context.SaveChangesAsync();
        }
    }
}
