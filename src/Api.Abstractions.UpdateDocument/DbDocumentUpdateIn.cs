using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DbDocumentUpdateIn
{
    public DbDocumentUpdateIn(
        string containerId,
        string documentId, 
        string partitionKey,
        IReadOnlyCollection<DbDocumentOperation> documentOperations,
        [AllowNull] string condition = null)
    {
        ContainerId = containerId ?? string.Empty;
        DocumentId = documentId ?? string.Empty;
        PartitionKey = partitionKey ?? string.Empty;
        Condition = string.IsNullOrEmpty(condition) ? null : condition;
        DocumentOperations = documentOperations ?? Array.Empty<DbDocumentOperation>();
    }

    public string ContainerId { get; }

    public string DocumentId { get; }

    public string PartitionKey { get; }

    public string? Condition { get; }

    public IReadOnlyCollection<DbDocumentOperation> DocumentOperations { get; }
}