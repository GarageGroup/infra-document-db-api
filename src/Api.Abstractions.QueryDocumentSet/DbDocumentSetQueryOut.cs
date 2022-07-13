using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DbDocumentSetQueryOut<T>
{
    public DbDocumentSetQueryOut([AllowNull] IReadOnlyCollection<T> documents, [AllowNull] string continuationToken)
    {
        Documents = documents ?? Array.Empty<T>();
        ContinuationToken = string.IsNullOrEmpty(continuationToken) ? null : continuationToken;
    }

    public IReadOnlyCollection<T> Documents { get; }

    public string? ContinuationToken { get; }
}