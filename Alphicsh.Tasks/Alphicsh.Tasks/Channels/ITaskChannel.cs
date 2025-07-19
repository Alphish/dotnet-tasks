using System;
using System.Threading.Tasks;
using Alphicsh.Tasks.Progress;

namespace Alphicsh.Tasks.Channels;

public interface ITaskChannel<TResult>
{
    void AcceptTask(IManagedAwaitable<TResult> task);

    void Cancel();
    IProgressSubject<TProgress> GetProgressSubjectOf<TProgress>();

    Task<TResult> GetResultAsync();
    TResult? Result { get; }
    event EventHandler<TResult>? TaskCompleted;
}
