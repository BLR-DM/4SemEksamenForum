﻿namespace ContentService.Application.Services
{
    public interface IPublisherService
    {
        Task PublishEvent<T>(string topic, T Data);
    }
}