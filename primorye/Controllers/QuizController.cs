using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using primorye.Data;
using primorye.Models;

[Route("api/[controller]")]
[ApiController]
public class QuestionController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public QuestionController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("random")]
    public async Task<ActionResult<Question>> GetRandomQuestion()
    {
        var question = await _context.Questions
            .Include(q => q.Variants)
            .OrderBy(r => Guid.NewGuid())
            .FirstOrDefaultAsync();

        if (question == null)
            return NotFound();

        return Ok(new
        {
            id = question.Id,
            text = question.Text,
            price = question.Price,
            variants = question.Variants.Select(v => new { v.Id, v.Text })
        });
    }
}