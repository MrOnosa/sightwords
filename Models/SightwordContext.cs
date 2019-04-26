using Microsoft.EntityFrameworkCore;
namespace SightwordsApi.Models
{
    public class SightwordContext : DbContext
    {
        public SightwordContext(DbContextOptions<SightwordContext> options) : base(options)
        {
        }
        public DbSet<Sightword> Sightwords { get; set; }

    }
}