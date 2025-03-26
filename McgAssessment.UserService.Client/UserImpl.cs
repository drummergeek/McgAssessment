namespace McgAssessment.UserService.Client;

// Below are the implementations of the base User classes for this client.
// In an actual implementation of a service, the Server would have its
// own implementation with related logic.
// Sealing the class signals that nothing else should be modifying
// the User class from this point forward.

public sealed class InternalUserImpl : InternalUser
{
    internal InternalUserImpl(string userId, UserPermissions permissions)
        : base(userId, permissions) {}
}

public sealed class ProviderUserImpl : ProviderUser
{
    internal ProviderUserImpl(string userId, string providerId, UserPermissions permissions)
        : base(userId, providerId, permissions) {}
}

public sealed class PatientUserImpl : PatientUser
{
    internal PatientUserImpl(string userId, string patientId, UserPermissions permissions)
        : base(userId, patientId, permissions) {}
}