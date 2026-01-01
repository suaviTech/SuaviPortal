using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Menu;

namespace IzmPortal.Application.Abstractions.Services;

public interface IMenuDocumentService
{
    Task<Result<List<MenuDocumentDto>>> GetBySubMenuIdAsync(
        Guid subMenuId,
        CancellationToken ct = default);

    Task<Result> UploadAsync(
        Guid subMenuId,
        string title,
        Stream fileStream,
        string fileName,
        CancellationToken ct = default);

    Task<Result> DeleteAsync(
        Guid id,
        CancellationToken ct = default);
}
