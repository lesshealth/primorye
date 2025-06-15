namespace primorye.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<TournamentTable> TournamentTables { get; set; } = new List<TournamentTable>();
    }
}
