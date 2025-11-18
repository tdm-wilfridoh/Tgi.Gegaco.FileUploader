using Serilog;
using Tgi.Gegaco.FileUploader.Application;
using Tgi.Gegaco.FileUploader.Infrastructure;
using Tgi.Gegaco.FileUploader.Infrastructure.Models;

//Serilog configuration
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
    // Add services to the container.
    builder.Services.Configure<DocumentSettings>(builder.Configuration.GetSection(DocumentSettings.SectionName));
    builder.Services.AddAMediatrApplication();
    builder.Services.AddInfrastructure(builder.Configuration);



    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Configurar CORS para Angular
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

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowAngular");

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal("Aplicación terminada inesperadamente: ", ex);
}
finally
{
    Log.CloseAndFlush();
}
