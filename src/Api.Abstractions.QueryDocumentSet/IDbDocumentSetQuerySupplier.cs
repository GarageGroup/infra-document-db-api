using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IDbDocumentSetQuerySupplier
{
    ValueTask<Result<DbDocumentSetQueryOut<T>, Failure<DbDocumentSetQueryFailureCode>>> QueryDocumentsAsync<T>(
        DbDocumentSetQueryIn input, CancellationToken cancellationToken = default);
}