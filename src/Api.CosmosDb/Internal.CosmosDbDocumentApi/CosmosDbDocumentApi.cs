using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

internal sealed partial class CosmosDbDocumentApi : IDbDocumentApi
{
    public static CosmosDbDocumentApi Create(HttpMessageHandler httpMessageHandler, CosmosDbApiOption option)
    {
        _ = httpMessageHandler ?? throw new ArgumentNullException(nameof(httpMessageHandler));
        _ = option ?? throw new ArgumentNullException(nameof(option));

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