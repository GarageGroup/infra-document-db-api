namespace GGroupp.Infra;

public sealed record class DbDocumentUpdateOut<T>
{
    public DbDocumentUpdateOut(T document)
        =>
        Document = document;

    public T Document { get; }
}