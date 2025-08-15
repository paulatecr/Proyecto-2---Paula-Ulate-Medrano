using Arca.Api.Repositories;
using Arca.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspeciesController : ControllerBase
    {
        private readonly EspecieRepository _repo;
        public EspeciesController(EspecieRepository repo) => _repo = repo;

        // GET: api/especies
        [HttpGet]
        public async Task<ActionResult<List<Especie>>> GetAll()
            => Ok(await _repo.GetAllAsync());

        // GET: api/especies/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Especie>> GetById(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e is null ? NotFound() : Ok(e);
        }

        // POST: api/especies
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Especie e)
        {
            if (e is null) return BadRequest("Datos vacíos.");
            var newId = await _repo.InsertAsync(e);
            return Ok(newId);
        }

        // PUT: api/especies/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Especie e)
        {
            if (e is null || id != e.Id) return BadRequest("Id inválido.");
            var ok = await _repo.UpdateAsync(e);
            return ok ? Ok() : NotFound();
        }

        // DELETE: api/especies/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok ? Ok() : NotFound();
        }
    }
}
