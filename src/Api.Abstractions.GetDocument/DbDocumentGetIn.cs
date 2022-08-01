namespace GGroupp.Infra;

public sealed record DbDocumentGetIn
{
    public DbDocumentGetIn(
        string containerId,
        string documentId, 
        string partitionKey)
    {
        ContainerId = containerId ?? string.Empty;
        DocumentId = documentId ?? string.Empty;
        PartitionKey = partitionKey ?? string.Empty;
    }

    public string ContainerId { get; }

    public string DocumentId { get; }

    public string PartitionKey { get; }
}