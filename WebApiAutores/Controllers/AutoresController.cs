using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;
using WebApiAutores.Servicios;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IServicio servicio;

        public AutoresController(ApplicationDbContext context, IServicio servicio)
        {
            _context = context;
            this.servicio = servicio;
        }


        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await _context.Autores.Include(x => x.Libros).ToListAsync();
        }
        [HttpGet]
        [HttpGet("listado")]
        [HttpGet("/listado")]
        public List<Autor> Listado()
        {
            servicio.Realizartarea();

            return _context.Autores.Include(x => x.Libros).ToList();
        }


        [HttpGet("primero")]
        public async Task<ActionResult<Autor>> Primero()
        {
            return await _context.Autores.Include(x => x.Libros).FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> Get(int id)
        {


            var autor = await _context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
                return NotFound();

            return Ok(autor);

        }

        //En este usuo query parameters details?id=1&&name=Juan
        [HttpGet("details")]
        public async Task<ActionResult<Autor>> Get([FromHeader] int id, [FromQuery] string name)
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
        public async Task<ActionResult> Post([FromBody] Autor autor)
        {
            var existeAutorConElMismoNombre = await _context.Autores.AnyAsync(x => x.Name == autor.Name);

            if (existeAutorConElMismoNombre)
                return BadRequest($"Ya existe un autor con el nombre {autor.Name}");

            _context.Add(autor);
            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromBody] Autor autor, [FromQuery] int id)
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
