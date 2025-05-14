using NotificationService.Application.Commands.CommandDto;
using NotificationService.Application.Commands.Interfaces;
using NotificationService.Application.Repositories;
using NotificationService.Domain.Entities;

namespace NotificationService.Application.Commands;

public class SentNotificationCommand : ISentNotificationCommand
{
    private readonly ISentNotificationRepository _repository;

    public SentNotificationCommand(ISentNotificationRepository repository)
    {
        _repository = repository;
    }
    async Task ISentNotificationCommand.CreateSentNotificationAsync(CreateSentNotificationDto dto)
    {
        try
        {
            var sentNotification = SentNotification.Create(dto.NotificationId, dto.UserId);
            await _repository.AddAsync(sentNotification);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

