namespace McgAssessment.UserService;

/// <summary>
/// Provides the query scope for the User, ensuring they can only see patient records that they are allowed to view.
/// </summary>
public enum UserQueryScope
{
    None = 0,
    Self = 1, // For patients, they can only access their own info
    Provider = 2, // For providers, they can only access patients associated to them
    All = 3, // Access to all patients
}