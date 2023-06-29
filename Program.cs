using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NhonOJT_redis;
using NhonOJT_redis.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string databaseConnection = "Server=localhost;Port=5432;Database=redisdb;User Id=postgres;Password=123;";
// string databaseConnectionDocker = "Server=postgres;Port=5432;Database=redisdb;User Id=postgres;Password=123;";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(databaseConnection);
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<ICacheService, CacheService>();

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
