using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Alphicsh.Tasks;

public class ManagedTask<TResult, TProgress> : IManagedAwaitable<TResult, TProgress>
{
    protected Task<TResult> InnerTask { get; set; }
    private CancellationTokenSource CancellationSource { get; }
    public TResult? Result { get; private set; }
    public event EventHandler<TResult>? Completed;

    private IProgress<TProgress> ProgressReporter { get; }
    public TProgress? CurrentProgress { get; private set; }
    public event EventHandler<TProgress>? ProgressChanged;

    public ManagedTask(ManagedTaskMethod<TResult, TProgress> taskMethod)
    {
        CancellationSource = new CancellationTokenSource();
        ProgressReporter = new Progress<TProgress>(UpdateProgress);

        var taskStub = taskMethod(CancellationSource.Token, ProgressReporter);
        InnerTask = taskStub.ContinueWith(ReportCompletion, TaskContinuationOptions.OnlyOnRanToCompletion);
    }

    public TaskAwaiter<TResult> GetAwaiter()
        => InnerTask.GetAwaiter();

    public void Cancel()
        => CancellationSource.Cancel();

    protected void UpdateProgress(TProgress progress)
    {
        CurrentProgress = progress;
        ProgressChanged?.Invoke(this, progress);
    }

    private TResult ReportCompletion(Task<TResult> task)
    {
        Completed?.Invoke(this, task.Result);
        return task.Result;
    }
}

public class ManagedTask<TProgress> : ManagedTask<TaskBlank, TProgress>, IManagedAwaitable<TProgress>
{
    public ManagedTask(ManagedTaskMethod<TProgress> taskMethod)
        : base((cancellationToken, progressReporter) => TaskBlank.FromTask(taskMethod(cancellationToken, progressReporter)))
    {
    }
}

public static class ManagedTask
{
    public static ManagedTask<TProgress> Create<TProgress>(ManagedTaskMethod<TProgress> taskMethod)
        => new ManagedTask<TProgress>(taskMethod);

    public static ManagedTask<TResult, TProgress> FromResult<TResult, TProgress>(ManagedTaskMethod<TResult, TProgress> taskMethod)
        => new ManagedTask<TResult, TProgress>(taskMethod);
}
