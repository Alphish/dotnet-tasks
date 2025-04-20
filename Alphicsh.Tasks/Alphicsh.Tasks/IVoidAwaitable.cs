using System;

namespace Alphicsh.Tasks;

public interface IVoidAwaitable : IAwaitable
{
    event EventHandler? Completed;
}
