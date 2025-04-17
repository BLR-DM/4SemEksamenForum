using ContentService.Application.Commands.CommandDto.CommentDto;
using ContentService.Application.Commands.CommandDto.ForumDto;
using ContentService.Application.Commands.CommandDto.PostDto;
using ContentService.Application.Commands.Interfaces;
using ContentService.Application.Helpers;
using ContentService.Infrastructure.Interfaces;

namespace ContentService.Infrastructure.Helpers
{
    public class ModerationResultRouter : IModerationResultHandler
    {
        private readonly IForumCommand _forumCommand;
        private readonly IPostCommand _postCommand;

        public ModerationResultRouter(IForumCommand forumCommand, IPostCommand postCommand)
        {
            _forumCommand = forumCommand;
            _postCommand = postCommand;
        }

        public async Task HandleModerationResultAsync(ContentModeratedDto dto)
        {
            var (type, ids) = ContentIdFormatter.Parse(dto.ContentId);

            switch (type)
            {
                case "Forum":
                    if (dto.Result == ModerationResult.Accept)
                        await _forumCommand.HandleForumApprovalAsync(new PublishForumDto(ids[0]));
                    //else
                    //    await _forumCommand.HandleRejectionAsync(new RejectForumDto(ids[0]));
                    break;

                case "Post":
                    if (dto.Result == ModerationResult.Accept)
                        await _forumCommand.HandlePostApprovalAsync(new PublishPostDto(ids[0], ids[1]));
                    //else
                    //    await _postCommand.HandleRejectionAsync(new RejectPostDto(ids[0], ids[1]));
                    break;
                case "Comment":
                    if (dto.Result == ModerationResult.Accept)
                        await _postCommand.HandleCommentApprovalAsync(new PublishCommentDto(ids[0], ids[1], ids[2]));
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported content type: {type}");
            }
        }
    }
}
public record ContentModeratedDto(string ContentId, ModerationResult Result);

public enum ModerationResult
{
    Accept,
    Reject
}