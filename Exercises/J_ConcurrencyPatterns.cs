using System.Threading.Channels;

namespace CSharpInterviewPrep.Exercises;

public static class J_ConcurrencyPatterns
{
    // Scenario: High-volume data ingestion (e.g., IoT signals, Clickstream data)
    // Requirement: We need to ingest data fast (Producer) and process it in the background (Consumer)
    // without blocking the main thread or losing data.
    // Solution: System.Threading.Channels (The "Go-style" channels for .NET)

    public record DataMessage(int Id, string Payload, DateTime Timestamp);

    public static async Task Run()
    {
        Console.WriteLine("--- Section 10: Concurrency Patterns (Producer-Consumer) ---");

        // 1. Create a Bounded Channel
        // "Bounded" provides backpressure. If the consumer is too slow, the producer will wait 
        // instead of crashing memory with infinite queued items.
        var channelOptions = new BoundedChannelOptions(capacity: 100)
        {
            FullMode = BoundedChannelFullMode.Wait, // Wait for space if full
            SingleReader = false, // Multiple consumers allowed
            SingleWriter = true   // Single producer
        };

        var channel = Channel.CreateBounded<DataMessage>(channelOptions);

        Console.WriteLine("[System] Starting Pipeline...");

        // 2. Start Consumers (Workers)
        // We'll simulate 2 consumers processing data in parallel
        var consumerTask1 = ProcessMessagesAsync(channel.Reader, 1);
        var consumerTask2 = ProcessMessagesAsync(channel.Reader, 2);

        // 3. Start Producer
        var producerTask = ProduceMessagesAsync(channel.Writer);

        // Wait for producer to finish writing
        await producerTask;

        // Wait for consumers to finish processing (they stop when channel completes)
        await Task.WhenAll(consumerTask1, consumerTask2);

        Console.WriteLine("[System] Pipeline Finished.");
        Console.WriteLine("--------------------------------------\n");
    }

    private static async Task ProduceMessagesAsync(ChannelWriter<DataMessage> writer)
    {
        for (int i = 1; i <= 20; i++)
        {
            var msg = new DataMessage(i, "Log_Entry_" + i, DateTime.UtcNow);
            
            // WriteAsync will wait if the channel is full (Backpressure handling)
            await writer.WriteAsync(msg);
            
            Console.WriteLine($" -> [Producer] Pushed message {i}");
            
            // Simulate variable ingestion speed
            await Task.Delay(50); 
        }

        // Signal that no more items will be written
        writer.Complete();
        Console.WriteLine(" -> [Producer] Complete. Channel closed.");
    }

    private static async Task ProcessMessagesAsync(ChannelReader<DataMessage> reader, int workerId)
    {
        // ReadAllAsync is an IAsyncEnumerable - efficient iteration
        await foreach (var msg in reader.ReadAllAsync())
        {
            // Simulate processing work (e.g., writing to DB, calling external API)
            await Task.Delay(150); 
            
            Console.WriteLine($"    <- [Worker {workerId}] Processed {msg.Id} ({msg.Payload})");
        }
        Console.WriteLine($"    <- [Worker {workerId}] Done.");
    }
}
