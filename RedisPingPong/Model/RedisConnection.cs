namespace RedisPingPong.Model;

public class RedisConnection
{
    public string Host { get; set; }
    public int Port { get; set; }
    public bool IsSSL { get; set; }
    public string Password { get; set; }
}