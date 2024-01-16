using Prohelika.Template.CleanArchitecture.Infrastructure.Data;
using Prohelika.Template.CleanArchitecture.Presentation.Common.Utils;
using Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Middlewares;
using Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Utils;

namespace Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Extensions;

/// <summary>
/// WebApplication extension
/// </summary>
public static class WebApplicationExtension
{
    /// <summary>
    ///  Configures the application request pipeline
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static void ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsProduction())
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            DbInitializer.Initialize(serviceProvider);
        }


        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var swaggerOptions = app.Configuration.GetSection(SwaggerConfigurationOptions.Key).Get<SwaggerConfigurationOptions>();
            options.OAuthClientId(swaggerOptions?.OAuthClientId);
            options.OAuthClientSecret(swaggerOptions?.OAuthClientSecret);
            options.OAuthScopes("profile", "openid", "email", "roles", "offline_access");
            options.OAuthUsePkce();
            options.EnablePersistAuthorization();
        });

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseMiddleware<LoggerMiddleware>();

        if (app.Environment.IsProduction())
        {
            app.UseHttpsRedirection();
        }

        app.UseStaticFiles();

        app.UseCors(AppCorsPolicy.AllowAll);

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.MapHealthChecks("/health");
    }
}