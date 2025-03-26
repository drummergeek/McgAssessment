namespace McgAssessment.Data;

public class Patient
{
    public virtual string Id { get; set; } 
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual DateTime DateOfBirth { get; set; }
    public virtual string Email { get; set; }
    public virtual string Phone { get; set; }
    public virtual string Address { get; set; }
    public virtual string City { get; set; }
    public virtual string State { get; set; }
    public virtual string Zip { get; set; }
    public virtual string Country { get; set; }
    public virtual string Gender { get; set; }
    public virtual IEnumerable<string> MedicalConditions { get; set; }
}