using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SightwordsApi.DataModels;
using SightwordsApi.ApiDtos;

namespace SightwordsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SightwordsController : ControllerBase
    {
        private const int SIGHTWORDS_TO_QUEUE = 100;

        private readonly SightwordContext _context;
        private Random rand = new Random();
        public SightwordsController(SightwordContext context)
        {
            _context = context;
            if (!_context.Sightwords.Any())
            {
                List<string> words = new List<string>
                {
                    "And",
                    "Said",
                    "To",

                    "For",
                    "Like",
                    "No",

                    "Of",
                    "Off",
                    "See",

                    "Are",
                    "Not",
                    "Or",

                    "Have",
                    "On",
                    "My",

                    "By",
                    "Good",
                    "Out",

                    "Put",
                    "You",
                    "Can",

                    "Look",
                    "With",
                    "We",

                    "This",
                    "Is",
                    "I",

                    "Me",
                    "She",
                    "We",

                    "He",
                    "Be",
                    "Has",

                    "Saw",
                    "The",
                    "Her",

                    "Then",
                    "An",
                    "Was",

                    "As",
                    "Go",
                    "Want",

                    "Went",
                    "A",
                    "Here",

                    "There",
                    "Am",
                    "Come",

                    "Home",
                    "That",
                    "They",
                    "What"
                };
                var sightwords = words.Select(w => new Sightword { Word = w });
                _context.Sightwords.AddRange(sightwords);
                _context.SaveChanges();
            }
        }

        // GET api/sightwords
        [HttpGet]
        public ActionResult<LessonSet> Get()
        {
            var set = new Queue<Sightword>(SIGHTWORDS_TO_QUEUE);
            var sightWords = _context.Sightwords.ToList();
            var start = rand.Next(0, sightWords.Count);
            Console.WriteLine("Starting with Sight Word #" + start + " of " + sightWords.Count);
            do
            {
                set.Enqueue(sightWords[(start + set.Count) % sightWords.Count]);
            } while (set.Count < SIGHTWORDS_TO_QUEUE);
            return new LessonSet
            {
                Words = set
            } ;
        }

        [HttpPut("answer")]
        [HttpPost("answer")]
        // PUT/POST api/sightwords/answer
        public void Answer([FromBody]Answer answer)
        {
            var word = _context.Sightwords.FirstOrDefault(s => s.Id == answer.SightwordId);
            if(answer.PersistResult)
            {
                Console.WriteLine($"[Not Persisted] The answer for {word.Word} was {(answer.Correct ? "correct": "incorrect")}.");
            }
            else
            {
                //TODO: Log result.
            }
        }
    }
}
