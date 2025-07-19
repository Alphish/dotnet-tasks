using System;
using System.Runtime.CompilerServices;

namespace Alphicsh.Tasks;

public interface IAwaitable<TResult>
{
    TaskAwaiter<TResult> GetAwaiter();
    TResult? Result { get; }
    bool RanToCompletion { get; }
    event EventHandler<TResult>? TaskCompleted;
}

public interface IAwaitable : IAwaitable<TaskBlank>
{
}
