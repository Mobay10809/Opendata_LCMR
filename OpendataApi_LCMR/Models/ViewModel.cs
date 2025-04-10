namespace OpendataApi_LCMR.Models
{
    public class Revenue
    {
        public int Id { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
        public string? YearMonth { get; set; }
        public long? RevenueAmount { get; set; } 
    }
}
