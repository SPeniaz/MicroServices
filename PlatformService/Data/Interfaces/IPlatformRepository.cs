using PaltformService.Models;

namespace PaltformService.Data.Interfaces
{
    public interface IPlatformRepository
    {
        bool SaveChanges();

        IEnumerable<Platform> GetAll();

        Platform? Get(int id);
        
        void Create(Platform platform);
    }
}