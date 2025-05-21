using Microsoft.EntityFrameworkCore;
using Npgsql;
using PruebaBackend.Data;
using PruebaBackend.SeedData;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        var dbContext = services.GetRequiredService<AppDbContext>();

        try
        {
            logger.LogInformation("Esperando a que PostgreSQL esté listo...");
            await WaitForPostgresAsync(dbContext, logger);

            logger.LogInformation("Verificando migraciones pendientes...");
            await ApplyMigrationsAsync(dbContext, logger);

            logger.LogInformation("Ejecutando seed data...");
            DbInitializer.Seed(app);
            logger.LogInformation("Inicialización completada exitosamente.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error durante la inicialización de la base de datos");
            throw;
        }
    }

    private static async Task ApplyMigrationsAsync(AppDbContext dbContext, ILogger<Program> logger)
    {
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            logger.LogInformation($"Aplicando {pendingMigrations.Count()} migraciones pendientes...");
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Migraciones aplicadas con éxito.");
        }
        else
        {
            logger.LogInformation("No hay migraciones pendientes.");
        }
    }

    private static async Task WaitForPostgresAsync(AppDbContext dbContext, ILogger<Program> logger,
        int maxRetries = 10, int delaySeconds = 5)
    {
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                logger.LogInformation($"Intentando conectar a PostgreSQL (intento {i + 1}/{maxRetries})...");
                var canConnect = await dbContext.Database.CanConnectAsync();

                if (canConnect)
                {
                    logger.LogInformation("¡Conexión exitosa a PostgreSQL!");
                    return;
                }
            }
            catch (Npgsql.PostgresException ex) when (ex.IsTransient)
            {
                logger.LogWarning($"Error transitorio: {ex.Message}. Reintentando en {delaySeconds} segundos...");
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error inesperado al conectar a PostgreSQL");
                throw;
            }
        }

        throw new Exception("No se pudo conectar a PostgreSQL después de múltiples intentos");
    }
}