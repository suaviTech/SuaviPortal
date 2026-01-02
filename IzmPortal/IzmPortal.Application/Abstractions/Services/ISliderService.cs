using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Slider;
using IzmPortal.Domain.Entities;

namespace IzmPortal.Application.Abstractions.Services;

public interface ISliderService
{
    Task<Result<List<Slider>>> GetAllAsync(CancellationToken ct = default);
    Task<Result> CreateAsync(string imagePath, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}
