using ContentService.Application.Commands.CommandDto.CommentDto;
using ContentService.Application.Commands.CommandDto.ForumDto;
using ContentService.Application.Commands.CommandDto.PostDto;
using ContentService.Application.Commands.Interfaces;
using ContentService.Application.Helpers;

namespace ContentService.Application.Services
{
    public class ModerationResultHandler : IModerationResultHandler
    {
        private readonly IForumCommand _forumCommand;
        private readonly IPostCommand _postCommand;

        public ModerationResultHandler(IForumCommand forumCommand, IPostCommand postCommand)
        {
            _forumCommand = forumCommand;
            _postCommand = postCommand;
        }

        public async Task HandleModerationResultAsync(ContentModeratedDto dto)
        {
            var (contentType, ids) = ContentIdFormatter.Parse(dto.ContentId);

            // Ex: "Comment:2-5-10" = forumId: 2, postId: 5, commentId: 10
            var forumId = ids.Length > 0 ? ids[0] : 0;
            var postId = ids.Length > 1 ? ids[1] : 0;
            var commentId = ids.Length > 2 ? ids[2] : 0;

            switch (contentType)
            {
                case "Forum":
                    if (dto.Result == ModerationResult.Accept)
                        await _forumCommand.HandleForumApprovalAsync(new PublishForumDto(forumId));
                    else if (dto.Result == ModerationResult.Reject)
                        await _forumCommand.HandleForumRejectionAsync(new RejectForumDto(forumId));
                    break;

                case "Post":
                    if (dto.Result == ModerationResult.Accept)
                        await _forumCommand.HandlePostApprovalAsync(new PublishPostDto(forumId, postId));
                    else if (dto.Result == ModerationResult.Reject)
                        await _forumCommand.HandlePostRejectionAsync(new RejectPostDto(forumId, postId));
                    break;

                case "Comment":
                    if (dto.Result == ModerationResult.Accept)
                        await _postCommand.HandleCommentApprovalAsync(new PublishCommentDto(forumId, postId, commentId));
                    else if (dto.Result == ModerationResult.Reject)
                        await _postCommand.HandleCommentRejectionAsync(new RejectCommentDto(forumId, postId, commentId));
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported content type: {contentType}");
            }
        }
    }

    public record ContentModeratedDto(string ContentId, ModerationResult Result);

    public enum ModerationResult
    {
        Accept,
        Reject
    }
}