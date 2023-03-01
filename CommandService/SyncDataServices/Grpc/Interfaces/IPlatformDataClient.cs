using CommandService.Models;

namespace CommandService.SyncDataServices.Grpc.Interfaces
{
    public interface IPlatformDataClient
    {
        IEnumerable<Platform> ReturnAllPlatforms();
    }
}