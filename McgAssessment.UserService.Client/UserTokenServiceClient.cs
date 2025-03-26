namespace McgAssessment.UserService.Client;

/// <summary>
/// This is a static in-memory implementation of the user service.
/// In a real environment, this would be a client connecting to an
/// actual user management service. For sake of the assessment, I
/// have hardcoded the valid tokens and their permissions.
/// </summary>
public class UserTokenServiceClient : IUserTokenService
{
    // Some simple shorthands for the available permission sets
    private const UserPermissions CLERK_PERMISSIONS = 
        UserPermissions.ViewPatientGeneral 
            | UserPermissions.ViewPatientDocuments;
    private const UserPermissions ADMIN_PERMISSIONS = 
        UserPermissions.AllPermissions;
   
    // The users for this service are static, only these tokens will return data from the API
    private readonly IReadOnlyDictionary<string, User> _validTokens = 
        new Dictionary<string, User>
        {
            {"clerk-valid", new InternalUserImpl("jdoe01", CLERK_PERMISSIONS)},
            {"admin-valid", new InternalUserImpl("msue", ADMIN_PERMISSIONS)}
        };

    public bool TryValidateUserToken(string token, out User user) 
        => _validTokens.TryGetValue(token, out user);
}