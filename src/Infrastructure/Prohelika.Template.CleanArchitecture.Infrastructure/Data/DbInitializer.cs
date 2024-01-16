using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Prohelika.Template.CleanArchitecture.Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationDbContext>>());
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.Database.Migrate();

        DbSeeder.Seed(context, serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>());
    }
}