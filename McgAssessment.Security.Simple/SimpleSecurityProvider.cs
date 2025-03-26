using System.Security.Permissions;

namespace McgAssessment.Security.Simple;

/// <summary>
/// Stand in for actual encryption layer. Provides the layer for implementation later
/// </summary>
public class SimpleSecurityProvider : ISecurityProvider
{
    public string EncryptString(string value)
    {
        return value;
    }

    public Stream EncryptBinary(Stream value)
    {
        return value;
    }

    public string DecryptString(string value)
    {
        return value;
    }

    public Stream DecryptBinary(Stream value)
    {
        return value;
    }
}

