using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DbDocumentSetQueryIn
{
    public DbDocumentSetQueryIn(
        string containerId,
        string query,
        [AllowNull] IReadOnlyCollection<KeyValuePair<string, string>> queryParameters,
        int? maxItemCount = null,
        [AllowNull] string continuationToken = null)
    {
        ContainerId = containerId ?? string.Empty;
        Query = query ?? string.Empty;
        QueryParameters = queryParameters?.Count is not > 0 ? null : queryParameters;
        MaxItemCount = maxItemCount;
        ContinuationToken = string.IsNullOrEmpty(continuationToken) ? null : continuationToken;
    }

    public string ContainerId { get; }

    public string Query { get; }

    public IReadOnlyCollection<KeyValuePair<string, string>>? QueryParameters { get; }

    public int? MaxItemCount { get; }

    public string? ContinuationToken { get; }
}