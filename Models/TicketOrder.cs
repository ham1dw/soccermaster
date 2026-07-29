namespace soccer.Models
{
    public class TicketOrder
    {
        public int Id { get; set; }
        public string MatchName { get; set; }
        public string Category { get; set; }
        public string FullName { get; set; }
        public int TicketCount { get; set; }
        
        public decimal TotalAmount { get; set; } 
        public bool IsPaid { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
