using AutoMapper;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        {
             var libro = await _context.Libros.FirstOrDefaultAsync(x => x.Id == id);
                                //.Include(x=>x.Comentarios)

            if (libro == null)  
                return NotFound(); 

            return mapper.Map<LibroDTO>(libro);
        }
        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacion)
        {
            if(libroCreacion.AutoresIds.Any())
                return BadRequest("No se puede crear un libro sin autores");


            var  autoresIds = await _context.Autores.Where(at=> libroCreacion.AutoresIds.
                                        Contains(at.Id)).Select(x=>x.Id).ToListAsync();
            if(autoresIds.Count != libroCreacion.AutoresIds.Count) 
            { 
                return BadRequest("No exite uno de los autores enviados.");
            
            }

            var libro = mapper.Map<Libro>(libroCreacion);
            _context.Add(libro);
            await _context.SaveChangesAsync();
            return Ok();

        }
    }
}
