using System;
using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal sealed record class DbUpdateJsonIn
{
    [JsonPropertyName("condition")]
    public string? Condition { get; init; }

    [JsonPropertyName("operations")]
    public FlatArray<DbUpdateJsonOperation> Operations { get; init; }
}