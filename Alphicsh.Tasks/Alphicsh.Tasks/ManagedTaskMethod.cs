using System;
using System.Threading;
using System.Threading.Tasks;

namespace Alphicsh.Tasks;

public delegate Task ManagedTaskMethod<TProgress>(
    CancellationToken cancellationToken, IProgress<TProgress> progressReporter
    );

public delegate Task<TResult> ManagedTaskMethod<TResult, TProgress>(
    CancellationToken cancellationToken, IProgress<TProgress> progressReporter
    );
