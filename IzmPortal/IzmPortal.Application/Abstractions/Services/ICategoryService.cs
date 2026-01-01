using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Category;

namespace IzmPortal.Application.Abstractions.Services;

public interface ICategoryService
{
    Task<Result<List<CategoryDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<CategoryDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result> CreateAsync(CreateCategoryDto dto, CancellationToken ct = default);
    Task<Result> UpdateAsync(UpdateCategoryDto dto, CancellationToken ct = default);
}
