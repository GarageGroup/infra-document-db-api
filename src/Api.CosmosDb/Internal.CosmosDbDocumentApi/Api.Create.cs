using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class CosmosDbDocumentApi
{
    public ValueTask<Result<DbDocumentCreateOut<T>, Failure<DbDocumentCreateFailureCode>>> CreateDocumentAsync<T>(
        DbDocumentCreateIn<T> input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<DbDocumentCreateOut<T>, Failure<DbDocumentCreateFailureCode>>>(cancellationToken);
        }

        return httpMessageHandler.SendAsync(option, input, cancellationToken);
    }
}