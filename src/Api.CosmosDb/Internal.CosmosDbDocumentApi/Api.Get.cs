using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class CosmosDbDocumentApi
{
    public ValueTask<Result<DbDocumentGetOut<T>, Failure<DbDocumentGetFailureCode>>> GetDocumentAsync<T>(
        DbDocumentGetIn input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<DbDocumentGetOut<T>, Failure<DbDocumentGetFailureCode>>>(cancellationToken);
        }

        return httpMessageHandler.SendAsync<T>(option, input, cancellationToken);
    }
}