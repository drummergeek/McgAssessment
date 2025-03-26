using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace McgAssessment.Data.EFInMemory;

[Index(nameof(Id), IsUnique = true)]
public class PatientEntity : Patient
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PatientId { get; set; }
    
    [Column("PatientUid")]
    public override string Id { get; set; }
    
    public string DateOfBirthSecure { get; set; }

    [NotMapped]
    public override DateTime DateOfBirth { get; set; }

    public ICollection<PatientMedicalConditionEntity> Conditions { get; set; } = new List<PatientMedicalConditionEntity>();

    public ICollection<PatientDocumentEntity> Documents { get; set; } = new List<PatientDocumentEntity>();
    
    [NotMapped]
    public override IEnumerable<string> MedicalConditions
    {
        get => Conditions?.Select(x => x.MedicalCondition) ?? [];
        set => throw new InvalidOperationException();
    }
}