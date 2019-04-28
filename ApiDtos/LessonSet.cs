using System.Collections.Generic;
using SightwordsApi.DataModels;

namespace SightwordsApi.ApiDtos
{
    public class LessonSet {
        public Queue<Sightword> Words { get; set; }
    }
}
