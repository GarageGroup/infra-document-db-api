using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

internal static partial class HttpExtensions
{
    internal static async ValueTask<Result<DbDocumentCreateOut<T>, Failure<DbDocumentCreateFailureCode>>> SendAsync<T>(
        this HttpMessageHandler handler, CosmosDbApiOption option, DbDocumentCreateIn<T> input, CancellationToken cancellationToken)
    {
        var resourceId = $"dbs/{Encode(option.DatabaseId)}/colls/{Encode(input.ContainerId)}";
        using var hashAlgorithm = CreateHashAlgorithm(option.MasterKey);

        using var httpClient = InnerCreateHttpClient();
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, resourceId + "/docs")
        {
            Content = CreateDocumentContent(input.Document)
        };

        var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        if (httpResponse.IsSuccessStatusCode)
        {
            return new DbDocumentCreateOut<T>(input.Document);
        }

        var body = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return CreateHttpFailure(httpResponse.StatusCode, body).MapFailureCode(MapStatusCode);

        HttpClient InnerCreateHttpClient()
            =>
            CreateHttpClient(handler, option.BaseAddress)
            .AddCosmosDbCommonHeaders(hashAlgorithm, HttpMethod.Post.Method, resourceId, "docs")
            .AddPartitionKeyHeader(input.PartitionKey)
            .AddHeader("x-ms-documentdb-is-upsert", input.IsUpsert ? bool.TrueString : null);

        static DbDocumentCreateFailureCode MapStatusCode(HttpStatusCode statusCode)
            =>
            statusCode switch
            {
                HttpStatusCode.BadRequest => DbDocumentCreateFailureCode.InvalidRequest,
                HttpStatusCode.Conflict => DbDocumentCreateFailureCode.Conflict,
                _ => default
            };
    }
}