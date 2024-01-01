using Confluent.Kafka;
using KafkaExample3.Queue.Consumer;
using KafkaExample3.Queue.Core;
using KafkaExample3.Queue.Producer;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//KAFKA
builder.Services.Configure<KafkaSettings>(a => builder.Configuration.GetSection(nameof(KafkaSettings)).Bind(a));
builder.Services.AddScoped<PersonInfoProducer>();
builder.Services.AddHostedService<PersonInfoConsumer>();

//KAFKA

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
