namespace SightwordsApi.ApiDtos
{
    public class Answer
    {
        public int SightwordId { get; set; }
        public bool Correct { get; set; }
        public bool PersistResult { get; set; }
    }
}
