var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
ServiceConfiguration.ConfigureServices(builder);

var app = builder.Build();

// Configuración de la aplicación
AppConfiguration.ConfigureApplication(app);

// Inicialización de la base de datos
try
{
    await DatabaseInitializer.InitializeDatabaseAsync(app);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Error durante la inicialización de la aplicación");

    if (app.Environment.IsDevelopment())
    {
        throw;
    }
}

app.Run();