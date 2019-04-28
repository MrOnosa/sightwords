namespace SightwordsApi.ApiDtos
{
    public class AnswerDTO
    {
        public int SightwordId { get; set; }
        public bool Correct { get; set; }
        public bool PersistResult { get; set; }
    }
}
