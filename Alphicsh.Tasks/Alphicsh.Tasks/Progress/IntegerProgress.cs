namespace Alphicsh.Tasks.Progress;

public struct IntegerProgress
{
    public int Current { get; }
    public int Target { get; }

    public IntegerProgress(int current, int target)
    {
        Current = current;
        Target = target;
    }
}
