namespace CSharpInterviewPrep
{
    public interface INotificationSender
    {
        Task SendAsync(string uderId, string message);
    }
}
