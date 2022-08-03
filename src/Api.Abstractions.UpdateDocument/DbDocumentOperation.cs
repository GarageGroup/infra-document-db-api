namespace GGroupp.Infra;

public sealed record class DbDocumentOperation
{
    public DbDocumentOperation(DbDocumentOperationType operationType, string itemPath, object? value)
    {
        OperationType = operationType;
        ItemPath = itemPath ?? string.Empty;
        Value = value;
    }

    public DbDocumentOperationType OperationType { get; }

    public string ItemPath { get; }

    public object? Value { get; }
}