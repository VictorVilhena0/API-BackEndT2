using Charpter.WebApi.Interfaces;
using Charpter.WebApi.Models;
using Charpter.WebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Charpter.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioRepository _iUsuarioRepository;

        public LoginController(IUsuarioRepository iUsuarioRepository)
        {
            _iUsuarioRepository = iUsuarioRepository;
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            try
            {
                Usuario usuarioEncontrado = _iUsuarioRepository.Login(login.Email, login.Senha);

                if (usuarioEncontrado == null)
                {
                    return Unauthorized(new {msg = "Email e/ou senha inválidos" });
                }

                var minhasClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, usuarioEncontrado.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, usuarioEncontrado.Id.ToString()),
                    new Claim(ClaimTypes.Role, usuarioEncontrado.Tipo),
                };

                var chave = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("bomba-nuclear-autorizacao"));
                
                var credencials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

                var meuToken = new JwtSecurityToken(
                    issuer: "charpter.webapi",
                    audience: "charpter.webapi",
                    claims: minhasClaims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: credencials
                );

                return Ok(
                    new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(meuToken),
                    }
                );

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
