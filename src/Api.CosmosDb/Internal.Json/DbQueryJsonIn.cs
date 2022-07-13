using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal sealed record class DbQueryJsonIn
{
    [JsonPropertyName("query")]
    public string? Query { get; init; }

    [JsonPropertyName("parameters")]
    public DbQueryParameterJson[]? Parameters { get; init; }
}