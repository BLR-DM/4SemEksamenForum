using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointService.Application.EventDto
{
    public record PostVoteEventDto(int PostId, string UserId);
    public record CommentVoteEventDto(int CommentId, string UserId);
}
