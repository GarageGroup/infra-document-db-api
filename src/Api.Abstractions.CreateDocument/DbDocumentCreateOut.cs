namespace GGroupp.Infra;

public sealed record class DbDocumentCreateOut<T>
{
    public DbDocumentCreateOut(T document)
        =>
        Document = document;

    public T Document { get; }
}