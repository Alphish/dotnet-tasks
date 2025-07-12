using System;

namespace Alphicsh.Tasks.Progress;

public class ProgressManager : IProgressManager
{
    private bool IsCanceled { get; set; } = false;
    public event EventHandler<object>? ProgressChanged;

    public void Cancel()
    {
        IsCanceled = true;
    }

    public void Report(object value)
    {
        if (!IsCanceled)
            ProgressChanged?.Invoke(this, value);
    }
}
