namespace McgAssessment.UserService;

/// <summary>
/// Base class for a user from a provider such as a doctor's office, limits querying to only the patients with an
/// association to them. 
/// </summary>
public abstract class ProviderUser : User
{
    protected ProviderUser(string userId, string providerId, UserPermissions permissions)
        : base(userId, UserQueryScope.Provider, permissions)
    {
        _providerId = providerId;
    }

    private string _providerId;

    public virtual string ProviderId
    {
        get => _providerId;
        protected set => _providerId = value;
    }
}