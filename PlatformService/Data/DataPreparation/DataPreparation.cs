using Microsoft.EntityFrameworkCore;
using PaltformService.Models;

namespace PaltformService.Data.DataPreparation
{
    public static class DataPreparation
    {
        public static void Populate(WebApplication application, bool isProduction)
        {
            using (var serviceScope = application.Services.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
            }
        }

        private static void SeedData(AppDbContext? dbContext, bool isProduction)
        {

            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            if (isProduction)
            {
                System.Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
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