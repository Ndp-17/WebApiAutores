using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AutoresController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await _context.Autores.Include(x => x.Libros).ToListAsync();
        }

        [HttpGet("primero")]
        public async Task<ActionResult<Autor>> Primero()
        {
            return await _context.Autores.Include(x => x.Libros).FirstOrDefaultAsync();
        }
        [HttpGet("details")]
        public async Task<ActionResult<Autor>> Get(int id, string name)
        {


            var autor = await _context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null)
                 autor = await _context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Name.Contains(name));

            if (autor == null)
                return NotFound();

            return Ok(autor);

        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> Get(string nombre)
        {


            var autor = await _context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Name.Contains(nombre));

            if (autor == null)
                return NotFound();

            return Ok(autor);

        }
        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {

            _context.Add(autor);
            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id)
                return BadRequest("El id del autor no incide con el id de la url");
            var existe = await _context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
                return NotFound();

            _context.Update(autor);
            await _context.SaveChangesAsync();
            return Ok();


        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
                return NotFound();

            _context.Remove(new Autor() { Id = id });
            await _context.SaveChangesAsync();
            return Ok();


        }
    }
}
