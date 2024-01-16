using Microsoft.Extensions.Logging;

namespace Prohelika.Template.CleanArchitecture.Infrastructure.Data;

public static class DbSeeder
{
    public static void Seed(ApplicationDbContext context, ILogger<ApplicationDbContext> logger)
    {
        logger.LogInformation("Seeding database...");

        context.SaveChanges();

        logger.LogInformation("Done seeding database");
    }
}