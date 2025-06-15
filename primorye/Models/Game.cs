namespace primorye.Models
{
    public class Game
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
