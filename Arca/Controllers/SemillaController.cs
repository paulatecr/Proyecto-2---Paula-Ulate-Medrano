using Arca.Api.Repositories;
using Arca.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemillasController : ControllerBase
    {
        private readonly SemillaRepository _repo;
        public SemillasController(SemillaRepository repo) => _repo = repo;

        // GET: /api/semillas/grid
        [HttpGet("grid")]
        public async Task<ActionResult<List<SemillaGrid>>> GetGrid()
        {
            var data = await _repo.GetAllWithNamesAsync();
            return Ok(data);
        }
    }
}