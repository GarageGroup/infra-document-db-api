using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class CosmosDbDocumentApi
{
    public ValueTask<Result<Unit, Failure<DbDocumentDeleteFailureCode>>> DeleteDocumentAsync(
        DbDocumentDeleteIn input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<Unit, Failure<DbDocumentDeleteFailureCode>>>(cancellationToken);
        }

        return httpMessageHandler.SendAsync(option, input, cancellationToken);
    }
}