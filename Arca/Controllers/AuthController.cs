using Arca.Api.Repositories;
using Arca.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arca.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioRepository _repo;
        public AuthController(UsuarioRepository repo) => _repo = repo;

        public class LoginDto { public string Usuario { get; set; } = ""; public string Contrasena { get; set; } = ""; }

        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login([FromBody] LoginDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Usuario) || string.IsNullOrWhiteSpace(dto.Contrasena))
                return BadRequest("Datos inválidos.");

            var user = await _repo.GetByCredentialsAsync(dto.Usuario, dto.Contrasena);
            if (user is null) return Unauthorized("Credenciales incorrectas.");

            return Ok(user);
        }
    }
}