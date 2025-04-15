using PointService.Application.Command.CommandDto;
using PointService.Application.Repositories;
using PointService.Domain.Entities;

namespace PointService.Application.Command
{
    public class PointActionCommand : IPointActionCommand
    {
        private readonly IPointActionRepository _pointActionRepository;

        public PointActionCommand(IPointActionRepository pointActionRepository)
        {
            _pointActionRepository = pointActionRepository;
        }

        async Task IPointActionCommand.CreatePointActionAsync(CreatePointActionDto createPointActionDto)
        {
            var pointAction = PointAction.Create(createPointActionDto.Action, createPointActionDto.Points);

            await _pointActionRepository.AddAsync(pointAction);
        }
    }

    public interface IPointActionCommand
    {
        Task CreatePointActionAsync(CreatePointActionDto createPointActionDto);
    }
}