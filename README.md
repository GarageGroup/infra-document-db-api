# infra-document-db-api

### appsettings.json example
```
{
  "CosmosDb": {
    "BaseAddressUrl": "https://{storage-account}.documents.azure.com/",
    "MasterKey": "",
    "DatabaseId": "{container-id}"
  }
}
```

### Dependency example
```
private static Dependency<IDbDocumentApi> UseCosmosDbApi()
    =>
    PrimaryHandler.UseStandardSocketsHttpHandler()
    .UseLogging("CosmosDbApi")
    .UseCosmosDbDocumentApi();
```