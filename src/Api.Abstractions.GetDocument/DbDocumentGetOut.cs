namespace GGroupp.Infra;

public sealed record DbDocumentGetOut<T>
{
    public DbDocumentGetOut(T document)
        =>
        Document = document;

    T Document { get; }
}