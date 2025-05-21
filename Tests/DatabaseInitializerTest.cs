using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PruebaBackend.Data;
using System.Threading.Tasks;
using Xunit;

namespace PruebaBackend.Tests.Initializer
{
    public class DatabaseInitializerTests
    {
        private readonly AppDbContext _context;
        private readonly ILogger<Program> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DatabaseInitializerTests()
        {
            // Configurar servicios
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            serviceCollection.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            _serviceProvider = serviceCollection.BuildServiceProvider();
            _context = _serviceProvider.GetRequiredService<AppDbContext>();
            _logger = _serviceProvider.GetRequiredService<ILogger<Program>>();
        }

        [Fact] 
        public async Task WaitForPostgresAsync_ShouldConnectSuccessfully()
        {
            // Act
            var canConnect = await _context.Database.CanConnectAsync();

            // Assert
            Assert.True(canConnect, "La base de datos debería estar accesible.");
        }
    }
}