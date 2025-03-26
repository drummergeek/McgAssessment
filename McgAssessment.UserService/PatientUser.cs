namespace McgAssessment.UserService;

/// <summary>
/// Base class for a patient user allowing them to view only their own data.
/// </summary>
public abstract class PatientUser : User
{
    protected PatientUser(string userId, string patientId, UserPermissions permissions)
        : base(userId, UserQueryScope.Self, permissions)
    {
        _patientId = patientId;
    }

    private string _patientId;

    public virtual string PatientId
    {
        get => _patientId;
        protected set => _patientId = value;
    }
}