using LCMRCommon;
using Microsoft.EntityFrameworkCore;
using OpendataApi_LCMR.Services;

var builder = WebApplication.CreateBuilder(args);

// 檢查ConnectionString  
var configFile = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
// 取回ConnectionString  
var decryptedConnStr = ConfigurationCommon.EncryptedConnectionString(builder.Configuration, configFile);

// 註冊單例服務，連線字串
builder.Services.AddSingleton(new DBService(decryptedConnStr));

// 註冊資料庫  
builder.Services.AddDbContext<OpendataApiDbContext>(options =>
   options.UseSqlServer(decryptedConnStr));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.  
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
