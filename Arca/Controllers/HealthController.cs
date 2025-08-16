using Microsoft.AspNetCore.Mvc;

namespace Arca.Api.Controllers
{
    [ApiController]
    [Route("healthz")]
    public class HealthController : ControllerBase
    {
        private readonly Arca.Api.Repositories.SqlConnectionFactory _factory;
        public HealthController(Arca.Api.Repositories.SqlConnectionFactory factory) => _factory = factory;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                using var cn = _factory.Create();
                await cn.OpenAsync();
                return Ok(new { api = "ok", db = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { api = "ok", db = "fail", error = ex.Message });
            }
        }
    }
}