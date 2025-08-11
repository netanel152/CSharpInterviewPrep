namespace CSharpInterviewPrep;

public class SmsSender : INotificationSender
{
    public async Task SendAsync(string userId, string message)
    {
        // Simulate sending an SMS
        await Task.Delay(1000); // Simulate network delay
        Console.WriteLine($"SMS sent to {userId}: {message}");
    }
}
