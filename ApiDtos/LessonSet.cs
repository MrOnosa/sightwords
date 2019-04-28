using System.Collections.Generic;
using SightwordsApi.DataModels;

namespace SightwordsApi.ApiDtos
{
    public class LessonSetDTO {
        public Queue<Sightword> Words { get; set; }
    }
}
