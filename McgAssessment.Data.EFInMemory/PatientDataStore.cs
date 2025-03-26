using McgAssessment.Security;
using Microsoft.EntityFrameworkCore;

namespace McgAssessment.Data.EFInMemory;

public class PatientDataStore : IPatientDataStore
{
    private readonly PatientDbContext _context;
    private readonly ISecurityProvider _securityProvider;
    
    public PatientDataStore(PatientDbContext context, ISecurityProvider securityProvider)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _securityProvider = securityProvider;
    }
    
    private Patient DecryptPatientSummary(PatientEntity patient)
    {
        return new Patient()
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = DateTime.Parse(_securityProvider.DecryptString(patient.DateOfBirthSecure)),
            Gender = _securityProvider.DecryptString(patient.Gender),
        };
    }
    
    private Patient DecryptPatient(PatientEntity patient)
    {
        return new Patient()
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = DateTime.Parse(_securityProvider.DecryptString(patient.DateOfBirthSecure)),
            Gender = _securityProvider.DecryptString(patient.Gender),
            Address = _securityProvider.DecryptString(patient.Address),
            City = _securityProvider.DecryptString(patient.City),
            State = _securityProvider.DecryptString(patient.State),
            Country = _securityProvider.DecryptString(patient.Country),
            Zip = _securityProvider.DecryptString(patient.Zip),
            Phone = _securityProvider.DecryptString(patient.Phone),
            Email = _securityProvider.DecryptString(patient.Email),
            MedicalConditions = patient.MedicalConditions.ToArray()
        };
    }

    private PatientDocument DecryptPatientDocument(PatientDocumentEntity document)
    {
        return new PatientDocument()
        {
            Id = document.Id,
            Title = _securityProvider.DecryptString(document.Title)
        };
    }
    
    private PatientDocumentEntity EncryptPatientDocument(PatientDocument document, int patientId)
    {
        return new PatientDocumentEntity()
        {
            Id = document.Id,
            Title = _securityProvider.EncryptString(document.Title),
            PatientId = patientId,
            DocumentType = document.DocumentType,
        };
    }

    private PatientEntity EncryptPatient(Patient patient, PatientEntity? patientEntity = null)
    {
        ArgumentNullException.ThrowIfNull(patient);
        var result = patientEntity ?? new PatientEntity() { Id = patient.Id };
        result.FirstName = patient.FirstName;
        result.LastName = patient.LastName;
        result.DateOfBirthSecure = _securityProvider.EncryptString(patient.DateOfBirth.ToShortDateString());
        result.Gender = _securityProvider.EncryptString(patient.Gender);
        result.Address = _securityProvider.EncryptString(patient.Address);
        result.City = _securityProvider.EncryptString(patient.City);
        result.State = _securityProvider.EncryptString(patient.State);
        result.Country = _securityProvider.EncryptString(patient.Country);
        result.Zip = _securityProvider.EncryptString(patient.Zip);
        result.Phone = _securityProvider.EncryptString(patient.Phone);
        result.Email = _securityProvider.EncryptString(patient.Email);
        
        return result;
    }

    public async Task CreatePatientAsync(Patient patient, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(patient);
        
        var entry = await _context.Patients.Where(x=>x.Id == patient.Id).SingleOrDefaultAsync(cancellationToken);
        
        if (entry != null)
            throw new KeyNotFoundException("Patient already exists with supplied ID!");

        entry = EncryptPatient(patient);
        
        _context.Patients.Add(entry);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Patient> GetPatientByIdAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var entry = await _context.Patients.Include(p => p.Conditions).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entry == null)
            throw new KeyNotFoundException("Patient record not found for supplied ID!");
        
        return DecryptPatient(entry);
    }
    
    public async Task<IEnumerable<Patient>> GetPatientsByNameAsync(string lastName, string? firstName, bool includePartial = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<PatientEntity> query = !includePartial
            ? _context.Patients.Where(p =>
                (p.LastName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase))
                && (firstName == null || p.FirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase)))
            : _context.Patients.Where(p => 
                (p.LastName.Contains(lastName, StringComparison.InvariantCultureIgnoreCase))
                && (firstName == null || p.FirstName.Contains(firstName, StringComparison.InvariantCultureIgnoreCase)));
        
        return await query.Select(x => DecryptPatientSummary(x)).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Patient>> GetPatientsByMedicalConditionAsync(string medicalCondition,
        CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .Join(
                _context.PatientMedicalConditions.Where(c => c.MedicalCondition.Equals(medicalCondition, StringComparison.InvariantCultureIgnoreCase)), 
                p => p.PatientId, c => c.PatientId, (p, c) => p)
            .Select(x => DecryptPatientSummary(x)).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Patient>> GetPatientsByDocumentTypeAsync(PatientDocumentType documentType,
        CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .Join(
                _context.PatientDocuments.Where(d => d.DocumentType == documentType), 
                p => p.PatientId, d => d.PatientId, (p, d) => p)
            .Select(x => DecryptPatientSummary(x)).Distinct().ToListAsync(cancellationToken);    
    }

    public async Task UpdatePatientAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(patient);
        
        var entry = await _context.Patients.SingleOrDefaultAsync(x => x.Id == patient.Id, cancellationToken);
        if (entry == null)
            throw new KeyNotFoundException("Patient record not found for supplied ID!");
                
        var entity = EncryptPatient(patient, entry);

        _context.Entry(entry).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePatientMedicalConditionsAsync(string patientId, IEnumerable<string> medicalConditions,
        CancellationToken cancellationToken = default)
    {
        var entry = await _context.Patients.Where(x=>x.Id == patientId).Include(x => x.Conditions).SingleOrDefaultAsync(cancellationToken);
        
        if (entry == null)
            throw new KeyNotFoundException("Patient record not found for supplied ID!");

        var newConditions = medicalConditions.ToArray();
        var toRemove = entry.Conditions.Where(x => !newConditions.Contains(x.MedicalCondition));
        var toAdd = newConditions.Where(x => entry.Conditions.All(c => c.MedicalCondition != x))
            .Select(x => new PatientMedicalConditionEntity
            {
                MedicalCondition = x,
                PatientId = entry.PatientId,
            });
        
        _context.PatientMedicalConditions.RemoveRange(toRemove);
        _context.PatientMedicalConditions.AddRange(toAdd);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletePatientAsync(string patientId, CancellationToken cancellationToken = default)
    {
        var entry = await _context.Patients.Where(x=>x.Id == patientId).SingleOrDefaultAsync(cancellationToken);
        
        if (entry == null)
            throw new KeyNotFoundException("Patient record not found for supplied ID!");
        
        _context.Patients.Remove(entry);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<PatientDocument>> GetPatientDocumentsAsync(string patientId,
        CancellationToken cancellationToken = default)
    {
        var entry = await _context.Patients.Where(x=>x.Id == patientId).Include(x => x.Documents).SingleOrDefaultAsync(cancellationToken);
                
        if (entry == null)
            throw new KeyNotFoundException("Patient record not found for supplied ID!");

        return entry.Documents.Select(DecryptPatientDocument);
    }

    public async Task<PatientDocument> GetPatientDocumentByIdAsync(string patientId, string documentId,
        CancellationToken cancellationToken = default)
    {
        var entry = await _context.Patients.Where(x=>x.Id == patientId)
            .Select(x => new
            {
                x.Id, 
                Documents = x.Documents.Where(d => d.Id == documentId).ToArray(),
            }).SingleOrDefaultAsync(cancellationToken); 
                
        if (entry == null)
            throw new KeyNotFoundException("Patient record not found for supplied ID!");
        
        if (entry.Documents.Length == 0)
            throw new KeyNotFoundException("Patient document record not found for supplied ID!");

        return DecryptPatientDocument(entry.Documents.First());
    }

    public async Task<IEnumerable<PatientDocument>> GetPatientDocumentsByTypeAsync(string patientId, PatientDocumentType documentType,
        CancellationToken cancellationToken = default)
    {
        var entry = await _context.Patients.Where(x=>x.Id == patientId)
            .Select(x => new
                {
                    x.Id, 
                    Documents = x.Documents.Where(d => d.DocumentType == documentType).ToArray(),
                }).SingleOrDefaultAsync(cancellationToken);        
        
        if (entry == null)
            throw new KeyNotFoundException("Patient record not found for supplied ID!");

        return entry.Documents.Select(DecryptPatientDocument);
    }

    public async Task AttachPatientDocumentAsync(string patientId, string documentId, string documentName,
        PatientDocumentType patientDocumentType, CancellationToken cancellationToken = default)
    {
        var entry = await _context.Patients.Where(x=>x.Id == patientId).SingleOrDefaultAsync(cancellationToken);
                
        if (entry == null)
            throw new KeyNotFoundException("Patient record not found for supplied ID!");
        
        entry.Documents.Add(EncryptPatientDocument(
            new PatientDocument()
            {
                Id = documentId,
                DocumentType = patientDocumentType, 
                Title = documentName, 
                DocumentDate = DateTime.UtcNow
            }, entry.PatientId));
        
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletePatientDocumentAsync(string patientId, string documentId,
        CancellationToken cancellationToken = default)
    {
        var entry = await _context.Patients.Where(x=>x.Id == patientId)
            .Select(x => new
            {
                x.Id, 
                Documents = x.Documents.Where(d => d.Id == documentId).ToArray(),
            }).SingleOrDefaultAsync(cancellationToken); 
                
        if (entry == null)
            throw new KeyNotFoundException("Patient record not found for supplied ID!");
        
        if (entry.Documents.Length == 0)
            throw new KeyNotFoundException("Patient document record not found for supplied ID!");
        
        _context.PatientDocuments.Remove(entry.Documents.First());
        await _context.SaveChangesAsync(cancellationToken);
    }
}