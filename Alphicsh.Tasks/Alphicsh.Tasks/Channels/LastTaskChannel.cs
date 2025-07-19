using System;
using System.Threading.Tasks;
using Alphicsh.Tasks.Progress;

namespace Alphicsh.Tasks.Channels;

public class LastTaskChannel<TResult> : ITaskChannel<TResult>
{
    private ProgressManager ProgressManager { get; }
    private IManagedAwaitable<TResult> LastTask { get; set; }
    public TResult Result { get; private set; }
    public event EventHandler<TResult>? TaskCompleted;

    public LastTaskChannel(TResult startingValue)
    {
        ProgressManager = new ProgressManager(this);
        LastTask = ManagedTask.FromResult(startingValue);
        Result = startingValue;
    }

    public LastTaskChannel() : this(default!) { }

    public void AcceptTask(IManagedAwaitable<TResult> task)
    {
        LastTask.Cancel();

        LastTask = task;
        LastTask.LinkProgress(ProgressManager);
        LastTask.TaskCompleted += (sender, result) => TaskCompleted?.Invoke(this, result);
    }

    public void Cancel()
    {
        LastTask.Cancel();
    }

    public IProgressSubject<TProgress> GetProgressSubjectOf<TProgress>()
        => new ProgressSubject<TProgress>(ProgressManager);

    public async Task<TResult> GetResultAsync()
    {
        var isResolved = await TryResolveLastTask();
        while (!isResolved)
        {
            isResolved = await TryResolveLastTask();
        }
        return Result;
    }

    private async Task<bool> TryResolveLastTask()
    {
        var nextTask = LastTask;
        await nextTask;

        if (nextTask != LastTask)
            return false;

        if (LastTask.RanToCompletion)
            Result = nextTask.Result!;
        
        return true;
    }
}
