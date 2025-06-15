namespace primorye.Models
{
    public class TournamentTable
    {
        public int Id { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;

        public int CityId { get; set; }
        public City City { get; set; } = null!;
    }
}
