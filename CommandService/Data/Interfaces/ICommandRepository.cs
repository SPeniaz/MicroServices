using CommandService.Models;

namespace CommandService.Data.Interfaces
{
    public interface ICommandRepository
    {
        bool SaveChanges();

        //Related to Platforms
        IEnumerable<Platform> GetAllPlatforms();

        void CreatePlatform(Platform platform);

        bool IsPlatformExists(int platformId);

        //Commands
        IEnumerable<Command> GetForPlatform(int platformId);

        Command? GetForPlatformAndId(int platformId, int commandId);

        void CreateForPlatform(int platformId, Command command);
    }
}