namespace CSharpInterviewPrep;

public class EmailSender : INotificationSender
{
    public async Task SendAsync(string userId, string message)
    {
        // Simulate sending an email
        await Task.Delay(1000); // Simulate network delay
        Console.WriteLine($"Email sent to {userId}: {message}");
    }
}
