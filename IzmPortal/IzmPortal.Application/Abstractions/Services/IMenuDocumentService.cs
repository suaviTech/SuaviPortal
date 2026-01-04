using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.MenuDocument;

namespace IzmPortal.Application.Abstractions.Services;

public interface IMenuDocumentService
{
    Task<Result<List<MenuDocumentDto>>> GetBySubMenuAsync(
        Guid subMenuId,
        CancellationToken ct = default);

    Task<Result> CreateAsync(
        CreateMenuDocumentDto dto,
        string filePath,
        CancellationToken ct = default);

    Task<Result> UpdateAsync(
        UpdateMenuDocumentDto dto,
        CancellationToken ct = default);

    Task<Result> ActivateAsync(
        Guid id,
        CancellationToken ct = default);

    Task<Result> DeactivateAsync(
        Guid id,
        CancellationToken ct = default);
}
