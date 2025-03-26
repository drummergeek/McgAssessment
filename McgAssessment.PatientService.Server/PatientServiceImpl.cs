using McgAssessment.Data;

namespace McgAssessment.PatientService.Server;

public class PatientServiceImpl : IPatientService
{
    private readonly IPatientDataStore _patientDataStore;
    private readonly IDocumentStore _documentStore;

    public PatientServiceImpl(IPatientDataStore patientDataStore, IDocumentStore documentStore)
    {
        _patientDataStore = patientDataStore;
        _documentStore = documentStore;
    }

    public async Task<PatientServiceResponse> CreatePatientAsync(Patient patient, CancellationToken cancellationToken)
    {
        try
        {
            await _patientDataStore.CreatePatientAsync(patient, cancellationToken);
            return new ();
        }
        catch (Exception e)
        {
            return new (e.Message);
        }
    }

    public async Task<PatientServiceResponse<Patient>> GetPatientByIdAsync(string id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return new (await _patientDataStore.GetPatientByIdAsync(id, cancellationToken));
        }
        catch (Exception e)
        {
            return new (e.Message);
        }
    }

    public async Task<PatientServiceResponse<IEnumerable<Patient>>> GetPatientsByNameAsync(string lastName, string? firstName, bool includePartial = true,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return new (await _patientDataStore.GetPatientsByNameAsync(lastName, firstName, includePartial, cancellationToken));
        }
        catch (Exception e)
        {
            return new (e.Message);
        }
    }

    public async Task<PatientServiceResponse<IEnumerable<Patient>>> GetPatientsByMedicalConditionAsync(string medicalCondition,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return new (await _patientDataStore.GetPatientsByMedicalConditionAsync(medicalCondition, cancellationToken));
        }
        catch (Exception e)
        {
            return new (e.Message);
        }
    }

    public async Task<PatientServiceResponse<IEnumerable<Patient>>> GetPatientsByDocumentTypeAsync(PatientDocumentType documentType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return new (await _patientDataStore.GetPatientsByDocumentTypeAsync(documentType, cancellationToken));
        }
        catch (Exception e)
        {
            return new (e.Message);
        }
    }

    public async Task<PatientServiceResponse> UpdatePatientAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        try
        {
            await _patientDataStore.UpdatePatientAsync(patient, cancellationToken);
            return new();
        }
        catch (Exception e)
        {
            return new (e.Message);
        }
    }

    public async Task<PatientServiceResponse> UpdatePatientMedicalConditionsAsync(string patientId, IEnumerable<string> medicalConditions,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _patientDataStore.UpdatePatientMedicalConditionsAsync(patientId, medicalConditions, cancellationToken);
            return new();
        }
        catch (Exception e)
        {
            return new (e.Message);
        }
    }

    public async Task<PatientServiceResponse> DeletePatientAsync(string patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            var documents = await _patientDataStore.GetPatientDocumentsAsync(patientId, cancellationToken);
            
            foreach (var document in documents)
                await _documentStore.DeleteDocumentAsync(document.Id, cancellationToken);
            
            await _patientDataStore.DeletePatientAsync(patientId, cancellationToken);
            return new();
        }
        catch (Exception e)
        {
            return new (e.Message);
        }
    }

    public async Task<PatientServiceResponse<IEnumerable<PatientDocument>>> GetPatientDocumentsAsync(string patientId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return new(await _patientDataStore.GetPatientDocumentsAsync(patientId, cancellationToken)); 
        }
        catch (Exception e)
        {
            return new (e.Message);
        }
    }

    public async Task<PatientServiceResponse<IEnumerable<PatientDocument>>> GetPatientDocumentsByTypeAsync(string patientId, PatientDocumentType documentType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return new(await _patientDataStore.GetPatientDocumentsByTypeAsync(patientId, documentType, cancellationToken)); 
        }
        catch (Exception e)
        {
            return new (e.Message);
        }
    }

    public async Task<PatientServiceResponse<Stream>> GetPatientDocumentContentsAsync(string patientId, string documentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate the document exists and belongs to the patient
            var document =
                await _patientDataStore.GetPatientDocumentByIdAsync(patientId, documentId, cancellationToken);
            
            return new(await _documentStore.DownloadDocumentAsync(documentId, cancellationToken)); 
        }
        catch (Exception e)
        {
            return new (e.Message);
        }
    }

    public async Task<PatientServiceResponse> AttachPatientDocumentAsync(string patientId, string documentName, PatientDocumentType patientDocumentType,
        Stream documentStream, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate that the patient exists
            var patient = await _patientDataStore.GetPatientByIdAsync(patientId, cancellationToken);
            
            string documentId = await _documentStore.UploadDocumentAsync(documentStream, cancellationToken);
            await _patientDataStore.AttachPatientDocumentAsync(patientId, documentId, documentName, patientDocumentType, cancellationToken);
            return new(); 
        }
        catch (Exception e)
        {
            return new (e.Message);
        }
    }

    public async Task<PatientServiceResponse> DeletePatientDocumentAsync(string patientId, string documentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var document = _patientDataStore.GetPatientDocumentByIdAsync(patientId, documentId, cancellationToken);
            await _documentStore.DeleteDocumentAsync(documentId, cancellationToken);
            await _patientDataStore.DeletePatientDocumentAsync(patientId, documentId, cancellationToken);
            return new(); 
        }
        catch (Exception e)
        {
            return new (e.Message);
        }
    }
}