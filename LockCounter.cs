namespace CSharpInterviewPrep;

public class LockCounter
{
    private readonly Lock _lock = new();
    private int _value = 0;

    public void Increment()
    {
        lock (_lock)
        {
            _value++;
        }
    }

    public int GetValue()
    {
        lock (_lock)
        {
            return _value;
        }
    }
}
