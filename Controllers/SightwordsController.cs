using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SightwordsApi.ApiDtos;
using SightwordsApi.DataModels;

namespace SightwordsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SightwordsController : ControllerBase
    {
        private const int SightwordsToQueue = 100;

        private readonly SightwordContext _context;
        private readonly Random _rand = new Random();
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
        public ActionResult<LessonSetDTO> Get()
        {
            var set = new List<Sightword>(SightwordsToQueue);
            var sightWords = _context.Sightwords.ToList();
            var start = _rand.Next(0, sightWords.Count);
            Console.WriteLine("Starting with Sight Word #" + start + " of " + sightWords.Count);
            do
            {
                set.Add(sightWords[(start + set.Count) % sightWords.Count]);
            } while (set.Count < SightwordsToQueue);
            return new LessonSetDTO
            {
                Words = Jostle(set)
            };
        }

        private List<T> Jostle<T>(ICollection<T> items)
        {
            var jostledItems = new List<T>(items);
            for(var i = 0; i < items.Count; i++){
                var coinflip = _rand.Next(0, 2);
                if(coinflip == 0){
                    var temp = jostledItems[i];
                    jostledItems[i] = jostledItems[(i + 1)%jostledItems.Count];
                    jostledItems[(i + 1)%jostledItems.Count] = temp;
                }
            }

            return jostledItems;
        }

        [HttpPost("answer")]
        // PUT/POST api/sightwords/answer
        public SightwordAnswersSummaryDTO Answer([FromBody]AnswerDTO answer)
        {
            Answer answerResult = new Answer
            {
                Id = 0,
                SightwordId = answer.SightwordId,
                AnsweredCorrectly = answer.Correct,
                Date = DateTime.UtcNow
            };
            if (answer.PersistResult)
            {
                _context.Answers.Add(answerResult);
                _context.SaveChanges();
            }
            else
            {
                Console.WriteLine($"[Not Persisted] The answer for {answer.SightwordId} was {(answer.Correct ? "correct" : "incorrect")}.");
            }

            var counts = _context
                            .Answers
                            .Where(a => a.SightwordId == answer.SightwordId)
                            .Select(a => new
                            {
                                Correct = a.AnsweredCorrectly ? 1 : 0
                            })
                            .GroupBy(a => 1) 
                            .Select(a => new
                            {
                                TotalAnswers = a.Count(),
                                AnsweredCorrectly = a.Sum(e => e.Correct)
                            })
                            .FirstOrDefault();

            return new SightwordAnswersSummaryDTO
            {
                AnswerId = answerResult.Id,
                AnsweredCorrectly = (counts?.AnsweredCorrectly).GetValueOrDefault(),
                TotalAnswers = (counts?.TotalAnswers).GetValueOrDefault()
            };
        }

        [HttpDelete("answer")]
        public int DeleteAnswer([FromBody]int answerId)
        {
            _context.Remove(new Answer {Id = answerId});
            return _context.SaveChanges();
        }

        [HttpPut("answer")]
        public int SwitchAnswer([FromBody]AnswerDTO answerDto)
        {
            var answer = _context.Answers.FirstOrDefault(a => a.Id == answerDto.Id);
            if (answer != null)
            {
                answer.AnsweredCorrectly = answerDto.Correct;
            }
            return _context.SaveChanges();
        }
    }
}
