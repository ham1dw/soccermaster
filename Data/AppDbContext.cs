using Microsoft.EntityFrameworkCore;
using soccer.Models;

namespace soccer.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<HeroSection> HeroSections { get; set; }
        public DbSet<NewsItem> News { get; set; } 
        public DbSet<Standing> Standings { get; set; }
        public DbSet<NextMatch> NextMatches { get; set; }
        public DbSet<Match> Matches { get; set; }

    }
   

    }

