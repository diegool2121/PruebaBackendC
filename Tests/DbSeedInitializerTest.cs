using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using PruebaBackend.Data;
using PruebaBackend.Models;
using PruebaBackend.SeedData;
using System.Threading.Tasks;
using Xunit;

namespace PruebaBackend.Tests.Initializer
{
    public class DbInitializerTests
    {
        private readonly IServiceProvider _serviceProvider;

        public DbInitializerTests()
        {
            // Configurar servicios y base de datos en memoria
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task Seed_ShouldPopulateDatabase()
        {
            // Obtener el contexto y limpiar la base de datos antes de ejecutar `Seed()`
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.MarcasAutos.RemoveRange(context.MarcasAutos); // 🔥 Eliminamos datos previos
            await context.SaveChangesAsync();

            // Act - Ejecutar `Seed()`
            var app = new ApplicationBuilder(_serviceProvider);
            DbInitializer.Seed(app);

            // Assert - Verificar que los datos se insertaron correctamente
            var marcas = await context.MarcasAutos.ToListAsync();
            Assert.NotEmpty(marcas);
        }
    }
}