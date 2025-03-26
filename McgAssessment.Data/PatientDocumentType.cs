namespace McgAssessment.Data;

public enum PatientDocumentType
{
    // Ideally this would be a database defined list with IDs that would be returned by a separate service call
    Other,
    ProviderNote,
    MRI,
    CATScan,
}