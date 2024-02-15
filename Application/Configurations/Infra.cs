using Application.Data;
using Application.Data.Validation;
using Application.Middleware;
using Application.Resources;
using Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace Application
{
    public static class Infra
    {
        public static IServiceCollection DependencyInjection(this IServiceCollection services)
        {
            services.TryAddScoped<StarTrekContext>();
            services.AddValidatorsFromAssemblyContaining<CreateEpisodeValidator>();

            services.TryAddTransient<IStringLocalizer<Messages>, StringLocalizer<Messages>>();
            services.TryAddTransient<IStringLocalizer<TitleSynopsis>, StringLocalizer<TitleSynopsis>>();

            services.TryAddScoped<CrewService>();
            services.TryAddScoped<MovieService>();
            services.TryAddScoped<SerieService>();
            services.TryAddScoped<TmdbService>();
            return services;
        }

        public static IApplicationBuilder ConfigMiddleware(this IApplicationBuilder app, bool IsProduction)
        {
            app.UseHttpsRedirection();

            app.UseCors(opts =>
            {
                opts.AllowAnyOrigin().WithMethods("GET");
            });

            app.UseRequestLocalization(opts =>
            {
                string[] supportedCultures = SupportedCultures();
                opts.AddSupportedCultures(supportedCultures)
                    .SetDefaultCulture(supportedCultures[0]);
            });
            
            if (!IsProduction)
            {
                app.UseSwagger();
                app.UseSwaggerUI(opts =>
                {
                    opts.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    opts.RoutePrefix = string.Empty;
                });
            }
            app.UseAppMiddleware();
            app.UseAuthorization();

            return app;
        }

        internal static string[] SupportedCultures()
        {
            return ["en-US", "pt-BR"];
        }
    }
}
