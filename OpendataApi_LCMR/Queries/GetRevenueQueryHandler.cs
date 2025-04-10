namespace OpendataApi_LCMR.Queries
{
    using AutoMapper;
    using MediatR;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using OpendataApi_LCMR.DTO;
    using OpendataApi_LCMR.Models;
    using OpendataApi_LCMR.Services;
    using System.Data;

    public class GetRevenueQueryHandler : IRequestHandler<GetRevenueQuery, List<RevenueDto>>
    {
        private readonly string _connectionString;
        private readonly OpendataApiDbContext _opendataApiDb;
        private readonly IMapper _mapper;


        public GetRevenueQueryHandler(DBService dbService, IMapper mapper)
        {
            _connectionString = dbService.ConnectionString;
            _mapper = mapper;
        }
        public async Task<List<RevenueDto>> Handle(GetRevenueQuery request, CancellationToken cancellationToken)
        {
            var revenues = new List<Revenue>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetRevenue", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DataYYYMM", (object?)request.DataYYYMM ?? DBNull.Value);
                command.Parameters.AddWithValue("@CompanyCode", (object?)request.CompanyCode ?? DBNull.Value);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        revenues.Add(new Revenue
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ReportDate = reader["ReportDate"]?.ToString(),
                            DataYYYMM = reader["DataYYYMM"]?.ToString(),
                            CompanyCode = reader["CompanyCode"]?.ToString(),
                            CompanyName = reader["CompanyName"]?.ToString(),
                            Industry = reader["Industry"]?.ToString(),
                            CurrentRevenue = reader["CurrentRevenue"] != DBNull.Value ? Convert.ToInt64(reader["CurrentRevenue"]) : 0,
                            LastMonthRevenue = reader["LastMonthRevenue"] != DBNull.Value ? Convert.ToInt64(reader["LastMonthRevenue"]) : 0,
                            LastYearSameMonthRevenue = reader["LastYearSameMonthRevenue"] != DBNull.Value ? Convert.ToInt64(reader["LastYearSameMonthRevenue"]) : 0,
                            MonthlyChangePercentage = reader["MonthlyChangePercentage"] != DBNull.Value ? Convert.ToDecimal(reader["MonthlyChangePercentage"]) : 0,
                            YearlyChangePercentage = reader["YearlyChangePercentage"] != DBNull.Value ? Convert.ToDecimal(reader["YearlyChangePercentage"]) : 0,
                            CumulativeCurrentRevenue = reader["CumulativeCurrentRevenue"] != DBNull.Value ? Convert.ToInt64(reader["CumulativeCurrentRevenue"]) : 0,
                            CumulativeLastYearRevenue = reader["CumulativeLastYearRevenue"] != DBNull.Value ? Convert.ToInt64(reader["CumulativeLastYearRevenue"]) : 0,
                            CumulativeChangePercentage = reader["CumulativeChangePercentage"] != DBNull.Value ? Convert.ToDecimal(reader["CumulativeChangePercentage"]) : 0,
                            Remark = reader["Remark"]?.ToString()
                        });
                    }
                }
            }

            return _mapper.Map<List<RevenueDto>>(revenues);
        }
    }
}
