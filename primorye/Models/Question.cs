namespace primorye.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Price { get; set; }
        public int DifficultyLevel { get; set; }

        public int CityId { get; set; }
        public City City { get; set; } = null!;
        public ICollection<Variant> Variants { get; set; } = new List<Variant>();
    }
}
