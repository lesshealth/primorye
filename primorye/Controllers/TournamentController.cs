using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using primorye.Data;
using primorye.Models;

namespace primorye.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TournamentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentTable>>> GetTable()
        {
            return await _context.TournamentTables
                .Include(t => t.Team)
                .Include(t => t.City)
                .ToListAsync();
        }
    }
}
