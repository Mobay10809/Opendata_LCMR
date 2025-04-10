namespace OpendataApi_LCMR.DTO
{
    public class RevenueDto
    {
        public int Id { get; set; }
        public string ReportDate { get; set; }
        public string DataYYYMM { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string Industry { get; set; }
        public long CurrentRevenue { get; set; }
        public long LastMonthRevenue { get; set; }
        public long LastYearSameMonthRevenue { get; set; }
        public decimal MonthlyChangePercentage { get; set; }
        public decimal YearlyChangePercentage { get; set; }
        public long CumulativeCurrentRevenue { get; set; }
        public long CumulativeLastYearRevenue { get; set; }
        public decimal CumulativeChangePercentage { get; set; }
        public string Remark { get; set; }
    }
}
