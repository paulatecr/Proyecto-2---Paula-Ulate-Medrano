using System.Collections.Generic;
using System.Threading.Tasks;
using Arca.Api.Repositories;
using Arca.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioRepository _repo;
        public UsuariosController(UsuarioRepository repo) => _repo = repo;

        // POST api/usuarios/login
        public class LoginRequest { public string user { get; set; } public string contrasena { get; set; } }

        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login([FromBody] LoginRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.user) || string.IsNullOrWhiteSpace(req.contrasena))
                return BadRequest("Datos incompletos.");

            var u = await _repo.GetByCredentialsAsync(req.user, req.contrasena);
            if (u == null) return Unauthorized();

            // En producción NO devuelvas Contrasena
            u.Contrasena = null;
            return Ok(u);
        }

        // --- CRUD (opcional si ya lo usas desde MVC) ---

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetAll() => Ok(await _repo.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            var u = await _repo.GetByIdAsync(id);
            return u == null ? NotFound() : Ok(u);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Usuario u)
        {
            if (u == null) return BadRequest("Datos vacíos.");
            var newId = await _repo.InsertAsync(u);
            return Ok(newId);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Usuario u)
        {
            if (u == null || id != u.Id) return BadRequest("Id inválido.");
            var ok = await _repo.UpdateAsync(u);
            return ok ? Ok() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok ? Ok() : NotFound();
        }
    }
}