using System;

namespace Alphicsh.Tasks.Progress;

public interface IProgressSubject<TProgress>
{
    event EventHandler<TProgress>? ProgressChanged;
}
