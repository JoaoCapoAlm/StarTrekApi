using System.Net;
using System.Reflection;
using Application;
using Application.Data;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(opts => opts.AddPolicy("cors", b =>
{
    b.AllowAnyOrigin().WithMethods("GET");
}));

builder.Services.AddDbContext<StarTrekContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});
builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Path.GetTempPath()));

builder.Services.AddLocalization();
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.DependencyInjection();
builder.Services.AddEndpointsApiExplorer();
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
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Jo�o",
            Email = "   ato@capoanisolucoes.com.br"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

var app = builder.Build();

app.ConfigMiddleware(app.Environment.IsProduction());

app.MapControllers();

app.Run();
