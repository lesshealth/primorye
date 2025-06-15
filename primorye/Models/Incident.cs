namespace primorye.Models
{
    public class Incident
    {
        public int Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public int Difficulty { get; set; }

        public int CityId { get; set; }

        public City City { get; set; } = null!;

        public ICollection<SolutionToTheCase> Solutions { get; set; } = new List<SolutionToTheCase>();
    }
}
