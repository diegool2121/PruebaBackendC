using Microsoft.AspNetCore.Mvc;
using PruebaBackend.Data;
using PruebaBackend.Models;

namespace PruebaBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarcasAutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MarcasAutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MarcaAuto>> Get()
        {
            return Ok(_context.MarcasAutos.ToList());
        }
    }
}
