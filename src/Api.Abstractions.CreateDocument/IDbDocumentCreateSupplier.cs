using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IDbDocumentCreateSupplier
{
    ValueTask<Result<DbDocumentCreateOut<T>, Failure<DbDocumentCreateFailureCode>>> CreateDocumentAsync<T>(
        DbDocumentCreateIn<T> input, CancellationToken cancellationToken = default);
}