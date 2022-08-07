using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IDbDocumentDeleteSupplier
{
    ValueTask<Result<Unit, Failure<DbDocumentDeleteFailureCode>>> DeleteDocumentAsync(
        DbDocumentDeleteIn input, CancellationToken cancellationToken = default);
}