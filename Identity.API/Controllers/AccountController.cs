using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserDbContext.Infrastructure;

namespace Identity.API.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtTokenHandler _jwtTokenHandler;
        private readonly UserContext _ctx;
        public AccountController(JwtTokenHandler jwtTokenHandler, UserContext ctx)
        {
            _jwtTokenHandler = jwtTokenHandler;
            _ctx = ctx;
        }
        
        [HttpPost]
        public ActionResult<AuthenticationResponse?> Authenticate([FromBody] AuthenticationRequest authenticationRequest)
        {
            //Agregando el try-catch
            if (string.IsNullOrWhiteSpace(authenticationRequest.Carnet) || string.IsNullOrWhiteSpace(authenticationRequest.Clave))
                return NotFound();

            try
            {
                var userAccount = _ctx.Usuarios.Where(x => x.Carnet == authenticationRequest.Carnet && EF.Functions.Collate(x.Clave, "SQL_Latin1_General_CP1_CS_AS") == authenticationRequest.Clave && x.estado == 1).FirstOrDefault();
                //
                if (userAccount == null) return NotFound();

                UserAccount account = new UserAccount(userAccount.Carnet, userAccount.Clave, userAccount.IdRol, userAccount.NombreUsuario);

                var authenticationResponse = _jwtTokenHandler.GenerateJwtToken(account);
                if (authenticationResponse == null) return Unauthorized();
                return Ok(authenticationResponse);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
