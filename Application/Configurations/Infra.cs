using Application.Middleware;
using Application.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Domain;
using CrossCutting.Resources;
using Domain.Validation;

namespace Application.Configurations
{
    public static class Infra
    {
        public static IServiceCollection DependencyInjection(this IServiceCollection services)
        {
            services.TryAddScoped<StarTrekContext>();

            services.TryAddTransient<IStringLocalizer<Messages>, StringLocalizer<Messages>>();
            services.TryAddTransient<IStringLocalizer<TitleSynopsis>, StringLocalizer<TitleSynopsis>>();

            services.TryAddScoped<CrewService>();
            services.TryAddScoped<MovieService>();
            services.TryAddScoped<SerieService>();
            services.TryAddScoped<TmdbService>();

            services.TryAddScoped<CreateEpisodeValidator>();
            services.TryAddScoped<CreateMovieValidation>();
            services.TryAddScoped<CreateSeasonValidation>();
            services.TryAddScoped<CreateSerieValidation>();
            services.TryAddScoped<UpdateMovieValidation>();
            services.TryAddScoped<UpdateSerieValidation>();
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
