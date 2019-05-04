namespace SightwordsApi.ApiDtos
{
    public class SightwordAnswersSummaryDTO
    {
        public int AnswerId { get; set; }
        public int AnsweredCorrectly { get; set; }
        public int TotalAnswers { get; set; }
    }
}
