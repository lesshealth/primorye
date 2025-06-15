using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using primorye.Data;
using primorye.Models;

namespace primorye.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuestionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetAllQuestions()
        {
            return await _context.Questions
                .Include(q => q.Variants)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestionById(int id)
        {
            var question = await _context.Questions
                .Include(q => q.Variants)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
                return NotFound();
            return question;
        }

        [HttpGet("random/by-city")]
        public async Task<IActionResult> GetRandomQuestion([FromQuery] int cityId, [FromQuery] int difficultyLevel = 1)
        {
            var question = await _context.Questions
                .Where(q => q.CityId == cityId && q.DifficultyLevel == difficultyLevel)
                .Include(q => q.Variants)
                .OrderBy(q => Guid.NewGuid())
                .FirstOrDefaultAsync();

            if (question == null)
                return NotFound(new { message = "Нет вопросов такой сложности" });

            return Ok(new
            {
                id = question.Id,
                text = question.Text,
                price = question.Price,
                variants = question.Variants.Select(v => new { id = v.Id, text = v.Text })
            });
        }
    }
}
