using MQTTnet;
using MQTTnet.Client;

namespace Lab.MQTT.Subscriber.BackgroundServices;

public class MqttClientBackgroundService : BackgroundService
{
    private readonly ILogger<MqttClientBackgroundService> _logger;
    private readonly IMqttClient _mqttClient;
    private readonly MqttFactory _mqttFactory;

    public MqttClientBackgroundService(ILoggerFactory loggerFactory, MqttFactory mqttFactory)
    {
        _logger = loggerFactory.CreateLogger<MqttClientBackgroundService>();
        _mqttFactory = mqttFactory;
        _mqttClient = mqttFactory.CreateMqttClient();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await ConnectAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            var message = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fffzzz} 連接服務器失敗 Msg：{ex}";

            _logger.LogError(ex, message);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            await NormalDisconnectionAsync(cancellationToken);
        }

        await ServerShuttingDownAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _mqttClient.Dispose();
        base.Dispose();
    }

    private async Task ConnectAsync(CancellationToken cancellationToken)
    {
        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1833)
            .WithClientId(Environment.MachineName)
            .Build();
        
        await _mqttClient.ConnectAsync(mqttClientOptions, cancellationToken);

        _mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            Console.WriteLine("Received application message.");
            Console.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fffzzz} Msg：{e.ApplicationMessage.ConvertPayloadToString()}");

            return Task.CompletedTask;
        };
        
        var mqttSubscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(f => f.WithTopic("topic/chat"))
            .Build();
        
        await _mqttClient.SubscribeAsync(mqttSubscribeOptions, cancellationToken);

        if (_mqttClient.IsConnected)
        {
            Console.WriteLine("Connected");
            return;
        }
        
        Console.WriteLine("retry");
        await _mqttClient.ReconnectAsync(cancellationToken);
    }

    private async Task NormalDisconnectionAsync(CancellationToken cancellationToken)
    {
        var disconnectOption = new MqttClientDisconnectOptions()
        {
            Reason = MqttClientDisconnectOptionsReason.NormalDisconnection,
            ReasonString = "Close the connection normally. Do not send the Will Message."
        };
        
        await _mqttClient.DisconnectAsync(disconnectOption, cancellationToken);
    }

    private async Task ServerShuttingDownAsync(CancellationToken cancellationToken)
    {
        var disconnectOption = new MqttClientDisconnectOptions()
        {
            Reason = (MqttClientDisconnectOptionsReason)139,
            ReasonString = "The Server is shutting down."
        };
        
        await _mqttClient.DisconnectAsync(disconnectOption, cancellationToken);
    }
}