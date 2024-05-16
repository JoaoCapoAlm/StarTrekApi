using System.Net;
using System.Reflection;
using System.Text.Json.Serialization;
using Application.Configurations;
using CrossCutting.Enums;
using Domain;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(opts => opts.AddPolicy("cors", b =>
{
    b.AllowAnyOrigin().DisallowCredentials().WithMethods("GET");
}));

builder.Services.AddDbContext<StarTrekContext>(opts =>
{
    opts.UseSqlServer(
        builder.Configuration.GetConnectionString("DbConnection"),
        x => x.MigrationsAssembly(nameof(Application))
    );
});
//builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Path.GetTempPath()));

builder.Services.AddLocalization()
            .AddControllers()
            .AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter<QuadrantEnum>());
            });
builder.Services.DependencyInjection();
builder.Services.ConfigureHttpClientDefaults(opts =>
{
    opts.ConfigureHttpClient(o =>
    {
        o.DefaultRequestVersion = HttpVersion.Version30;
        o.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;
    });
});
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Star Trek API",
        Version = "v1",
        License = new Microsoft.OpenApi.Models.OpenApiLicense()
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/license/mit-0")
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
    opts.OperationFilter<AddRequiredHeaderParameter>();

    opts.IgnoreObsoleteActions();
    opts.IgnoreObsoleteProperties();
});

var app = builder.Build();

app.ConfigMiddleware();
app.MapControllers();
app.Run();
