using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IDbDocumentGetSupplier
{
    ValueTask<Result<DbDocumentGetOut<T>, Failure<DbDocumentGetFailureCode>>> GetDocumentAsync<T>(
        DbDocumentGetIn input, CancellationToken cancellationToken = default);
}