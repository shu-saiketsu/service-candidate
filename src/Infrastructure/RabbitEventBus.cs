using System.Text;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Saiketsu.Service.Candidate.Application.Common;
using Saiketsu.Service.Candidate.Domain.Options;

namespace Saiketsu.Service.Candidate.Infrastructure;

public sealed class RabbitEventBus : IEventBus
{
    private const string BrokerName = "event_bus";
    private readonly IModel _channel;

    private readonly ILogger<RabbitEventBus> _logger;
    private readonly QueueDeclareOk _queue;
    private readonly RabbitMQOptions _rabbitOptions;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly Dictionary<string, Type> _typeDictionary;

    public RabbitEventBus(IOptions<RabbitMQOptions> rabbitOptions, ILogger<RabbitEventBus> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _rabbitOptions = rabbitOptions.Value;
        _typeDictionary = new Dictionary<string, Type>();

        // yields
        // best practice says no
        // short uni deadline says yes! :D
        // meh it's a singleton, doesn't really matter
        var connection = GetConnection();
        _channel = connection.CreateModel();
        _queue = _channel.QueueDeclare();

        SetupReceiver();
    }

    public void Publish(IRequest @event)
    {
        var eventName = @event.GetType().Name;

        _channel.ExchangeDeclare(BrokerName, ExchangeType.Topic);

        var json = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(BrokerName, eventName, body: body);

        _logger.LogInformation("[RabbitMQ] Published {eventName}.", eventName);
    }

    public void Subscribe<T>() where T : IRequest
    {
        var eventName = typeof(T).Name;

        _channel.ExchangeDeclare(BrokerName, ExchangeType.Topic);
        _channel.QueueBind(_queue.QueueName, BrokerName, eventName);

        _typeDictionary[eventName] = typeof(T);

        _logger.LogInformation("[RabbitMQ] Subscribed to {eventName}.", eventName);
    }

    private IConnection GetConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitOptions.HostName,
            Port = _rabbitOptions.Port,
            UserName = _rabbitOptions.UserName,
            Password = _rabbitOptions.Password,
            DispatchConsumersAsync = true,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
            ClientProvidedName = "CandidateService"
        };

        IConnection? connection = null;
        do
        {
            try
            {
                connection = factory.CreateConnection();
                _logger.LogInformation("[RabbitMQ] A successful connection was made.");
            }
            catch (Exception)
            {
                _logger.LogWarning("[RabbitMQ] No connection could be found.");
                Thread.Sleep(1000);
            }
        } while (connection == null);

        return connection;
    }

    private async Task HandleEventAsync(string routingKey, string message)
    {
        var @event = JsonConvert.DeserializeObject(message, _typeDictionary[routingKey]);

        // able to use scoped services in singletons... :/
        // https://samwalpole.com/using-scoped-services-inside-singletons
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await mediator.Send(@event!);
    }

    private void SetupReceiver()
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.Received += async (_, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var routingKey = eventArgs.RoutingKey;

            _logger.LogInformation("[RabbitMQ] Handling {eventName}.", routingKey);

            try
            {
                await HandleEventAsync(routingKey, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RabbitMQ] Unable to handle {eventName}.", routingKey);
            }
        };

        _channel.BasicConsume(_queue.QueueName, true, consumer);
    }
}