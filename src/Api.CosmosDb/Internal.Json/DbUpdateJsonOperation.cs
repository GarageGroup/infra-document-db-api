using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal sealed record class DbUpdateJsonOperation
{
    public DbUpdateJsonOperation(string operationType, string itemPath, object? value)
    {
        OperationType = operationType;
        ItemPath = itemPath ?? string.Empty;
        Value = value;
    }

    [JsonPropertyName("op")]
    public string OperationType { get; }

    [JsonPropertyName("path")]
    public string ItemPath { get; }

    [JsonPropertyName("value")]
    public object? Value { get; }
}