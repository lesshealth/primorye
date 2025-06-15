namespace primorye.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Finance { get; set; }
        public int SocialPoints { get; set; }
        public int Progress { get; set; }
        public int? PlaceInTheTable { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
