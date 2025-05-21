using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaBackend.Controllers;
using PruebaBackend.Data;
using PruebaBackend.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PruebaBackend.Tests.Controllers
{
    public class MarcasAutosControllerTests
    {
        private readonly AppDbContext _context;
        private readonly MarcasAutosController _controller;

        public MarcasAutosControllerTests()
        {
            // Configurar una base de datos en memoria
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            
            _context = new AppDbContext(options);

            // Agregar datos de prueba
            _context.MarcasAutos.AddRange(
                new MarcaAuto { Nombre = "Toyota" },
                new MarcaAuto { Nombre = "Ford" },
                new MarcaAuto { Nombre = "Chevrolet" }
            );
            _context.MarcasAutos.RemoveRange(_context.MarcasAutos);
            _context.SaveChanges();

            // Crear instancia del controlador con el contexto
            _controller = new MarcasAutosController(_context);
        }

        [Fact]
        public void Get_ReturnsAllMarcasAutos()
        {
            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var marcas = Assert.IsAssignableFrom<IEnumerable<MarcaAuto>>(okResult.Value);
            Assert.Equal(3, marcas.Count());
        }

        [Fact]
        public void Get_ReturnsCorrectMarcas()
        {
            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var marcas = Assert.IsAssignableFrom<IEnumerable<MarcaAuto>>(okResult.Value);
            Assert.Contains(marcas, m => m.Nombre == "Toyota");
            Assert.Contains(marcas, m => m.Nombre == "Ford");
            Assert.Contains(marcas, m => m.Nombre == "Chevrolet");
        }
    }
}