using Application.Middleware;
using Application.Services;
using CrossCutting.Resources;
using Domain;
using Domain.Profiles;
using Domain.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace Application.Configurations
{
    public static class Infra
    {
        public static IServiceCollection DependencyInjection(this IServiceCollection services)
        {
            services.TryAddScoped<StarTrekContext>();
            services.AddAutoMapper(typeof(SeasonProfile), typeof(EpisodeProfile));

            services.TryAddTransient<IStringLocalizer<Messages>, StringLocalizer<Messages>>();
            services.TryAddTransient<IStringLocalizer<TitleSynopsis>, StringLocalizer<TitleSynopsis>>();

            services.TryAddScoped<CrewService>();
            services.TryAddScoped<MovieService>();
            services.TryAddScoped<SeasonService>();
            services.TryAddScoped<SerieService>();
            services.TryAddScoped<TmdbService>();

            services.AddValidatorsFromAssemblyContaining(typeof(CreateEpisodeValidator));
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
                string[] supportedCultures = SupportedCultures();
                opts.AddSupportedCultures(supportedCultures)
                    .SetDefaultCulture(supportedCultures[0]);
            });

            app.UseSwagger();
            app.UseSwaggerUI(opts =>
            {
                opts.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                opts.RoutePrefix = string.Empty;
            });
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
