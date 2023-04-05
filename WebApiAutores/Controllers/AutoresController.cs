using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;
using WebApiAutores.Filtros;


namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }



        [HttpGet]
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {

            var autores = await _context.Autores.ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<AutorLibrosDTO>> Get(int id)
        {


            var autor = await _context.Autores
                .Include(c=>c.AutoresLibros)
                .ThenInclude(d=>d.Libro)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
                return NotFound();

            return mapper.Map<AutorLibrosDTO>(autor); ;

        }

        //En este usuo query parameters details?id=1&&name=Juan
        [HttpGet("details")]
        public async Task<ActionResult<Autor>> Get([FromHeader] int id, [FromQuery] string name)
        {


            var autor = await _context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null)
                autor = await _context.Autores.FirstOrDefaultAsync(x => x.Name.Contains(name));

            if (autor == null)
                return NotFound();

            return Ok(autor);

        }


        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromRoute] string nombre)
        {


            var autores = await _context.Autores.Where(x => x.Name.Contains(nombre)).ToListAsync();

            //if (autor == null)
            //    return NotFound();

            return mapper.Map<List<AutorDTO>>(autores);

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {
            var existeAutorConElMismoNombre = await _context.Autores.AnyAsync(x => x.Name == autorCreacionDTO.Name);

            if (existeAutorConElMismoNombre)
                return BadRequest($"Ya existe un autor con el nombre {autorCreacionDTO.Name}");

            var autor = mapper.Map<Autor>(autorCreacionDTO);

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
