namespace primorye.Models
{
    public class GameState
    {
        public int TeamId { get; set; }
        public int CityId { get; set; }
        public int Finance { get; set; }
        public int SocialPoints { get; set; }
        public int Progress { get; set; }
        public int StepNumber { get; set; } = 1;
        public bool WaitingForAnswer { get; set; } = true;
        public string CurrentGoal { get; set; } = "";

        public int CurrentRound { get; set; } = 1;
        public int QuestionsAnsweredInRound { get; set; } = 0;

        public HashSet<int> AskedQuestionIds { get; set; } = new();
    }

}
