using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OpendataApi_LCMR.Models;
using OpendataApi_LCMR.Services;
using System.Data;

namespace OpendataApi_LCMR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class queryController : Controller
    {
        private readonly string _connectionString;
        private readonly OpendataApiDbContext _opendataApiDb;

        public queryController(OpendataApiDbContext opendataApiDb, DBService dbService) : base()
        {
            _connectionString = dbService.ConnectionString;
            _opendataApiDb = opendataApiDb;
        }
        [HttpGet("query")]
        public IActionResult GetRevenueByMonth(string yearMonth)
        {
            var revenues = new List<Revenue>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetRevenueByMonth", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@YearMonth", yearMonth);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        revenues.Add(new Revenue
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CompanyCode = reader["CompanyCode"]?.ToString(),
                            CompanyName = reader["CompanyName"]?.ToString(),
                            YearMonth = reader["YearMonth"]?.ToString(),
                            RevenueAmount = reader["RevenueAmount"] != DBNull.Value ? Convert.ToInt64(reader["RevenueAmount"]) : (long?)null
                        });
                    }
                }
            }

            return Ok(revenues);
        }
        [HttpGet("querySp")]
        public IActionResult GetRevenueByMonthSp(string yearMonth = "")
        {
            var revenues = _opendataApiDb.Revenues
                .FromSqlInterpolated($"EXEC sp_GetRevenueByMonth @YearMonth = {yearMonth}")
                .ToList();

            return Ok(revenues);

        }
    }
}
