using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Slider;

public interface ISliderService
{
    Task<Result<List<SliderDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result> CreateAsync(CreateSliderDto dto, string imagePath, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}
