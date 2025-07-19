using System;

namespace Alphicsh.Tasks.Progress;

public interface IProgressManager : IProgress<object>
{
    event EventHandler<object>? ProgressChanged;
    void Cancel();
    void Link(IProgress<object> manager);
}
