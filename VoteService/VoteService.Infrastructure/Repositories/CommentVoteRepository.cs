using Microsoft.EntityFrameworkCore;
using VoteService.Domain.Entities;
using VoteService.Domain.Interfaces;

namespace VoteService.Infrastructure.Repositories;

public class CommentVoteRepository : ICommentVoteRepository
{
    private readonly VoteContext _context;

    public CommentVoteRepository(VoteContext context)
    {
        _context = context;
    }
    async Task ICommentVoteRepository.AddCommentVoteAsync(CommentVote commentVote)
    {
        _context.CommentVotes.Add(commentVote);
        await _context.SaveChangesAsync();
    }

    async Task ICommentVoteRepository.DeleteCommentVoteAsync(CommentVote commentVote)
    {
        _context.CommentVotes.Remove(commentVote);
        await _context.SaveChangesAsync();
    }

    async Task<CommentVote?> ICommentVoteRepository.GetVoteByUserIdAsync(string userId, string commentId)
    {
        //Null tjek sker i application layer (Skal bruges ift. ToggleVote metoden til at styre votes)
        return await _context.CommentVotes.FirstOrDefaultAsync(v => v.UserId == userId && v.CommentId == commentId);
    }

    async Task ICommentVoteRepository.UpdateVoteAsync(CommentVote commentVote)
    {
        _context.CommentVotes.Update(commentVote);
        await _context.SaveChangesAsync();
    }
}