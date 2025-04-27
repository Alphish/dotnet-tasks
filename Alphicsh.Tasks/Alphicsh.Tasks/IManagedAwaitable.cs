namespace Alphicsh.Tasks;

public interface IManagedAwaitable<TResult, TProgress> : IAwaitable<TResult>, IManagedTask<TProgress>
{
}

public interface IManagedAwaitable<TProgress> : IAwaitable, IManagedTask<TProgress>
{
}
