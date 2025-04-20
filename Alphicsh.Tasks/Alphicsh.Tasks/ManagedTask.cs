using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Alphicsh.Tasks;

public abstract class ManagedTaskBase<TTask, TProgress> : IAwaitable
    where TTask : Task
{
    protected TTask InnerTask { get; set; }
    private CancellationTokenSource CancellationSource { get; }

    private IProgress<TProgress> ProgressReporter { get; }
    public TProgress? CurrentProgress { get; private set; }

    public event EventHandler<TProgress>? ProgressChanged;
    protected ManagedTaskBase(Func<CancellationToken, IProgress<TProgress>, TTask> taskMethod)
    {
        CancellationSource = new CancellationTokenSource();
        ProgressReporter = new Progress<TProgress>(UpdateProgress);
        InnerTask = taskMethod(CancellationSource.Token, ProgressReporter);
    }

    public TaskAwaiter GetAwaiter()
        => InnerTask.GetAwaiter();

    public void Cancel()
        => CancellationSource.Cancel();

    protected void UpdateProgress(TProgress progress)
    {
        CurrentProgress = progress;
        ProgressChanged?.Invoke(this, progress);
    }
}

public class ManagedTask<TProgress> : ManagedTaskBase<Task, TProgress>, IVoidAwaitable
{
    public event EventHandler? Completed;

    public ManagedTask(ManagedTaskMethod<TProgress> taskMethod)
        : base((cancellationToken, progressReporter) => taskMethod(cancellationToken, progressReporter))
    {
        InnerTask = InnerTask.ContinueWith(ReportCompletion, TaskContinuationOptions.OnlyOnRanToCompletion);
    }

    protected virtual void ReportCompletion(Task task)
    {
        Completed?.Invoke(this, EventArgs.Empty);
    }
}

public class ManagedTask<TResult, TProgress> : ManagedTaskBase<Task<TResult>, TProgress>, IAwaitable<TResult>
{
    public event EventHandler? Completed;
    public event EventHandler<TResult>? ResultCompleted;

    public ManagedTask(ManagedTaskMethod<TResult, TProgress> taskMethod)
        : base((cancellationSource, progressReporter) => taskMethod(cancellationSource, progressReporter))
    {
        InnerTask = InnerTask.ContinueWith(ReportCompletion, TaskContinuationOptions.OnlyOnRanToCompletion);
    }

    public TResult Result => InnerTask.Result;

    public new TaskAwaiter<TResult> GetAwaiter()
        => InnerTask.GetAwaiter();

    private TResult ReportCompletion(Task<TResult> task)
    {
        Completed?.Invoke(this, EventArgs.Empty);
        ResultCompleted?.Invoke(this, task.Result);
        return task.Result;
    }
}

public static class ManagedTask
{
    public static ManagedTask<TProgress> Create<TProgress>(ManagedTaskMethod<TProgress> taskMethod)
        => new ManagedTask<TProgress>(taskMethod);

    public static ManagedTask<TResult, TProgress> FromResult<TResult, TProgress>(ManagedTaskMethod<TResult, TProgress> taskMethod)
        => new ManagedTask<TResult, TProgress>(taskMethod);
}
