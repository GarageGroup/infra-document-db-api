using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

internal static partial class HttpExtensions
{
    internal static async ValueTask<Result<Unit, Failure<DbDocumentDeleteFailureCode>>> SendAsync(
        this HttpMessageHandler handler, CosmosDbApiOption option, DbDocumentDeleteIn input, CancellationToken cancellationToken)
    {
        var resourceId = $"dbs/{Encode(option.DatabaseId)}/colls/{Encode(input.ContainerId)}/{ItemResourceType}/{input.DocumentId}";
        using var hashAlgorithm = CreateHashAlgorithm(option.MasterKey);

        using var httpClient = InnerCreateHttpClient();
        using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, resourceId);

        var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        if (httpResponse.IsSuccessStatusCode)
        {
            return default(Unit);
        }

        var body = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return CreateHttpFailure(httpResponse.StatusCode, body).MapFailureCode(MapStatusCode);

        HttpClient InnerCreateHttpClient()
            =>
            CreateHttpClient(handler, option.BaseAddress)
            .AddCosmosDbCommonHeaders(hashAlgorithm, HttpMethod.Delete.Method, resourceId, ItemResourceType)
            .AddPartitionKeyHeader(input.PartitionKey);

        static DbDocumentDeleteFailureCode MapStatusCode(HttpStatusCode statusCode)
            =>
            statusCode switch
            {
                HttpStatusCode.NotFound => DbDocumentDeleteFailureCode.NotFound,
                _ => default
            };
    }
}