using IzmPortal.Application.Abstractions.Services;
using Microsoft.AspNetCore.Hosting;

namespace IzmPortal.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _env;

    public LocalFileStorageService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveAsync(
        Stream fileStream,
        string fileName,
        string folder,
        CancellationToken ct = default)
    {
        var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", folder);
        Directory.CreateDirectory(uploadsRoot);

        var uniqueFileName =
            $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";

        var fullPath = Path.Combine(uploadsRoot, uniqueFileName);

        using var stream = new FileStream(
            fullPath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None);

        await fileStream.CopyToAsync(stream, ct);

        // DB’de saklanacak relative path
        return Path.Combine("uploads", folder, uniqueFileName)
            .Replace("\\", "/");
    }

    public Task DeleteAsync(
        string relativePath,
        CancellationToken ct = default)
    {
        var fullPath = Path.Combine(
            _env.WebRootPath,
            relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }
}
