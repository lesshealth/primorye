using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using primorye.Data;
using primorye.Models;

namespace primorye.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeamController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetAll()
        {
            return await _context.Teams.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetById(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();
            return team;
        }
    }
}
