namespace CSharpInterviewPrep.Exercises;

public static class K_Resilience
{
    // Scenario: Communicating with an unstable Microservice or 3rd Party API.
    // Requirement: Transient failures (network blips, timeouts) shouldn't crash our app.
    // Solution: Retry Pattern with Exponential Backoff (Clean implementation without external libs like Polly for demo)

    public static async Task Run()
    {
        Console.WriteLine("--- Section 11: Microservices Resilience (Retry Pattern) ---");

        var unstableService = new ExternalService();

        Console.WriteLine("Attempting to fetch data from unstable service...");

        try
        {
            // Execute the operation with retry logic
            string result = await ExecuteWithRetryAsync(
                action: () => unstableService.FetchDataAsync(),
                maxRetries: 3,
                delayMilliseconds: 500
            );

            Console.WriteLine($"✅ Success! Result: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Operation failed after retries. Final Error: {ex.Message}");
        }

        Console.WriteLine("--------------------------------------\n");
    }

    // Generic Retry Logic
    private static async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action, int maxRetries, int delayMilliseconds)
    {
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   [Warning] Attempt {attempt} failed: {ex.Message}");

                if (attempt == maxRetries)
                {
                    // If this was the last attempt, rethrow the exception
                    throw; 
                }

                // Exponential Backoff: Wait longer between each retry (500ms -> 1000ms -> 2000ms...)
                // Jitter (randomness) is often added here in production to prevent "Thundering Herd" problem.
                int waitTime = delayMilliseconds * (int)Math.Pow(2, attempt - 1);
                
                Console.WriteLine($"   -> Waiting {waitTime}ms before retry...");
                await Task.Delay(waitTime);
            }
        }
        throw new InvalidOperationException("Should not be reached");
    }

    // Simulation of a flaky service
    public class ExternalService
    {
        private int _callCount = 0;

        public async Task<string> FetchDataAsync()
        {
            _callCount++;
            await Task.Delay(200); // Simulate network latency

            // Simulate failure for the first 2 calls, success on the 3rd
            if (_callCount < 3)
            {
                throw new HttpRequestException("503 Service Unavailable");
            }

            return "Payload_From_Microservice";
        }
    }
}
