using PaltformService.Models;

namespace PaltformService.Data.DataPreparation
{
    public static class DataPreparation
    {
        public static void Populate(WebApplication application)
        {
            using (var serviceScope = application.Services.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext? dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (!dbContext.Platforms.Any())
            {
                System.Console.WriteLine("--> Seeding data.");
                dbContext.AddRange(
                    new Platform()
                    {
                        Name = ".NET",
                        Publisher = "Microsoft",
                        Cost = "Free",
                    },
                    new Platform()
                    {
                        Name = "SQL Server Express",
                        Publisher = "Microsoft",
                        Cost = "Free",
                    },
                    new Platform()
                    {
                        Name = "Kubernetes",
                        Publisher = "Cloud Native Computing Foundation",
                        Cost = "Free",
                    }
                );

                dbContext.SaveChanges();

                System.Console.WriteLine("--> Seeding data finished.");
            }
            else
            {
                Console.WriteLine("--> Data already exists.");
            }
        }
    }
}