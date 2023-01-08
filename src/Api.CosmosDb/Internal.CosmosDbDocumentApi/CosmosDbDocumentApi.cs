using System;
using System.Net.Http;

namespace GGroupp.Infra;

internal sealed partial class CosmosDbDocumentApi : IDbDocumentApi
{
    public static CosmosDbDocumentApi Create(HttpMessageHandler httpMessageHandler, CosmosDbApiOption option)
    {
        ArgumentNullException.ThrowIfNull(httpMessageHandler);
        ArgumentNullException.ThrowIfNull(option);

        return new(httpMessageHandler, option);
    }

    private readonly HttpMessageHandler httpMessageHandler;

    private readonly CosmosDbApiOption option;

    private CosmosDbDocumentApi(HttpMessageHandler httpMessageHandler, CosmosDbApiOption option)
    {
        this.httpMessageHandler = httpMessageHandler;
        this.option = option;
    }
}