using PaltformService.Dtos;

namespace PaltformService.SyncDataServices.Http.Interfaces
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto platform);
    }
}