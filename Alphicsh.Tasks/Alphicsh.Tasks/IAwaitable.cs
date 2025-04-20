using System;
using System.Runtime.CompilerServices;

namespace Alphicsh.Tasks;

public interface IAwaitable
{
    TaskAwaiter GetAwaiter();
}

public interface IAwaitable<TResult> : IAwaitable
{
    new TaskAwaiter<TResult> GetAwaiter();
    TResult Result { get; }
    event EventHandler<TResult>? ResultCompleted;
}
