
using RedisPingPong.Model;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

RedisConnection redisConnection = new();
config.GetSection("RedisConnection").Bind(redisConnection);

services.AddSingleton<IConnectionMultiplexer>(option =>
   ConnectionMultiplexer.Connect(new ConfigurationOptions{
   
      
      EndPoints = {$"{redisConnection.Host}:{redisConnection.Port}"},
      AbortOnConnectFail = false,
      Ssl = redisConnection.IsSSL,
      Password = redisConnection.Password
    
   }));

var app = builder.Build();

app.MapGet("/", async (IConnectionMultiplexer redis) =>
{
   var result = await  redis.GetDatabase().PingAsync();
   try
   {
      return result.CompareTo(TimeSpan.Zero) > 0 ? $"PONG: {result}" : null;
   }
   catch (RedisTimeoutException e)
   {
      Console.WriteLine(e);
      throw;
   }
   
});

app.Run();