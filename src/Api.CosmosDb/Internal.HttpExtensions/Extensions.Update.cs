using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class HttpExtensions
{
    private const int updateOperationsLimit = 10;

    internal static async ValueTask<Result<DbDocumentUpdateOut<T>, Failure<DbDocumentUpdateFailureCode>>> SendAsync<T>(
        this HttpMessageHandler handler, CosmosDbApiOption option, DbDocumentUpdateIn input, CancellationToken cancellationToken)
    {
        if(input.DocumentOperations.Count > updateOperationsLimit)
        {
            return Failure.Create(DbDocumentUpdateFailureCode.OperationsLimitExceeded, "The number of operations must be less than 10");
        }

        var resourceId = $"dbs/{Encode(option.DatabaseId)}/colls/{Encode(input.ContainerId)}/docs/{input.DocumentId}";
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

        return DeserializeOrFailure(body);

        HttpClient InnerCreateHttpClient()
            =>
            CreateHttpClient(handler, option.BaseAddress)
            .AddCosmosDbCommonHeaders(hashAlgorithm, HttpMethod.Patch.Method, resourceId, "docs")
            .AddPartitionKeyHeader(input.PartitionKey);

        static DbDocumentUpdateFailureCode MapStatusCode(HttpStatusCode statusCode)
            =>
            statusCode switch
            {
                HttpStatusCode.NotFound => DbDocumentUpdateFailureCode.NotFound,
                HttpStatusCode.BadRequest => DbDocumentUpdateFailureCode.InvalidDocumentOperations,
                _ => default
            };
        
        static Result<DbDocumentUpdateOut<T>, Failure<DbDocumentUpdateFailureCode>> DeserializeOrFailure(string body)
        {
            try
            {
                var document = Deserialize<T>(body);
                if(document is null)
                {
                    return Failure.Create(DbDocumentUpdateFailureCode.Unknown, $"Cannot deserialize response body: {body}");
                }

                return new DbDocumentUpdateOut<T>(document);
            }
            catch (JsonException ex)
            {
                return Failure.Create(DbDocumentUpdateFailureCode.Unknown, $"An error occurred during deserialization response body: {body}, error: {ex.Message}");
            }
        }
    }
}