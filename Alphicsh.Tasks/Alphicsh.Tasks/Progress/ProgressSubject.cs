using System;

namespace Alphicsh.Tasks.Progress;

public class ProgressSubject<TProgress> : IProgressSubject<TProgress>
{
    private IProgressManager SourceProgress { get; }
    public event EventHandler<TProgress>? ProgressChanged;

    public ProgressSubject(IProgressManager sourceProgress)
    {
        SourceProgress = sourceProgress;
        SourceProgress.ProgressChanged += (sender, e) =>
        {
            if (e is not TProgress progress)
                return;

            ProgressChanged?.Invoke(sender, progress);
        };
    }
}
