namespace primorye.Models
{
    public class SolutionToTheCase
    {
        public int Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public int Price { get; set; }

        public int PublicOpinion { get; set; }

        public int Progress { get; set; }

        public int IdIncidents { get; set; }

        public Incident Incident { get; set; } = null!;
    }
}
