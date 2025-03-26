using McgAssessment.Data;

namespace McgAssessment.PatientService;

public interface IPatientService
{
    Task<PatientServiceResponse> CreatePatientAsync(
        Patient patient, 
        CancellationToken cancellationToken);
    
    Task<PatientServiceResponse<Patient>> GetPatientByIdAsync(
        string id,
        CancellationToken cancellationToken = default);

    Task<PatientServiceResponse<IEnumerable<Patient>>> GetPatientsByNameAsync(
        string lastName,
        string? firstName,
        bool includePartial = true,
        CancellationToken cancellationToken = default);
    
    Task<PatientServiceResponse<IEnumerable<Patient>>> GetPatientsByMedicalConditionAsync(
        string medicalCondition,
        CancellationToken cancellationToken = default);
    
    Task<PatientServiceResponse<IEnumerable<Patient>>> GetPatientsByDocumentTypeAsync(
        PatientDocumentType documentType,
        CancellationToken cancellationToken = default);
    
    Task<PatientServiceResponse> UpdatePatientAsync(
        Patient patient,
        CancellationToken cancellationToken = default);

    Task<PatientServiceResponse> UpdatePatientMedicalConditionsAsync(
        string patientId,
        IEnumerable<string> medicalConditions,
        CancellationToken cancellationToken = default);
    
    Task<PatientServiceResponse> DeletePatientAsync(
        string patientId, 
        CancellationToken cancellationToken = default);
    
    Task<PatientServiceResponse<IEnumerable<PatientDocument>>> GetPatientDocumentsAsync(
        string patientId,
        CancellationToken cancellationToken = default);
    
    Task<PatientServiceResponse<IEnumerable<PatientDocument>>> GetPatientDocumentsByTypeAsync(
        string patientId,
        PatientDocumentType documentType,
        CancellationToken cancellationToken = default);
    
    Task<PatientServiceResponse<Stream>> GetPatientDocumentContentsAsync(
        string patientId,
        string documentId,
        CancellationToken cancellationToken = default);
    
    Task<PatientServiceResponse> AttachPatientDocumentAsync(
        string patientId,
        string documentName, 
        PatientDocumentType patientDocumentType, 
        Stream documentStream,
        CancellationToken cancellationToken = default);
    
    Task<PatientServiceResponse> DeletePatientDocumentAsync(
        string patientId,
        string documentId,
        CancellationToken cancellationToken = default);
}
