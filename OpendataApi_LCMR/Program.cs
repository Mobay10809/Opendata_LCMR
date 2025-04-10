using LCMRCommon;
using Microsoft.EntityFrameworkCore;
using OpendataApi_LCMR.Services;

var builder = WebApplication.CreateBuilder(args);

// �ˬdConnectionString  
var configFile = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
// ���^ConnectionString  
var decryptedConnStr = ConfigurationCommon.EncryptedConnectionString(builder.Configuration, configFile);

// ���U��ҪA�ȡA�s�u�r��
builder.Services.AddSingleton(new DBService(decryptedConnStr));

// ���U��Ʈw  
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
