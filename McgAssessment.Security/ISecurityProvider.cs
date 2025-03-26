namespace McgAssessment.Security;

public interface ISecurityProvider
{
    /// <summary>
    /// Encrypts a string
    /// </summary>
    /// <param name="value">The value to be encrypted</param>
    /// <returns>The encrypted string value</returns>
    string EncryptString(string value);
    /// <summary>
    /// Encrypts a binary stream
    /// </summary>
    /// <param name="value">The value to be encrypted</param>
    /// <returns>A stream containing the binary data.</returns>
    Stream EncryptBinary(Stream value);
    
    /// <summary>
    /// Decrypts a string
    /// </summary>
    /// <param name="value">The value to be decrypted</param>
    /// <returns>The unecrypted string value</returns>
    string DecryptString(string value);
    /// <summary>
    /// Decrypts a binary stream
    /// </summary>
    /// <param name="value">The value to be decrypted</param>
    /// <returns>A stream containing the decrypted binary data.</returns>
    Stream DecryptBinary(Stream value);
}