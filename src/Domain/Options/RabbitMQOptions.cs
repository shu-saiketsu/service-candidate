namespace Saiketsu.Service.Candidate.Domain.Options;

public sealed class RabbitMQOptions
{
    public const string Position = "RabbitMQ";

    public string HostName { get; set; } = null!;
    public int Port { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}