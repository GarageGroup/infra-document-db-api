using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DbDocumentSetQueryOut<T>
{
    public DbDocumentSetQueryOut(FlatArray<T> documents, [AllowNull] string continuationToken)
    {
        Documents = documents;
        ContinuationToken = string.IsNullOrEmpty(continuationToken) ? null : continuationToken;
    }

    public DbDocumentSetQueryOut(FlatArray<T> documents)
    {
        Documents = documents;
        ContinuationToken = null;
    }

    public FlatArray<T> Documents { get; }

    public string? ContinuationToken { get; }
}