using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal sealed record class DbUpdateJsonIn
{
    public DbUpdateJsonIn(string? condition, IReadOnlyCollection<DbUpdateJsonOperation> operations)
    {
        Condition = condition;
        Operations = operations ?? Array.Empty<DbUpdateJsonOperation>();
    }

    [JsonPropertyName("condition")]
    public string? Condition { get; }

    [JsonPropertyName("operations")]
    public IReadOnlyCollection<DbUpdateJsonOperation> Operations { get; }
}