using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal sealed record class DbQueryJsonIn
{
    public DbQueryJsonIn([AllowNull] string query, [AllowNull] DbQueryParameterJson[] parameters)
    {
        Query = string.IsNullOrEmpty(query) ? null : query;
        Parameters = parameters?.Length is not > 0 ? null : parameters;
    }

    [JsonPropertyName("query")]
    public string? Query { get; }

    [JsonPropertyName("parameters")]
    public DbQueryParameterJson[]? Parameters { get; }
}