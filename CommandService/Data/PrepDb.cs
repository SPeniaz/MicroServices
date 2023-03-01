using CommandService.Data.Interfaces;
using CommandService.Models;
using CommandService.SyncDataServices.Grpc.Interfaces;

namespace CommandService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
                var platforms = grpcClient.ReturnAllPlatforms();

                SeedData(serviceScope.ServiceProvider.GetService<ICommandRepository>(), platforms);
            }
        }

        private static void SeedData(ICommandRepository repo, IEnumerable<Platform> platforms)
        {
            System.Console.WriteLine("--> Seeding new platforms...");
            foreach (var platform in platforms)
            {
                if(!repo.IsExternalPlatformExists(platform.ExternalId))
                {
                    repo.CreatePlatform(platform);
                }
                repo.SaveChanges();
            }

        }
    }
}