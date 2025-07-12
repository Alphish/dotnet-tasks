namespace Alphicsh.Tasks;

public interface IManagedAwaitable<TResult> : IAwaitable<TResult>, IManagedTask
{
}

public interface IManagedAwaitable : IAwaitable, IManagedTask
{
}
