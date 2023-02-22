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

            if (libro == null)  
                return NotFound(); 

            return mapper.Map<LibroDTO>(libro);
        }
        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO LibroCraacion)
        {
            //var existe = await _context.Autores.AnyAsync(x => x.Id == libro.AutorId);

            //if (!existe)
            //    return BadRequest($"No existe el autor de Id :{libro.AutorId}");

            var libro = mapper.Map<Libro>(LibroCraacion);
            _context.Add(libro);
            await _context.SaveChangesAsync();
            return Ok();

        }
    }
}
