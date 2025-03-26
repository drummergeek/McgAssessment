using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace McgAssessment.Data.EFInMemory;

[Index(nameof(DocumentId), IsUnique = true)]
[Index(nameof(DocumentType), nameof(PatientId))]
public class PatientDocumentEntity : PatientDocument
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DocumentId { get; set; }

    [Required]
    public int PatientId { get; set; }
    public PatientEntity Patient { get; set; }
    
    [Column("DocumentUid")]
    public override string Id { get; set; }
}