using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Slider;

namespace IzmPortal.Application.Abstractions.Services;

public interface ISliderService
{
    Task<Result<List<SliderDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result> CreateAsync(string imagePath, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}
