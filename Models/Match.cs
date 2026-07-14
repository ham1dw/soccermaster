namespace soccer.Models
{
    public class Match
    {
        public int Id { get; set; }
        public string HomeTeam { get; set; }
        public string HomeResult { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public string AwayTeam { get; set; }
        public string AwayResult { get; set; }

        public string HomePlayer1 { get; set; }
        public string HomePlayer2 { get; set; }
        public string HomePlayer3 { get; set; }
        public string HomePlayer4 { get; set; }

        public string AwayPlayer1 { get; set; }
        public string AwayPlayer2 { get; set; }
        public string AwayPlayer3 { get; set; }
        public string AwayPlayer4 { get; set; }
    }

}
