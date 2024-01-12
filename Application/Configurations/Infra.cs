using Application.Data;
using Application.Middleware;
using Application.Resources;
using Application.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace Application
{
    public static class Infra
    {
        public static IServiceCollection DependencyInjection(this IServiceCollection services)
        {
            services.TryAddTransient(typeof(IStringLocalizer<Messages>), typeof(StringLocalizer<Messages>));
            services.TryAddScoped(typeof(StarTrekContext));
            services.TryAddScoped<CastService>();
            services.TryAddScoped<MovieService>();

            return services;
        }

        public static IApplicationBuilder ConfigMiddleware(this IApplicationBuilder app, bool IsDevelopment)
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
                    .SetDefaultCulture(supportedCultures[1]);
            });
            
            if (IsDevelopment)
            {
                app.UseDeveloperExceptionPage();
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
    }
}
