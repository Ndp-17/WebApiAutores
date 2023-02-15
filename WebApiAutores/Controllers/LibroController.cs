using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LibroController(ApplicationDbContext context)
        {
            _context = context;
        }

        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<Libro>> Get(int id)
        //{
        //    return await _context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);
        
        //}
        //[HttpPost]
        //public async Task<ActionResult> Post(Libro libro)
        //{
        //    var existe = await _context.Autores.AnyAsync(x => x.Id == libro.AutorId);

        //    if (!existe)
        //        return BadRequest($"No existe el autor de Id :{ libro.AutorId}");

        //    _context.Add(libro);
        //    await _context.SaveChangesAsync();
        //    return Ok();

        //}
    }
}
