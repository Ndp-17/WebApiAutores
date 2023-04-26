using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public LibroController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "obtenerLibro")]
        public async Task<ActionResult<LibroAutoresDTO>> Get(int id)
        {
            var libro = await _context.Libros.Include(c=>c.AutoresLibros)
                                .ThenInclude(a=>a.Autor).FirstOrDefaultAsync(x => x.Id == id);
            //.Include(x=>x.Comentarios)

            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();

            if (libro == null)
                return NotFound();

            return mapper.Map<LibroAutoresDTO>(libro);
        }
        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacion)
        {
            if (libroCreacion.AutoresIds == null)
                return BadRequest("No se puede crear un libro sin autores");


            var autoresIds = await _context.Autores.Where(at => libroCreacion.AutoresIds.
                                        Contains(at.Id)).Select(x => x.Id).ToListAsync();
            if (autoresIds.Count != libroCreacion.AutoresIds.Count)
            {
                return BadRequest("No exite uno de los autores enviados.");

            }

            var libro = mapper.Map<Libro>(libroCreacion);
            AsignarOrdenAutores(libro);


            _context.Add(libro);
            await _context.SaveChangesAsync();

            var LibroDTO = mapper.Map<LibroDTO>(libro);

            return CreatedAtRoute("obtenerLibro", new { id = libro.Id }, LibroDTO);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] LibroCreacionDTO libroCreacionDTO)
        { 
            var libroDB = await _context.Libros
                                            .Include(x=>x.AutoresLibros)
                                            .FirstOrDefaultAsync(x=>x.Id==id);
            if(libroDB==null)
                return NotFound();
            libroDB = mapper.Map(libroCreacionDTO, libroDB);
            AsignarOrdenAutores(libroDB);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private void AsignarOrdenAutores(Libro libro)
        {
            for (int i = 0; i < libro.AutoresLibros.Count; i++)
            {
                libro.AutoresLibros[i].Orden = i;

            }
        }
        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id,JsonPatchDocument<LibroPatchDTO> patchDocument)
        { 
            if(patchDocument==null) return BadRequest();

            var libroDB = await _context.Libros.FirstOrDefaultAsync(x=>x.Id==id);

            if(libroDB==null) return NotFound();

            var libroDTO = mapper.Map<LibroPatchDTO>(libroDB);

            patchDocument.ApplyTo(libroDTO, ModelState);

            var esValido = TryValidateModel(libroDTO);

            if(!esValido) return BadRequest(ModelState);

            mapper.Map(libroDTO, libroDB);

            await _context.SaveChangesAsync();

            return NoContent();
        
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _context.Libros.AnyAsync(x => x.Id == id);

            if (!existe)
                return NotFound();

            _context.Remove(new Libro() { Id = id });
            await _context.SaveChangesAsync();
            return Ok();


        }
    }
}
