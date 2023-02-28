using System.Text.Json;
using AutoMapper;
using CommandService.Data.Interfaces;
using CommandService.Dtos;
using CommandService.EventProcessing.Interfaces;
using CommandService.Models;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(
            IServiceScopeFactory serviceScopeFactory,
            IMapper mapper
        )
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }
        public void Process(string message)
        {
            var eventType = Determine(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    break;
                default:
                    break;
            }
        }

        private void Add(string platformPublishedMessage)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepository>();

                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishedDto);
                    if (repo.IsExternalPlatformExists(plat.ExternalId) == false)
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                    }
                    else
                    {
                        System.Console.WriteLine("--> Platform already exists.");
                    }
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine($"--> could not add platform to DB: {ex.Message}");

                }
            }
        }

        private EventType Determine(string notificationMessage)
        {
            System.Console.WriteLine("--> Determining Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType!.Event)
            {
                case "Platform_Published":
                    System.Console.WriteLine("--> Platform_Published Event detected.");
                    return EventType.PlatformPublished;
                default:
                    System.Console.WriteLine("--> Could not determine event type");
                    return EventType.Undetermined;
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}