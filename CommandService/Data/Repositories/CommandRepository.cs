using CommandService.Data.Interfaces;
using CommandService.Models;

namespace CommandService.Data.Repositories
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _context;

        public CommandRepository(AppDbContext context)
        {
            _context = context;
        }


        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
            _context.Platforms.Add(platform);
        }

        public void CreateForPlatform(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public IEnumerable<Command> GetForPlatform(int platformId)
        {
            return _context.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform!.Name);
        }

        public Command? GetForPlatformAndId(int platformId, int commandId)
        {
            return _context.Commands
                .Where(c => c.PlatformId == platformId && c.Id == commandId)
                .FirstOrDefault();
        }

        public bool IsPlatformExists(int platformId)
        {
            return _context.Platforms
                .Any(p => p.Id == platformId);
        }

        public bool IsExternalPlatformExists(int externalPlatformId)
        {
            return _context.Platforms
                .Any(p => p.ExternalId == externalPlatformId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }


    }
}