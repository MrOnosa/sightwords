using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace SightwordsApi.DataModels
{
    public class SightwordContext : DbContext
    {
        public SightwordContext(DbContextOptions<SightwordContext> options) : base(options)
        {
        }
        public DbSet<Sightword> Sightwords { get; set; }
        public DbSet<Answer> Answers { get; set; }

    }
}