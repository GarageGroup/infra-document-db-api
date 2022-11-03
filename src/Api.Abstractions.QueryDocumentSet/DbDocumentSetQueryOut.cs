using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DbDocumentSetQueryOut<T>
{
    public DbDocumentSetQueryOut([AllowNull] FlatArray<T> documents, [AllowNull] string continuationToken)
    {
        Documents = documents ?? FlatArray.Empty<T>();
        ContinuationToken = string.IsNullOrEmpty(continuationToken) ? null : continuationToken;
    }

    public FlatArray<T> Documents { get; }

    public string? ContinuationToken { get; }
}