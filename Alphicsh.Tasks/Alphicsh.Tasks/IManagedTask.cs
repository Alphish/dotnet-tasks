using System;
using Alphicsh.Tasks.Progress;

namespace Alphicsh.Tasks;

public interface IManagedTask
{
    void Cancel();
    IProgressSubject<TProgress> GetProgressSubjectOf<TProgress>();
    void LinkProgress(IProgress<object> progress);
}
