namespace GGroupp.Infra;

public sealed record DbDocumentGetOut<T>
{
    public DbDocumentGetOut(T document)
        =>
        Document = document;

    public T Document { get; }
}