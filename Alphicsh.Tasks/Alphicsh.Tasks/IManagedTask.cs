using System;

namespace Alphicsh.Tasks;

public interface IManagedTask<TProgress>
{
    void Cancel();
    TProgress? CurrentProgress { get; }
    event EventHandler<TProgress>? ProgressChanged;
}
