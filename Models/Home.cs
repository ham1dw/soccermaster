namespace soccer.Models
{
    public class HeroSection
    {
        public int Id { get; set; }
        public string  Title { get; set; }
        public string Description { get; set; }
        public string BackgroundImage { get; set; }
    }

    public class NewsItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public string AuthorImageUrl { get; set; }
    }

    public class Standing
    {
        public int Id { get; set; }
        public int? Position { get; set; }
        public string TeamName { get; set; }
        public int W { get; set; }
        public int D { get; set; }
        public int L { get; set; }
        public int Points { get; set; }
        
    }

    public class NextMatch
    {
        public int Id { get; set; }
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        public DateTime MatchDate { get; set; }
        public string Location { get; set; }
    }

    public class HomeViewModel
    {
        public HeroSection? Hero { get; set; }
        public List<NewsItem>? News { get; set; }
        public List<Standing>? Standings { get; set; }
        public NextMatch? NextMatch { get; set; }
        public List<Match>? Matches { get; set; }
        public Match? LastMatch { get; set; }
    }
}