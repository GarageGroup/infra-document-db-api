namespace GGroupp.Infra;

public sealed record DbDocumentDeleteIn
{
    public DbDocumentDeleteIn(string containerId, string documentId, string partitionKey)
    {
        ContainerId = containerId ?? string.Empty;
        DocumentId = documentId ?? string.Empty;
        PartitionKey = partitionKey ?? string.Empty;
    }

    public string ContainerId { get; }

    public string DocumentId { get; }

    public string PartitionKey { get; }
}