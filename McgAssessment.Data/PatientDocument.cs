namespace McgAssessment.Data;

public class PatientDocument
{
    public virtual string Id { get; set; }
    public virtual string Title { get; set; }
    public virtual PatientDocumentType DocumentType { get; set; }
    public virtual DateTime DocumentDate { get; set; }
}