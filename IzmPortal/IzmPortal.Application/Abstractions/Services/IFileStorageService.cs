namespace IzmPortal.Application.Abstractions.Services;

public interface IFileStorageService
{
    Task<string> SaveAsync(
        Stream fileStream,
        string fileName,
        string folder,
        CancellationToken ct = default);

    Task DeleteAsync(
        string relativePath,
        CancellationToken ct = default);
}

