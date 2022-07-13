namespace GGroupp.Infra;

public sealed record class DbDocumentCreateIn<T>
{
    public DbDocumentCreateIn(string containerId, string partitionKey, T document, bool isUpsert = false)
    {
        ContainerId = containerId ?? string.Empty;
        PartitionKey = partitionKey ?? string.Empty;
        Document = document;
        IsUpsert = isUpsert;
    }

    public string ContainerId { get; }

    public string PartitionKey { get; }

    public T Document { get; }

    public bool IsUpsert { get; }
}