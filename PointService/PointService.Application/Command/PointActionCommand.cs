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
            try
            {
                var pointAction = PointAction.Create(createPointActionDto.Action, createPointActionDto.Points);

                await _pointActionRepository.AddAsync(pointAction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        async Task IPointActionCommand.UpdatePointActionAsync(UpdatePointActionDto updatePointActionDto)
        {
            try
            {
                var pointAction = await _pointActionRepository.GetAsync(updatePointActionDto.PointActionId);

                pointAction.UpdatePoints(updatePointActionDto.NewPoints);

                await _pointActionRepository.UpdateAsync(pointAction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
    }

    public interface IPointActionCommand
    {
        Task CreatePointActionAsync(CreatePointActionDto createPointActionDto);
        Task UpdatePointActionAsync(UpdatePointActionDto updatePointActionDto);
    }
}