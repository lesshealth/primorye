using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using primorye.Data;

namespace primorye.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IncidentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("random-by-city")]
        public async Task<IActionResult> GetIncidentWithSolutions([FromQuery] int cityId)
        {
            var incident = await _context.Incidents
                .Where(i => i.CityId == cityId)
                .OrderBy(i => Guid.NewGuid())
                .FirstOrDefaultAsync();

            if (incident == null)
                return NotFound(new { message = "Нет инцидентов для города" });

            var solutions = await _context.Solutions
                .Where(s => s.IdIncidents == incident.Id)
                .ToListAsync();

            return Ok(new
            {
                id = incident.Id,
                text = incident.Text,
                difficulty = incident.Difficulty,
                solutions = solutions.Select(s => new
                {
                    id = s.Id,
                    text = s.Text,
                    price = s.Price,
                    progress = s.Progress,
                    public_opinion = s.PublicOpinion
                }).ToList()
            });
        }
    }
}
