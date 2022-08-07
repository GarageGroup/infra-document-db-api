namespace GGroupp.Infra;

public interface IDbDocumentApi
    : IDbDocumentCreateSupplier, IDbDocumentSetQuerySupplier, IDbDocumentGetSupplier, IDbDocumentUpdateSupplier, IDbDocumentDeleteSupplier
{
}