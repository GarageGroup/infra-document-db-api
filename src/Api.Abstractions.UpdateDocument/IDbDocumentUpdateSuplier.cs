using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IDbDocumentUpdateSupplier
{
    ValueTask<Result<DbDocumentUpdateOut<T>, Failure<DbDocumentUpdateFailureCode>>> UpdateDocumentAsync<T>(
        DbDocumentUpdateIn input, CancellationToken cancellationToken = default);
}