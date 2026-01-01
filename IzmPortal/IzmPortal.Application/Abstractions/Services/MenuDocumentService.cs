using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Menu;
using IzmPortal.Domain.Entities;

namespace IzmPortal.Application.Services;

public class MenuDocumentService : IMenuDocumentService
{
    private readonly IMenuDocumentRepository _documentRepository;
    private readonly ISubMenuRepository _subMenuRepository;
    private readonly IFileStorageService _fileStorageService;

    public MenuDocumentService(
        IMenuDocumentRepository documentRepository,
        ISubMenuRepository subMenuRepository,
        IFileStorageService fileStorageService)
    {
        _documentRepository = documentRepository;
        _subMenuRepository = subMenuRepository;
        _fileStorageService = fileStorageService;
    }

    public async Task<Result<List<MenuDocumentDto>>> GetBySubMenuIdAsync(
        Guid subMenuId,
        CancellationToken ct = default)
    {
        var documents =
            await _documentRepository.GetBySubMenuIdAsync(subMenuId, ct);

        var list = documents
            .Select(x => new MenuDocumentDto
            {
                Id = x.Id,
                Title = x.Title,
                FilePath = x.FilePath,
                CreatedAt = x.CreatedAt
            })
            .ToList();

        return Result<List<MenuDocumentDto>>.Success(list);
    }

    public async Task<Result> UploadAsync(
     Guid subMenuId,
     string title,
     Stream fileStream,
     string fileName,
     CancellationToken ct = default)
    {
        var subMenu = await _subMenuRepository.GetByIdAsync(subMenuId, ct);
        if (subMenu is null)
            return Result.Failure("Alt menü bulunamadı.");

        var filePath = await _fileStorageService.SaveAsync(
            fileStream,
            fileName,
            "docs/menu",
            ct);

        var document = new MenuDocument(
            title,
            filePath,
            subMenuId);

        await _documentRepository.AddAsync(document, ct);

        return Result.Success("Doküman yüklendi.");
    }


    public async Task<Result> DeleteAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var document = await _documentRepository.GetByIdAsync(id, ct);
        if (document is null)
            return Result.Failure("Doküman bulunamadı.");

        // 🔥 HARD DELETE (dosya + DB)
        await _fileStorageService.DeleteAsync(document.FilePath, ct);
        await _documentRepository.DeleteAsync(document, ct);

        return Result.Success("Doküman silindi.");
    }
}

