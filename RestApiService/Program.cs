using Microsoft.EntityFrameworkCore;
using RestApiService.DAL;
using RestApiService.Models.Configurations;
using RestApiService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// configuration:

// mysql 

MysqlConfigs mysqlConfigs = new MysqlConfigs()
{
    ConnectionString = "Server=localhost;Port=3306;Password=1234;User=root;Database=gbfs-db"
};
builder.Configuration.GetSection("MysqlConfigs").Bind(mysqlConfigs);
ServerVersion serverVersion = ServerVersion.AutoDetect(mysqlConfigs.ConnectionString);
builder.Services.AddDbContext<MySqlDbContext>(options => options.UseMySql(mysqlConfigs.ConnectionString, serverVersion));

// mongo
MongoConfiguration mongoConfiguration = new MongoConfiguration()
{
    ConnectionString = "mongodb://root:1234@localhost:27017",
    DatabaseName = "gbfs-db",
    StationStatusCollectionName = "reports-status"
};
builder.Configuration.GetSection("MongoConfiguration").Bind(mongoConfiguration);
builder.Services.AddSingleton(mongoConfiguration);
builder.Services.AddSingleton<MongoDbContext>();

//redis
RedisConfigs redisConfigs = new RedisConfigs
{
    ConnectionString = "localhost:6379"
};
builder.Configuration.GetSection("RedisConfigs").Bind(redisConfigs);
builder.Services.AddSingleton(redisConfigs);
builder.Services.AddSingleton<RedisContext>();


// services
builder.Services.AddScoped<IStationsRepository, StationsRepository>();


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
