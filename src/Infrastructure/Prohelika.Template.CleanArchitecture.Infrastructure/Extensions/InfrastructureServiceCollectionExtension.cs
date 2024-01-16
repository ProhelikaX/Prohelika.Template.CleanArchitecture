using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prohelika.Template.CleanArchitecture.Domain.UnitOfWorks;
using Prohelika.Template.CleanArchitecture.Infrastructure.Data;
using Prohelika.Template.CleanArchitecture.Infrastructure.UnitOfWorks;

namespace Prohelika.Template.CleanArchitecture.Infrastructure.Extensions;

public static class InfrastructureServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                // options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}