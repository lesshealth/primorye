using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using primorye.Models;
using primorye.Data;
using primorye.DTOs;

namespace primorye.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Team)
                .FirstOrDefaultAsync(u => u.Login == request.Login && u.Password == request.Password);

            if (user == null)
                return Unauthorized(new { message = "Неверные данные" });

            return Ok(new
            {
                user.Login,
                Team = user.Team.Name
            });
        }
    }
}
