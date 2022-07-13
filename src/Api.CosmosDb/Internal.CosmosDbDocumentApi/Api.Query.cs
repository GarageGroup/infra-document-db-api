using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class CosmosDbDocumentApi
{
    public ValueTask<Result<DbDocumentSetQueryOut<T>, Failure<DbDocumentSetQueryFailureCode>>> QueryDocumentsAsync<T>(
        DbDocumentSetQueryIn input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<DbDocumentSetQueryOut<T>, Failure<DbDocumentSetQueryFailureCode>>>(cancellationToken);
        }

        return httpMessageHandler.SendAsync<T>(option, input, cancellationToken);
    }
}