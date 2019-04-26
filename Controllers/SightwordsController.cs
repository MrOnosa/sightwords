using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SightwordsApi.Models;

namespace SightwordsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SightwordsController : ControllerBase
    {
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
        public ActionResult<SightWordSet> Get()
        {
            var set = new SightWordSet();
            var sightWords = _context.Sightwords.ToList();
            var start = rand.Next(0, sightWords.Count);
            Console.WriteLine("Starting with Sight Word #" + start + " of " + sightWords.Count);

            set.Word1 = sightWords[start].Word;
            set.Word2 = sightWords[(start + 1) % sightWords.Count].Word;
            set.Word3 = sightWords[(start + 2) % sightWords.Count].Word;
            return set;
        }

        // GET api/sightwords/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/sightwords
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/sightwords/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/sightwords/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class SightWordSet
    {
        public string Word1 { get; set; }
        public string Word2 { get; set; }
        public string Word3 { get; set; }
    }
}
