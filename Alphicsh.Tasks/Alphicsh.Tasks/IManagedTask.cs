using Alphicsh.Tasks.Progress;

namespace Alphicsh.Tasks;

public interface IManagedTask
{
    void Cancel();
    IProgressSubject<TProgress> ProgressSubjectOf<TProgress>();
}
