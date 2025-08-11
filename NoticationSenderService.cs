namespace CSharpInterviewPrep;

public class NotificationService
{
    private readonly IEnumerable<INotificationSender> _senders;

    public NotificationService(IEnumerable<INotificationSender> senders)
    {
        _senders = senders;
    }

    public async Task SendAllNotificationsAsync(string userId, string message)
    {
        var tasks = _senders.Select(sender => sender.SendAsync(userId, message));
        await Task.WhenAll(tasks);
        Console.WriteLine("All notifications sent successfully.");
    }
}
