using System;

namespace GGroupp.Infra;

public sealed record class CosmosDbApiOption
{
    public CosmosDbApiOption(Uri baseAddress, string masterKey, string databaseId)
    {
      BaseAddress = baseAddress;
      MasterKey = masterKey ?? string.Empty;
      DatabaseId = databaseId ?? string.Empty;
    }

    public Uri BaseAddress { get; }

    public string MasterKey { get; }

    public string DatabaseId { get; }
}