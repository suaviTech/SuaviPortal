using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Category;

namespace IzmPortal.Application.Abstractions.Services;

public interface ICategoryService
{
    // PUBLIC
    Task<Result<List<CategoryDto>>> GetPublicAsync(
        CancellationToken ct = default);

    // ADMIN
    Task<Result<List<CategoryDto>>> GetAllAsync(
        CancellationToken ct = default);

    Task<Result<CategoryDto>> GetByIdAsync(
        Guid id,
        CancellationToken ct = default);

    Task<Result> CreateAsync(
        CreateCategoryDto dto,
        CancellationToken ct = default);

    Task<Result> UpdateAsync(
        UpdateCategoryDto dto,
        CancellationToken ct = default);

    Task<Result> ActivateAsync(
        Guid id,
        CancellationToken ct = default);

    Task<Result> DeactivateAsync(
        Guid id,
        CancellationToken ct = default);
}
