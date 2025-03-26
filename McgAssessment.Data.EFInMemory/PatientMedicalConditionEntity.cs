using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace McgAssessment.Data.EFInMemory;

[Index(nameof(PatientId), nameof(MedicalCondition), IsUnique = true)]
[Index(nameof(MedicalCondition), nameof(PatientId))] // Reverse index based on condition
public class PatientMedicalConditionEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]    
    public int Id { get; set; }
    
    [Required]
    public int PatientId { get; set; }
    public PatientEntity Patient { get; set; }
    
    public string MedicalCondition { get; set; }
}