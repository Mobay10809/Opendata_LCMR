﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OpendataApi_LCMR.DTO;
using OpendataApi_LCMR.Models;
using OpendataApi_LCMR.Services;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OpendataApi_LCMR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class queryController : Controller
    {
        private readonly string _connectionString;
        private readonly OpendataApiDbContext _opendataApiDb; 
        private readonly IMapper _mapper;

        public queryController(IMapper mapper, OpendataApiDbContext opendataApiDb, DBService dbService) : base()
        {
            _connectionString = dbService.ConnectionString;
            _opendataApiDb = opendataApiDb;
            _mapper = mapper;
        }
        [HttpGet("query")]
        public IActionResult GetRevenueByMonth(string? dataYYYMM, string? companyCode)
        {
            var revenues = new List<Revenue>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetRevenue", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DataYYYMM", dataYYYMM);
                command.Parameters.AddWithValue("@CompanyCode", companyCode);

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
                            MonthlyChangePercentage = reader["MonthlyChangePercentage"] != DBNull.Value ? Convert.ToInt64(reader["MonthlyChangePercentage"]) : 0,
                            YearlyChangePercentage = reader["YearlyChangePercentage"] != DBNull.Value ? Convert.ToInt64(reader["YearlyChangePercentage"]) : 0,
                            CumulativeCurrentRevenue = reader["CumulativeCurrentRevenue"] != DBNull.Value ? Convert.ToInt64(reader["CumulativeCurrentRevenue"]) : 0,
                            CumulativeLastYearRevenue = reader["CumulativeLastYearRevenue"] != DBNull.Value ? Convert.ToInt64(reader["CumulativeLastYearRevenue"]) : 0,
                            CumulativeChangePercentage = reader["CumulativeChangePercentage"] != DBNull.Value ? Convert.ToInt64(reader["CumulativeChangePercentage"]) : 0,
                            Remark = reader["Remark"]?.ToString()
                        });
                    }
                }
            }
            
            var revenueDtos = _mapper.Map<List<RevenueDto>>(revenues);
            return Ok(revenueDtos);
        }
        [HttpGet("querySp")]
        public IActionResult GetRevenueByMonthSp(string? dataYYYMM, string? companyCode)
        {
            var revenues = _opendataApiDb.Revenues
                .FromSqlInterpolated($"EXEC sp_GetRevenue @DataYYYMM = {dataYYYMM}, @CompanyCode = {companyCode}")
                .ToList();
            var revenueDtos = _mapper.Map<List<RevenueDto>>(revenues);
            return Ok(revenueDtos);

        }


        [HttpPost("reloadRevenueData")]
        public async Task<IActionResult> reloadRevenueData()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                var url = "https://mopsfin.twse.com.tw/opendata/t187ap05_L.csv";
                using var httpClient = new HttpClient();
                var csvContent = await httpClient.GetStringAsync(url);

                var lines = csvContent.Split('\n').Skip(1); // 跳過標題列

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;


                    // 一併去除雙引號
                    var fields = line.Split(',').Select(f => f.Trim().Trim('"')).ToArray();

                    using var command = new SqlCommand("sp_InsertRevenue", connection, transaction);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ReportDate", fields[0].Trim());
                    command.Parameters.AddWithValue("@DataYYYMM", fields[1].Trim());
                    command.Parameters.AddWithValue("@CompanyCode", fields[2].Trim());
                    command.Parameters.AddWithValue("@CompanyName", fields[3].Trim());
                    command.Parameters.AddWithValue("@Industry", fields[4].Trim());
                    command.Parameters.AddWithValue("@CurrentRevenue", TryParseLong(fields[5]));
                    command.Parameters.AddWithValue("@LastMonthRevenue", TryParseLong(fields[6]));
                    command.Parameters.AddWithValue("@LastYearSameMonthRevenue", TryParseLong(fields[7]));
                    command.Parameters.AddWithValue("@MonthlyChangePercentage", TryParseDecimal(fields[8]));
                    command.Parameters.AddWithValue("@YearlyChangePercentage", TryParseDecimal(fields[9]));
                    command.Parameters.AddWithValue("@CumulativeCurrentRevenue", TryParseLong(fields[10]));
                    command.Parameters.AddWithValue("@CumulativeLastYearRevenue", TryParseLong(fields[11]));
                    command.Parameters.AddWithValue("@CumulativeChangePercentage", TryParseDecimal(fields[12]));
                    command.Parameters.AddWithValue("@Remark", fields[13]);
                    await command.ExecuteNonQueryAsync();
                }
                await transaction.CommitAsync(); 

                return Ok("資料匯入成功！");
            }
            catch
            {
                await transaction.RollbackAsync(); 
                return BadRequest("資料匯入失敗");
            }
        }
        ///將長字串轉為Long
        private long TryParseLong(string value)
        {
            return long.TryParse(value.Trim(), out var result) ? result : 0;
        }

        ///將小數點字串轉為Decimal
        private decimal TryParseDecimal(string value)
        {
            return decimal.TryParse(value.Trim(), out var result) ? result : 0;
        }
    }
}
