using MQTTnet;
using Lab.MQTT.Subscriber.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new MqttFactory());

builder.Services.AddHostedService<MqttClientBackgroundService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();