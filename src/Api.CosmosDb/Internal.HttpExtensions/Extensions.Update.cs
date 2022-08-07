using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class HttpExtensions
{
    private const int updateOperationsLimit = 10;

    internal static async ValueTask<Result<DbDocumentUpdateOut<T>, Failure<DbDocumentUpdateFailureCode>>> SendAsync<T>(
        this HttpMessageHandler handler, CosmosDbApiOption option, DbDocumentUpdateIn input, CancellationToken cancellationToken)
    {
        if (input.DocumentOperations.Count > updateOperationsLimit)
        {
            return Failure.Create(
                DbDocumentUpdateFailureCode.ExceededOperationsLimit, $"The number of operations must be less than {updateOperationsLimit}");
        }

        if (input.DocumentOperations.Count is 0)
        {
            return Failure.Create(
                DbDocumentUpdateFailureCode.PassedNoOperations, "The number of operations must be greater than 0");
        }

        var resourceId = $"dbs/{Encode(option.DatabaseId)}/colls/{Encode(input.ContainerId)}/{ItemResourceType}/{input.DocumentId}";
        using var hashAlgorithm = CreateHashAlgorithm(option.MasterKey);

        using var httpClient = InnerCreateHttpClient();
        using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, resourceId)
        {
            Content = CreateUpdateContent(input)
        };

        var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        var body = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (httpResponse.IsSuccessStatusCode is false)
        {
            return CreateHttpFailure(httpResponse.StatusCode, body).MapFailureCode(MapStatusCode);
        }

        return DeserializeOrFailure<T, DbDocumentUpdateFailureCode>(body).MapSuccess(static doc => new DbDocumentUpdateOut<T>(doc));

        HttpClient InnerCreateHttpClient()
            =>
            CreateHttpClient(handler, option.BaseAddress)
            .AddCosmosDbCommonHeaders(hashAlgorithm, HttpMethod.Patch.Method, resourceId, ItemResourceType)
            .AddPartitionKeyHeader(input.PartitionKey);

        static DbDocumentUpdateFailureCode MapStatusCode(HttpStatusCode statusCode)
            =>
            statusCode switch
            {
                HttpStatusCode.NotFound => DbDocumentUpdateFailureCode.NotFound,
                HttpStatusCode.BadRequest => DbDocumentUpdateFailureCode.InvalidDocumentOperations,
                _ => default
            };
    }
}