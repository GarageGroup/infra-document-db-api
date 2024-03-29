using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace GGroupp.Infra;

internal static partial class HttpExtensions
{
    private const string ItemResourceType = "docs";

    private const string ContinuationTokenHeaderName = "x-ms-continuation";
    
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private static HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler, Uri baseAddress)
        =>
        new(httpMessageHandler, false)
        {
            BaseAddress = baseAddress
        };

    private static HttpClient AddCosmosDbCommonHeaders(
        this HttpClient client, HashAlgorithm algorithm, string verb, string resourceId, string resourceType)
    {
        var utcDate = DateTime.UtcNow.ToString("r");
        var authHeaderValue = algorithm.GenerateAuthorizationHeaderValue(verb, resourceId, resourceType, utcDate);

        return client.AddHeader("x-ms-date", utcDate).AddHeader("x-ms-version", "2018-12-31").AddHeader("authorization", authHeaderValue);
    }

    private static HttpClient AddPartitionKeyHeader(this HttpClient httpClient, string? partitionKey)
    {
        if (string.IsNullOrEmpty(partitionKey))
        {
            return httpClient;
        }

        return httpClient.AddHeader("x-ms-documentdb-partitionkey", "[\"" + partitionKey + "\"]");
    }

    private static HttpClient AddHeader(this HttpClient httpClient, string name, string? value)
    {
        if (string.IsNullOrEmpty(value) is false)
        {
            httpClient.DefaultRequestHeaders.Add(name, value);
        }

        return httpClient;
    }

    private static string GenerateAuthorizationHeaderValue(
        this HashAlgorithm hashAlgorithm, string verb, string resourceId, string resourceType, string utcDate)
    {
        var payLoad = $"{verb.ToLowerInvariant()}\n{resourceType}\n{resourceId}\n{utcDate.ToLowerInvariant()}\n\n";

        var hashPayLoad = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(payLoad));
        var signature = Convert.ToBase64String(hashPayLoad);

        var masterKeyAuthorizationSignature = $"type=master&ver=1.0&sig={signature}";
        return HttpUtility.UrlEncode(masterKeyAuthorizationSignature);
    }

    private static HashAlgorithm CreateHashAlgorithm(string masterKey)
        =>
        new HMACSHA256()
        {
            Key = Convert.FromBase64String(masterKey)
        };

    private static StringContent? CreateDocumentContent<T>(T? document)
        =>
        document is null ? null : new(JsonSerializer.Serialize(document, jsonSerializerOptions), Encoding.UTF8, "application/json");

    private static StringContent CreateQueryContent(DbQueryJsonIn query)
    {
        var queryContent = new StringContent(JsonSerializer.Serialize(query, jsonSerializerOptions));
        queryContent.Headers.ContentType = new MediaTypeHeaderValue("application/query+json");

        return queryContent;
    }

    private static T? Deserialize<T>(string body)
        =>
        JsonSerializer.Deserialize<T>(body, jsonSerializerOptions);

    private static Failure<HttpStatusCode> CreateHttpFailure(HttpStatusCode statusCode, string? failureBody)
    {
        if (string.IsNullOrEmpty(failureBody))
        {
            return new(statusCode, "Response code is unexpected");
        }

        var dbFailureJson = Deserialize<DbFailureJson>(failureBody);
        if (string.IsNullOrEmpty(dbFailureJson.Message))
        {
            return new(statusCode, failureBody);
        }

        return new(statusCode, dbFailureJson.Message);
    }

    private static string? GetHeaderValue(this HttpResponseMessage response, string headerName)
    {
        if (response.Headers.TryGetValues(headerName, out var values) is false || values is null)
        {
            return null;
        }

        return string.Join(';', values);
    }

    private static string Encode(string source)
        =>
        HttpUtility.UrlEncode(source.ToLowerInvariant());

    private static StringContent CreateUpdateContent(DbDocumentUpdateIn input)
    {
        var updateJson = new DbUpdateJsonIn
        {
            Condition = input.Condition,
            Operations = input.DocumentOperations.Map(MapOperation)
        };
        
        var content = new StringContent(JsonSerializer.Serialize(updateJson, jsonSerializerOptions));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json_patch+json");

        return content;

        static DbUpdateJsonOperation MapOperation(DbDocumentOperation operation)
            =>
            new(
                operationType: MapOperationType(operation.OperationType),
                itemPath: operation.ItemPath,
                value: operation.Value );

        static string MapOperationType(DbDocumentOperationType operationType)
            =>
            operationType switch 
            {
                DbDocumentOperationType.Add       => "add",
                DbDocumentOperationType.Set       => "set",
                DbDocumentOperationType.Remove    => "remove",
                DbDocumentOperationType.Replace   => "replace",
                _ => throw new InvalidOperationException("Unknown operation type")
            };
    }

    private static Result<T, Failure<TFailureCode>> DeserializeOrFailure<T, TFailureCode>(string body)
        where TFailureCode : struct
    {
        try
        {
            var document = Deserialize<T>(body);
            if(document is null)
            {
                return Failure.Create<TFailureCode>(default, $"Cannot deserialize response body: {body}");
            }

            return document;
        }
        catch (JsonException ex)
        {
            return InnerCreateFailure(ex);
        }
        catch (NotSupportedException ex)
        {
            return InnerCreateFailure(ex);
        }

        Failure<TFailureCode> InnerCreateFailure(Exception ex)
            =>
            new(default, $"An error occurred during deserialization response body: {body}, error: {ex.Message}");
    }

    private static FlatArray<TResult> Map<TSource, TResult>(this FlatArray<TSource> source, Func<TSource, TResult> map)
    {
        if (source.IsEmpty)
        {
            return default;
        }

        var array = new TResult[source.Length];

        for (var i = 0; i < array.Length; i++)
        {
            array[i] = map.Invoke(source[i]);
        }

        return array;
    }
}