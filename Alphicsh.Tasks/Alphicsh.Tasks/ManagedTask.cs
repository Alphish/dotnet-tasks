using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Alphicsh.Tasks.Progress;

namespace Alphicsh.Tasks;

public class ManagedTask<TResult> : IManagedAwaitable<TResult>
{
    protected Task<TResult> InnerTask { get; set; }
    private CancellationTokenSource CancellationSource { get; }
    private ProgressManager ProgressManager { get; }
    public TResult? Result { get; private set; }
    public event EventHandler<TResult>? TaskCompleted;

    // --------
    // Creation
    // --------

    public ManagedTask(Func<CancellationToken, IProgress<object>, Task<TResult>> taskMethod)
    {
        CancellationSource = new CancellationTokenSource();
        ProgressManager = new ProgressManager();

        var taskStub = taskMethod(CancellationSource.Token, ProgressManager);
        InnerTask = taskStub.ContinueWith(ReportCompletion, TaskContinuationOptions.OnlyOnRanToCompletion);
    }

    public TaskAwaiter<TResult> GetAwaiter()
        => InnerTask.GetAwaiter();

    public void Cancel()
    {
        ProgressManager.Cancel();
        CancellationSource.Cancel();
    }

    private TResult ReportCompletion(Task<TResult> task)
    {
        TaskCompleted?.Invoke(this, task.Result);
        return task.Result;
    }

    public IProgressSubject<TProgress> ProgressSubjectOf<TProgress>()
        => new ProgressSubject<TProgress>(ProgressManager);
}

public class ManagedTask : ManagedTask<TaskBlank>, IManagedAwaitable
{
    public ManagedTask(Func<CancellationToken, IProgress<object>, Task> taskMethod)
        : base((cancellationToken, progressReporter) => TaskBlank.FromTask(taskMethod(cancellationToken, progressReporter)))
    {
    }

    public static ManagedTask Create(Func<CancellationToken, IProgress<object>, Task> taskMethod)
        => new ManagedTask(taskMethod);

    public static ManagedTask<TResult> Create<TResult>(Func<CancellationToken, IProgress<object>, Task<TResult>> taskMethod)
        => new ManagedTask<TResult>(taskMethod);

    public static ManagedTask<TResult> FromResult<TResult>(TResult result)
        => new ManagedTask<TResult>((cancellationToken, progress) => Task.FromResult(result));

}
