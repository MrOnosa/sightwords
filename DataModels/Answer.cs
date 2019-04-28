using System;

namespace SightwordsApi.DataModels
{
    public class Answer
    {
        public int Id { get; set; }
        public int SightwordId { get; set; }
        public DateTime Date { get; set; } 
        public bool AnsweredCorrectly { get; set; }

        #region Navigational Properties
        
        public Sightword Sightword { get; set; }

        #endregion Navigational Properties

    }
}