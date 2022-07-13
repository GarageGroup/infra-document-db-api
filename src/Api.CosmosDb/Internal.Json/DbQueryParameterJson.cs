using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal sealed record class DbQueryParameterJson
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("value")]
    public string? Value { get; init; }
}