namespace CSharpInterviewPrep;

public class InterLockCounter
{
    private int _value = 0;
    public void Increment()
    {
        Interlocked.Increment(ref _value);
    }
    public int GetValue()
    {
        return Interlocked.CompareExchange(ref _value, 0, 0);
    }
}
