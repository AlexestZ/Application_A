using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using MyRabbitMQService.BL.Common;
using MyRabbitMQService.BL.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

public class KafkaConsumer<Tkey, TValue> : IHostedService
{
    private readonly IConsumer<Tkey, TValue> consumer;

    public KafkaConsumer()
    {
        var config = new ConsumerConfig()
        {
            BootstrapServers = "localhost:9092",
            GroupId = Guid.NewGuid().ToString(),
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true,
            AutoCommitIntervalMs = 5000,
            FetchWaitMaxMs = 50,
        };

        consumer = new ConsumerBuilder<Tkey, TValue>(config)
           .SetValueDeserializer(new MsgPackDeserializer<TValue>())
           .Build();

        consumer.Subscribe("test");
    }

    CancellationTokenSource cts = new CancellationTokenSource();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            Task.Factory.StartNew(() =>
            {
                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        var cr = consumer.Consume(cts.Token);
                        UserCacheDictionary.userDictionary.Add(cr.Message.Key, cr.Message.Value);
                        Console.WriteLine($"Id is: {cr.Message.Key}, Name is: {cr.Message.Value}");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error is: {e.Error.Reason}");
                    }
                }
            }, cts.Token);
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
        return Task.CompletedTask;
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}