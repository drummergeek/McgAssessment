namespace McgAssessment.Data;

public interface IPatientDataStore
{
    Task CreatePatientAsync(
        Patient patient, 
        CancellationToken cancellationToken = default);
    
    Task<Patient> GetPatientByIdAsync(
        string id, 
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Patient>> GetPatientsByNameAsync(
        string lastName,
        string? firstName,
        bool includePartial = true,
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Patient>> GetPatientsByMedicalConditionAsync(
        string medicalCondition,
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Patient>> GetPatientsByDocumentTypeAsync(
        PatientDocumentType documentType,
        CancellationToken cancellationToken = default);
    
    Task UpdatePatientAsync(
        Patient patient,
        CancellationToken cancellationToken = default);

    Task UpdatePatientMedicalConditionsAsync(
        string patientId,
        IEnumerable<string> medicalConditions,
        CancellationToken cancellationToken = default);
    
    Task DeletePatientAsync(
        string patientId, 
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<PatientDocument>> GetPatientDocumentsAsync(
        string patientId,
        CancellationToken cancellationToken = default);
    
    Task<PatientDocument> GetPatientDocumentByIdAsync(
        string patientId,
        string documentId,
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<PatientDocument>> GetPatientDocumentsByTypeAsync(
        string patientId,
        PatientDocumentType documentType,
        CancellationToken cancellationToken = default);
    
    Task AttachPatientDocumentAsync(
        string patientId, 
        string documentId,
        string documentName, 
        PatientDocumentType patientDocumentType,
        CancellationToken cancellationToken = default);
    
    Task DeletePatientDocumentAsync(
        string patientId,
        string documentId,
        CancellationToken cancellationToken = default);
}