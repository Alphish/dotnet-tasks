using System;
using System.Runtime.CompilerServices;

namespace Alphicsh.Tasks;

public interface IAwaitable<TResult>
{
    TaskAwaiter<TResult> GetAwaiter();
    TResult? Result { get; }
    event EventHandler<TResult>? Completed;
}

public interface IAwaitable : IAwaitable<TaskBlank>
{
}
