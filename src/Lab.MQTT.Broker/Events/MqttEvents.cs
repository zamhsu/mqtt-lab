using MQTTnet.Server;

namespace Lab.MQTT.Broker.Events;

internal sealed class MqttEvents
{
    private readonly ILogger<MqttEvents> _logger;

    public MqttEvents(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<MqttEvents>();
    }
    
    public Task OnClientConnected(ClientConnectedEventArgs eventArgs)
    {
        _logger.LogInformation($"Client '{eventArgs.ClientId}' connected.");
        return Task.CompletedTask;
    }

    public Task OnClientDisconnected(ClientDisconnectedEventArgs eventArgs)
    {
        _logger.LogInformation($"Client '{eventArgs.ClientId}' disconnected.");
        return Task.CompletedTask;
    }

    public Task ValidateConnection(ValidatingConnectionEventArgs eventArgs)
    {
        _logger.LogInformation($"Client '{eventArgs.ClientId}' wants to connect. Accepting!");
        return Task.CompletedTask;
    }
}