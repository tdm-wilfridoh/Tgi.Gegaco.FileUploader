using Microsoft.AspNetCore.SpaServices.AngularCli;
using Serilog;
using System.Reflection;
using Tgi.Gegaco.FileUploader.Application;
using Tgi.Gegaco.FileUploader.Infrastructure;
using Tgi.Gegaco.FileUploader.Infrastructure.Models;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build())
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseSerilog();

    Log.Information("----- Iniciando FileUploader API -----");

    // Configuración de servicios
    builder.Services.Configure<DocumentSettings>(builder.Configuration.GetSection(DocumentSettings.SectionName));
    builder.Services.AddAMediatrApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddAutoMapperService();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Archivos del frontend Angular (solo para producción)
    //builder.Services.AddSpaStaticFiles(options =>
    //{
    //    options.RootPath = "ClientApp/dist";
    //});

    builder.Services.AddSpaStaticFiles(options =>
    {
        // Apuntamos a wwwroot porque el .csproj copia ahí los artefactos de Angular
        options.RootPath = "wwwroot";
    });

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngular", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });


    var app = builder.Build();

    // Swagger
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // CORS
    app.UseCors("AllowAngular");

    // Archivos estáticos (Angular compilado)
    app.UseStaticFiles();
    app.UseSpaStaticFiles();

    // Routing
    app.UseRouting();
    app.UseAuthorization();

    // Endpoints de la API
    app.MapControllers();

    // *IMPORTANTE*
    // NO usar UseSpa aquí, porque el csproj ya ejecuta Angular en Debug
    // y en Release Angular está compilado en wwwroot.
    // Dejarlo vacío simplemente sirve el index.html de Angular en producción

    if (!app.Environment.IsDevelopment())
    {
        // En producción: fallback al index.html
        app.MapFallbackToFile("index.html");
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplicación terminada inesperadamente:");
}
finally
{
    Log.CloseAndFlush();
}
