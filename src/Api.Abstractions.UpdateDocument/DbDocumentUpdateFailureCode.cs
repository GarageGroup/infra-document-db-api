namespace GGroupp.Infra;

public enum DbDocumentUpdateFailureCode
{
    Unknown,

    NotFound,

    InvalidDocumentOperations,

    ExceededOperationsLimit, 
    
    PassedNoOperations
}