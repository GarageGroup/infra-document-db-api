using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal readonly record struct DbQueryJsonOut<T>
{
    [JsonPropertyName("Documents")]
    public T[]? Documents { get; init; }
}