using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

namespace GGroupp.Infra;

public static class CosmosDbDocumentApiDependency
{
    public static Dependency<IDbDocumentApi> UseCosmosDbDocumentApi(
        this Dependency<HttpMessageHandler, CosmosDbApiOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);

        return dependency.Fold<IDbDocumentApi>(CosmosDbDocumentApi.Create);
    }

    public static Dependency<IDbDocumentApi> UseCosmosDbDocumentApi(
        this Dependency<HttpMessageHandler> dependency, Func<IServiceProvider, CosmosDbApiOption> optionResolver)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        ArgumentNullException.ThrowIfNull(optionResolver);

        return dependency.With(optionResolver).Fold<IDbDocumentApi>(CosmosDbDocumentApi.Create);
    }

    public static Dependency<IDbDocumentApi> UseCosmosDbDocumentApi(
        this Dependency<HttpMessageHandler> dependency, string sectionName = "CosmosDb")
    {
        ArgumentNullException.ThrowIfNull(dependency);

        return dependency.With(ResolveOption).Fold<IDbDocumentApi>(CosmosDbDocumentApi.Create);

        CosmosDbApiOption ResolveOption(IServiceProvider serviceProvider)
            =>
            serviceProvider.GetServiceOrThrow<IConfiguration>().GetSection(sectionName ?? string.Empty).GetCosmosDbApiOption();
    }

    private static CosmosDbApiOption GetCosmosDbApiOption(this IConfigurationSection section)
        =>
        new(
            baseAddress: new(section["BaseAddressUrl"] ?? string.Empty),
            masterKey: section["MasterKey"] ?? string.Empty,
            databaseId: section["DatabaseId"] ?? string.Empty);
}