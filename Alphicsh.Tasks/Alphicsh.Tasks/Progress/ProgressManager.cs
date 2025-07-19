using System;
using System.Collections.Generic;

namespace Alphicsh.Tasks.Progress;

public class ProgressManager : IProgressManager
{
    private object Sender { get; }
    private bool IsCanceled { get; set; } = false;
    private List<IProgress<object>> LinkedProgresses { get; } = new List<IProgress<object>>();

    public event EventHandler<object>? ProgressChanged;

    public ProgressManager(object sender)
    {
        Sender = sender;
    }

    public void Cancel()
    {
        IsCanceled = true;
    }

    public void Link(IProgress<object> manager)
    {
        LinkedProgresses.Add(manager);
    }

    public void Report(object value)
    {
        if (IsCanceled)
            return;

        ProgressChanged?.Invoke(Sender, value);
        foreach (var manager in LinkedProgresses)
        {
            manager.Report(value);
        }
    }
}
