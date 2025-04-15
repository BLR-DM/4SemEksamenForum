using PointService.Application.Command.CommandDto;
using PointService.Application.Repositories;
using PointService.Domain.Entities;

namespace PointService.Application.Command
{
    public class PointEntryCommand : IPointEntryCommand
    {
        private readonly IPointEntryRepository _pointEntryRepository;
        private readonly IPointActionRepository _pointActionRepository;

        public PointEntryCommand(IPointEntryRepository pointEntryRepository, IPointActionRepository pointActionRepository)
        {
            _pointEntryRepository = pointEntryRepository;
            _pointActionRepository = pointActionRepository;
        }

        async Task IPointEntryCommand.CreatePointEntryAsync(CreatePointEntryDto createPointEntry, string userId)
        {
            var pointAction = await _pointActionRepository.GetAsync(createPointEntry.PointActionId);

            var pointEntry = PointEntry.Create(userId, createPointEntry.PointActionId, pointAction.Points, createPointEntry.SourceId, 
                createPointEntry.SourceType, createPointEntry.ContextId, createPointEntry.ContextType);

            await _pointEntryRepository.AddAsync(pointEntry);
        }
    }

    public interface IPointEntryCommand
    {
        Task CreatePointEntryAsync(CreatePointEntryDto createPointEntry, string userId);
    }
}