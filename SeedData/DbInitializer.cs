using System.Text.Json;
using PruebaBackend.Models;
using PruebaBackend.Data;

namespace PruebaBackend.SeedData
{
    public static class DbInitializer
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!context.MarcasAutos.Any())
            {
                var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "SeedData", "marcas.json");
                var jsonData = File.ReadAllText(jsonPath);
                var marcas = JsonSerializer.Deserialize<List<MarcaAuto>>(jsonData);

                context.MarcasAutos.AddRange(marcas!);
                context.SaveChanges();
            }
        }
    }
}
