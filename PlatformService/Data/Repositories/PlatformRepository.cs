using PaltformService.Data.Interfaces;
using PaltformService.Models;

namespace PaltformService.Data.Repositories
{
    public class PlatformRepository : IPlatformRepository
    {
        private AppDbContext _dbContext;

        public PlatformRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Platform platform)
        {
            if(platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
            
            _dbContext.Add(platform);
        }

        public Platform? Get(int id)
        {
            return _dbContext.Platforms.FirstOrDefault(p =>p.Id == id);
        }

        public IEnumerable<Platform> GetAll()
        {
            return _dbContext.Platforms.ToList();
        }

        public bool SaveChanges()
        {
            return (_dbContext.SaveChanges() >= 0 );
        }
    }
}