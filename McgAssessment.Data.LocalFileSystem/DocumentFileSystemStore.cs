using McgAssessment.Security;

namespace McgAssessment.Data.LocalFileSystem;

public class DocumentFileSystemStore : IDocumentStore
{
    private readonly ISecurityProvider _securityProvider;
    private readonly string _fileRoot;

    public DocumentFileSystemStore(ISecurityProvider securityProvider, string fileRoot)
    {
        _securityProvider = securityProvider;
        _fileRoot = fileRoot;
        if (!Directory.Exists(_fileRoot))
            Directory.CreateDirectory(_fileRoot);
    }

    public async Task<Stream> DownloadDocumentAsync(string documentId, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(Path.Join(_fileRoot, documentId)))
            throw new FileNotFoundException("Document not found with supplied ID!");
        
        return _securityProvider.DecryptBinary(File.OpenRead(Path.Join(_fileRoot, documentId)));
    }

    public async Task<string> UploadDocumentAsync(Stream documentStream, CancellationToken cancellationToken = default)
    {
        string documentId = Guid.NewGuid().ToString();
        documentStream.Seek(0, SeekOrigin.Begin);
        var encryptedDocumentStream = _securityProvider.EncryptBinary(documentStream);
        
        Memory<char> buffer = new char[8192]; // 8K buffer for file saving
        using (var fileStream = File.OpenWrite(Path.Join(_fileRoot, documentId)))
        using (var streamWriter = new StreamWriter(fileStream))
        using (var streamReader = new StreamReader(encryptedDocumentStream))
        {
            while (!streamReader.EndOfStream)
            {
                await streamReader.ReadAsync(buffer, cancellationToken);
                await streamWriter.WriteAsync(buffer, cancellationToken);
            }
            streamReader.Close();
            await streamWriter.FlushAsync(cancellationToken);
            streamWriter.Close();
        }

        return documentId;
    }

    public async Task DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(Path.Join(_fileRoot, documentId)))
            throw new FileNotFoundException("Document not found with supplied ID!");
        
        File.Delete(Path.Join(_fileRoot, documentId));
    }
}