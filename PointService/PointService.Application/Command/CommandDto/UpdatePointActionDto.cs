using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointService.Application.Command.CommandDto
{
    public record UpdatePointActionDto(string PointActionId, int NewPoints);
}
