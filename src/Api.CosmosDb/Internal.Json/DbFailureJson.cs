using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal readonly record struct DbFailureJson
{
    [JsonPropertyName("message")]
    public string? Message { get; init; }
}