using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using primorye.Data;
using primorye.DTOs;
using primorye.Models;

namespace primorye.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private static readonly Dictionary<int, GameState> _games = new();

        [HttpPost("start")]
        public async Task<IActionResult> StartGame(
            [FromQuery] int teamId,
            [FromQuery] int cityId,
            [FromServices] ApplicationDbContext db)
        {
            if (_games.ContainsKey(teamId))
                return BadRequest(new { message = "Игра уже запущена" });

            var incident = await db.Incidents
                .Where(i => i.CityId == cityId)
                .OrderBy(i => Guid.NewGuid())
                .FirstOrDefaultAsync();

            if (incident == null)
                return NotFound(new { message = "Нет инцидентов для города" });

            var game = new GameState
            {
                TeamId = teamId,
                CityId = cityId,
                Finance = 0,
                SocialPoints = 0,
                Progress = 0,
                StepNumber = 1,
                WaitingForAnswer = true,
                CurrentGoal = $"Вы начали игру в городе ID={cityId}. Ответьте на вопрос!",
                CurrentRound = 1,
                QuestionsAnsweredInRound = 0,
                AskedQuestionIds = new HashSet<int>(),
                CurrentIncidentId = incident.Id,
                CurrentIncidentText = incident.Text
            };

            _games[teamId] = game;
            return Ok(game);
        }

        [HttpPost("answer-from-db")]
        public async Task<IActionResult> SubmitAnswerFromDb([FromBody] AnswerRequest request, [FromServices] ApplicationDbContext db)
        {
            if (!_games.TryGetValue(request.TeamId, out var game))
                return NotFound(new { message = "Нет сессии" });

            if (game.StepNumber != 1 || !game.WaitingForAnswer)
                return BadRequest(new { message = "Вопросы уже завершены или вы не в нужной фазе" });

            var variant = await db.Variants
                .Include(v => v.Question)
                .FirstOrDefaultAsync(v => v.Id == request.VariantId && v.QuestionId == request.QuestionId);

            if (variant == null)
                return BadRequest(new { message = "Некорректный вариант" });

            game.QuestionsAnsweredInRound++;

            if (variant.IsCorrect)
            {
                game.Finance += variant.Question.Price;
                game.CurrentGoal = $"Вы ответили: {variant.Text} — Правильно! +{variant.Question.Price}";
            }
            else
            {
                game.CurrentGoal = $"Вы ответили: {variant.Text} — Неверно!";
            }

            if (game.QuestionsAnsweredInRound >= 5)
            {
                if (game.CurrentRound == 1)
                {
                    game.CurrentRound = 2;
                    game.QuestionsAnsweredInRound = 0;
                    game.CurrentGoal += " Раунд 1 завершён. Начинаем Раунд 2.";
                }
                else
                {
                    game.StepNumber = 2;
                    game.WaitingForAnswer = false;
                    game.CurrentGoal += " Раунд 2 завершён. Перейдите к решению проблемы.";
                }
            }

            return Ok(game);
        }

        [HttpGet("next-question")]
        public async Task<IActionResult> GetNextQuestion([FromQuery] int teamId, [FromServices] ApplicationDbContext db)
        {
            if (!_games.TryGetValue(teamId, out var game))
                return NotFound(new { message = "Игра не найдена" });

            if (!game.WaitingForAnswer || game.StepNumber != 1)
                return BadRequest(new { message = "Сейчас нельзя получать вопрос" });

            int difficulty = game.QuestionsAnsweredInRound + 1;

            var question = await db.Questions
                .Include(q => q.Variants)
                .Where(q => q.CityId == game.CityId &&
                            q.DifficultyLevel == difficulty &&
                            !game.AskedQuestionIds.Contains(q.Id))
                .OrderBy(x => Guid.NewGuid())
                .FirstOrDefaultAsync();

            if (question == null)
                return NotFound(new { message = $"Нет вопроса с уровнем сложности {difficulty}" });

            game.AskedQuestionIds.Add(question.Id);

            return Ok(new
            {
                question.Id,
                question.Text,
                Variants = question.Variants.Select(v => new { v.Id, v.Text })
            });
        }

        [HttpPost("choose")]
        public IActionResult ChooseSolution([FromBody] StepRequest request)
        {
            if (!_games.ContainsKey(request.TeamId))
                return NotFound(new { message = "Игра не найдена" });

            var game = _games[request.TeamId];

            if (game.StepNumber != 2)
                return BadRequest(new { message = "Сейчас нельзя выбирать решение" });

            if (game.Finance < request.Cost)
                return BadRequest(new { message = "Недостаточно средств" });

            game.Finance -= request.Cost;
            game.SocialPoints += request.EffectOnPoints;
            game.Progress += request.EffectOnProgress;
            game.CurrentGoal = $"Вы выбрали: {request.Action}";
            game.StepNumber = 3;

            return Ok(game);
        }
    }
}
