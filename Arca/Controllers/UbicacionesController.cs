using Arca.Api.Repositories;
using Arca.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UbicacionesController : ControllerBase
    {
        private readonly UbicacionRepository _repo;
        public UbicacionesController(UbicacionRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<ActionResult<List<Ubicacion>>> GetAll()
            => Ok(await _repo.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Ubicacion>> GetById(int id)
        {
            var u = await _repo.GetByIdAsync(id);
            return u is null ? NotFound() : Ok(u);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Ubicacion u)
        {
            if (u is null) return BadRequest("Datos vacíos.");
            var newId = await _repo.InsertAsync(u);
            return Ok(newId);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Ubicacion u)
        {
            if (u is null || id != u.Id) return BadRequest("Id inválido.");
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
