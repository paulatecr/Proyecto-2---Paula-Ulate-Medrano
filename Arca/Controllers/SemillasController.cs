using Arca.Shared.Models;
using Arca.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Arca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemillasController : ControllerBase
    {
        private readonly SemillaRepository _repo;
        public SemillasController(SemillaRepository repo) => _repo = repo;

        // GET: api/semillas
        [HttpGet]
        public async Task<ActionResult<List<Semilla>>> GetAll()
            => Ok(await _repo.GetAllAsync());

        // GET: api/semillas/grid
        [HttpGet("grid")]
        public async Task<ActionResult<List<SemillaGrid>>> GetGrid()
            => Ok(await _repo.GetAllWithNamesAsync());

        // GET: api/semillas/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Semilla>> GetById(int id)
        {
            var s = await _repo.GetByIdAsync(id);
            if (s == null) return NotFound();
            return Ok(s);
        }

        // POST: api/semillas
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Semilla s)
        {
            if (s is null) return BadRequest("Datos vacíos.");
            if (s.FechaCreacion == default) s.FechaCreacion = DateTime.Now;
            if (s.CreadoPor == 0) s.CreadoPor = 1;

            await _repo.InsertAsync(s);
            return Ok();
        }

        // PUT: api/semillas/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Semilla s)
        {
            if (s is null || id != s.Id) return BadRequest("Id inválido.");
            if (s.FechaModificacion == null) s.FechaModificacion = DateTime.Now;
            if (s.ModificadoPor == null) s.ModificadoPor = 1;

            var ok = await _repo.UpdateAsync(s);
            if (!ok) return NotFound();
            return Ok();
        }

        // DELETE: api/semillas/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            if (!ok) return NotFound();
            return Ok();
        }
    }
}