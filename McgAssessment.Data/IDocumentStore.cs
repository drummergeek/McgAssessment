namespace McgAssessment.Data;

public interface IDocumentStore
{
    Task<Stream> DownloadDocumentAsync(
        string documentId,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Stores the supplied document stream and returns the document ID.
    /// </summary>
    /// <param name="documentStream"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> UploadDocumentAsync(
        Stream documentStream, 
        CancellationToken cancellationToken = default);
    
    Task DeleteDocumentAsync(
        string documentId, 
        CancellationToken cancellationToken = default);
}