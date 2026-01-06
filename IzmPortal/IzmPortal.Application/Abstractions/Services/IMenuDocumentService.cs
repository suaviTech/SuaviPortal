using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.MenuDocument;

namespace IzmPortal.Application.Abstractions.Services;

public interface IMenuDocumentService
{
    Task<Result<List<MenuDocumentDto>>> GetByMenuAsync(Guid menuId, CancellationToken ct);
    Task<Result> CreateAsync(CreateMenuDocumentDto dto, string filePath, CancellationToken ct);
    Task<Result> DeactivateAsync(Guid id, CancellationToken ct);

}
