using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebApiAutores.DTOs;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        public CuentasController(UserManager<IdentityUser> userManager,
                                 IConfiguration configuration) 
        {
            this.userManager = userManager;
            this.configuration = configuration;
        
        }



        [HttpPost("registrar")]
        public async Task<AcceptedResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario credencialesUsuario)
        {
            var usario = new IdentityUser { UserName = credencialesUsuario.Email, Email = credencialesUsuario.Email };
            var resultado = await userManager.CreateAsync(usario,credencialesUsuario.Password);

            if(resultado.Succeeded) 
            { 

            }
            else
            {
                return BadRequest(resultado.Errors);
            }
                
        }


        private RespuestaAutenticacion ConstruirToken(CredencialesUsuario credencialesUsuario)
        {
            var claims = new List<Claim>()
            {
                new Claim("email",credencialesUsuario.Email),
                new Claim("Niels","Niels")

            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[]));
        
        }
     
    }
}
