using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

internal static partial class HttpExtensions
{
    internal static async ValueTask<Result<DbDocumentSetQueryOut<T>, Failure<DbDocumentSetQueryFailureCode>>> SendAsync<T>(
        this HttpMessageHandler handler, CosmosDbApiOption option, DbDocumentSetQueryIn input, CancellationToken cancellationToken)
    {
        var resourceId = $"dbs/{Encode(option.DatabaseId)}/colls/{Encode(input.ContainerId)}";
        using var hashAlgorithm = CreateHashAlgorithm(option.MasterKey);

        using var httpClient = InnerCreateHttpClient();

        var query = new DbQueryJsonIn(input.Query, input.QueryParameters.Map(MapQueryParameter));

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, resourceId + "/" + ItemResourceType)
        {
            Content = CreateQueryContent(query)
        };

        var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        var body = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (httpResponse.IsSuccessStatusCode is false)
        {
            return CreateHttpFailure(httpResponse.StatusCode, body).MapFailureCode(MapStatusCode);
        }

        return new DbDocumentSetQueryOut<T>(
            documents: Deserialize<DbQueryJsonOut<T>>(body).Documents,
            continuationToken: httpResponse.GetHeaderValue(ContinuationTokenHeaderName));

        HttpClient InnerCreateHttpClient()
            =>
            CreateHttpClient(handler, option.BaseAddress)
            .AddCosmosDbCommonHeaders(hashAlgorithm, HttpMethod.Post.Method, resourceId, ItemResourceType)
            .AddHeader("x-ms-documentdb-isquery", bool.TrueString)
            .AddHeader("x-ms-max-item-count", input.MaxItemCount?.ToString())
            .AddHeader("x-ms-continuation", input.ContinuationToken)
            .AddPartitionKeyHeader(input.PartitionKey)
            .AddHeader("x-ms-documentdb-query-enablecrosspartition", bool.TrueString);

        static DbDocumentSetQueryFailureCode MapStatusCode(HttpStatusCode statusCode)
            =>
            statusCode switch
            {
                HttpStatusCode.BadRequest => DbDocumentSetQueryFailureCode.InvalidQuery,
                _ => default
            };
    }

    private static DbQueryParameterJson MapQueryParameter(KeyValuePair<string, string> pair)
        =>
        new()
        {
            Name = pair.Key,
            Value = pair.Value
        };
}