using MQTTnet.Client;

namespace Lab.MQTT.Publisher.BackgroundServices;

public class PublisherClientBackgroundService : BackgroundService
{
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _mqttClientOptions;

    public PublisherClientBackgroundService(IMqttClient mqttClient, MqttClientOptions mqttClientOptions)
    {
        _mqttClient = mqttClient;
        _mqttClientOptions = mqttClientOptions;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _mqttClient.ConnectAsync(_mqttClientOptions, stoppingToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _mqttClient.DisconnectAsync(cancellationToken: cancellationToken);

        return base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _mqttClient.Dispose();
        base.Dispose();
    }
}