using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

namespace GGroupp.Infra;

public static class CosmosDbDocumentApiDependency
{
    public static Dependency<IDbDocumentApi> UseCosmosDbDocumentApi(this Dependency<HttpMessageHandler, CosmosDbApiOption> dependency)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Fold<IDbDocumentApi>(CosmosDbDocumentApi.Create);
    }

    public static Dependency<IDbDocumentApi> UseCosmosDbDocumentApi(
        this Dependency<HttpMessageHandler> dependency, Func<IServiceProvider, CosmosDbApiOption> optionResolver)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = optionResolver ?? throw new ArgumentNullException(nameof(optionResolver));

        return dependency.With(optionResolver).Fold<IDbDocumentApi>(CosmosDbDocumentApi.Create);
    }

    public static Dependency<IDbDocumentApi> UseCosmosDbDocumentApi(this Dependency<HttpMessageHandler> dependency, string sectionName = "CosmosDb")
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.With(ResolveOption).Fold<IDbDocumentApi>(CosmosDbDocumentApi.Create);

        CosmosDbApiOption ResolveOption(IServiceProvider serviceProvider)
            =>
            serviceProvider.GetServiceOrThrow<IConfiguration>().GetSection(sectionName ?? string.Empty).GetCosmosDbApiOption();
    }

    private static CosmosDbApiOption GetCosmosDbApiOption(this IConfigurationSection section)
        =>
        new(
            baseAddress: new(section["BaseAddressUrl"]),
            masterKey: section["MasterKey"],
            databaseId: section["DatabaseId"]);
}