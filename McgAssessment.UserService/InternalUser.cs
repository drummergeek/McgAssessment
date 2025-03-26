namespace McgAssessment.UserService;

/// <summary>
/// Base class for an internal user who can query the entire patient database
/// </summary>
public abstract class InternalUser : User
{
    protected InternalUser(string userId, UserPermissions permissions)
        : base(userId, UserQueryScope.All, permissions)
    {
    }
}