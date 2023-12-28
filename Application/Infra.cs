using Application.Resources;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace Application
{
    public static class Infra
    {
        public static IServiceCollection DependencyInjection(this IServiceCollection services)
        {
            services.TryAddTransient(typeof(IStringLocalizer<Messages>), typeof(StringLocalizer<Messages>));

            return services;
        }

        public static IApplicationBuilder ConfigMiddleware(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseCors(opts =>
            {
                opts.AllowAnyOrigin().WithMethods("GET");
            });

            app.UseRequestLocalization(opts =>
            {
                string[] supportedCultures = ["en-US", "pt-BR"];
                opts.AddSupportedCultures(supportedCultures)
                    .SetDefaultCulture(supportedCultures[0]);
            });

            app.UseAuthorization();

            return app;
        }
    }
}
