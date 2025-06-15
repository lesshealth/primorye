namespace primorye.DTOs
{
    public class StepRequest
    {
        public int TeamId { get; set; }
        public string Action { get; set; } = string.Empty;
        public int Cost { get; set; }
        public int EffectOnPoints { get; set; }
        public int EffectOnProgress { get; set; }
    }
}
