namespace CSharpInterviewPrep;

public class WhatsAppSender : INotificationSender
{
    public async Task SendAsync(string userId, string message)
    {
        // Simulate sending a WhatsApp message
        await Task.Delay(1000); // Simulate network delay
        Console.WriteLine($"WhatsApp message sent to {userId}: {message}");
    }
}
