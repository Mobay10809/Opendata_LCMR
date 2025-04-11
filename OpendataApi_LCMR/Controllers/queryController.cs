using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OpendataApi_LCMR.DTO;
using OpendataApi_LCMR.Models;
using OpendataApi_LCMR.Queries;
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
        private readonly IMediator _mediator;

        public queryController(IMediator mediator,  DBService dbService) : base()
        {
            _connectionString = dbService.ConnectionString;
            _mediator = mediator;
        }
        [HttpGet("query")]
        public async Task<IActionResult> GetRevenue(string? dataYYYMM, string? companyCode)
        {
            var result = await _mediator.Send(new GetRevenueQuery(dataYYYMM, companyCode));
            return Ok(result);
        }
        [HttpGet("queryEF")]
        public async Task<IActionResult> GetRevenueEF(string? dataYYYMM, string? companyCode)
        {
            var result = await _mediator.Send(new GetRevenueQueryEF(dataYYYMM, companyCode));
            return Ok(result);
        }

        [HttpGet("queryEF_paged")]
        public async Task<IActionResult> GetRevenuePaged( string? dataYYYMM, string? companyCode,  int page = 1,  int pageSize = 20)
        {

            var revenues = await _mediator.Send(new GetRevenueQueryEF(dataYYYMM, companyCode));

            var totalRecords = revenues.Count();

            var result = revenues
                .OrderBy(r => r.Id) 
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            return Ok(new
            {
                Total = totalRecords,
                Page = page,
                PageSize = pageSize,
                Data = result
            });
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
