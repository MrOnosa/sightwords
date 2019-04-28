using System.Collections.Generic;

namespace SightwordsApi.DataModels
{
    public class Sightword
    {
        public int Id { get; set; }
        public string Word { get; set; }

        #region Navigational Properties
        
        public List<Answer> Answers { get; set; }

        #endregion Navigational Properties

    }
}