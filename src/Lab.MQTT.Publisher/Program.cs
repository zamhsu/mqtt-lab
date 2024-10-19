using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using Lab.MQTT.Publisher.BackgroundServices;
using MQTTnet.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MqttClientOptions>(provider =>
{
    var options = new MqttClientOptionsBuilder()
        .WithTcpServer("localhost", 1833)
        .Build();

    return options;
});
builder.Services.AddSingleton(new MqttFactory());
builder.Services.AddSingleton<IMqttClient>(provider => provider.GetRequiredService<MqttFactory>().CreateMqttClient());

builder.Services.AddHostedService<PublisherClientBackgroundService>();

var app = builder.Build();

app.MapPut("/publish", async ([FromServices] IMqttClient mqttClient, [FromBody] MqttData mqttData, CancellationToken stoppingToken) =>
{
    var applicationMessage = new MqttApplicationMessageBuilder()
        .WithTopic(mqttData.TopicId)
        .WithPayload(mqttData.Data)
        .Build();

    await mqttClient.PublishAsync(applicationMessage, stoppingToken);
});

app.Run();

public record MqttData
{
    public string Data { get; set; }
    public string TopicId { get; set; }
}