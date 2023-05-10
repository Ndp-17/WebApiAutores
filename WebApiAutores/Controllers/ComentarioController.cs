using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libro/{libroId:int}/comentarios")]
    public class ComentarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        public ComentarioController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId)
        {
            var existeLibro = await _context.Libros.AnyAsync(libroDb => libroDb.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();
            }

            var comentarios = await _context.Comentarios.
                Where(x => x.LibroId == libroId).ToListAsync();

            return mapper.Map<List<ComentarioDTO>>(comentarios);


        }

        [HttpGet("{id:int}", Name = "obtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> GetById(int id)
        {
            var comentario = await _context.Comentarios.FirstOrDefaultAsync(a => a.Id == id);
            if (comentario == null)
                return NotFound();
            return mapper.Map<ComentarioDTO>(comentario);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(x => x.Type == "email").FirstOrDefault();

            var email = emailClaim.Value;

            var usuario = await userManager.FindByEmailAsync(email);

            var usuarioId = usuario.Id;

            var existeLibro = await _context.Libros.AnyAsync(libroDb => libroDb.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            comentario.UsuarioId = usuarioId;
            _context.Add(comentario);

            await _context.SaveChangesAsync();

            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);
            return CreatedAtRoute("obtenerComentario", new { id = comentario.Id, libroId }, comentarioDTO);

        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromBody] ComentarioCreacionDTO comentarioDTO, int id, int libroId)
        {
            var existeLibro = await _context.Libros.AnyAsync(libroDb => libroDb.Id == libroId);

            if (!existeLibro)
                return NotFound();


            var existe = await _context.Comentarios.AnyAsync(x => x.Id == id);

            if (!existe)
                return NotFound();

            var comentario = mapper.Map<Comentario>(comentarioDTO);
            comentario.Id = id;
            comentario.LibroId = libroId;

            _context.Update(comentario);
            await _context.SaveChangesAsync();


            return NoContent();
        }

    }
}
