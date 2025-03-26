namespace McgAssessment.UserService;
/// <summary>
/// Basic User Permissions, uses bitmask for simplification,
/// ensures that permissions that require other permissions
/// include them as well.
/// </summary>
[Flags]
public enum UserPermissions
{
    None = 0,
    ViewPatientGeneral = 1,
    ViewPatientDocuments = 1 << 1 | ViewPatientGeneral, // Must be able to view the patient to view their docs
    CreatePatient = 1 << 2,
    ModifyPatientGeneral = 1 << 3 | ViewPatientGeneral, // Must be able to view the patient to modify it
    UploadPatientDocuments = 1 << 4,
    DeletePatientDocuments = 1 << 5 | ViewPatientDocuments, // Can't delete a document without being able to see them
    DeletePatient = 1 << 6 | ViewPatientGeneral, // Can't delete a patient with being able to view its general info

    AllPermissions = 0b1111111, // Shorthand for every available permission
}