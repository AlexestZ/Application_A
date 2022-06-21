using MessagePack;
using RabbitMQ.Client;
using System.Threading.Tasks;

public class RabbitMQService : IRabbitMQService
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    public RabbitMQService()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare("Test", ExchangeType.Fanout);

        _channel.QueueDeclare("person", true, false, autoDelete: false);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
    public async Task SendUserAsync(User u)
    {
        await Task.Factory.StartNew(() =>
        {
            var body = MessagePackSerializer.Serialize(u);

            _channel.BasicPublish("", "user", body: body);
        });
    }
}