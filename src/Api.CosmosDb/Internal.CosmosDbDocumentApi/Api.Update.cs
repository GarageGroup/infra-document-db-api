using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class CosmosDbDocumentApi
{

    public ValueTask<Result<DbDocumentUpdateOut<T>, Failure<DbDocumentUpdateFailureCode>>> UpdateDocumentAsync<T>(
        DbDocumentUpdateIn input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<DbDocumentUpdateOut<T>, Failure<DbDocumentUpdateFailureCode>>>(cancellationToken);
        }

        return httpMessageHandler.SendAsync<T>(option, input, cancellationToken);
    }
}