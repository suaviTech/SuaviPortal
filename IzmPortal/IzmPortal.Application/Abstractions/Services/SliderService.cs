using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Domain.Entities;

namespace IzmPortal.Application.Services;

public class SliderService : ISliderService
{
    private readonly ISliderRepository _sliderRepository;
    private readonly IFileStorageService _fileStorageService;

    public SliderService(
        ISliderRepository sliderRepository,
        IFileStorageService fileStorageService)
    {
        _sliderRepository = sliderRepository;
        _fileStorageService = fileStorageService;
    }

    // ✅ SERVICE ARTIK ENTITY DÖNDÜRÜR
    public async Task<Result<List<Slider>>> GetAllAsync(
        CancellationToken ct = default)
    {
        var sliders = await _sliderRepository.GetAllAsync(ct);

        return Result<List<Slider>>.Success(sliders);
    }

    public async Task<Result> CreateAsync(
        string imagePath,
        CancellationToken ct = default)
    {
        var slider = new Slider(imagePath);

        await _sliderRepository.AddAsync(slider, ct);

        return Result.Success("Slider eklendi.");
    }

    public async Task<Result> DeleteAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var slider = await _sliderRepository.GetByIdAsync(id, ct);

        if (slider is null)
            return Result.Failure(
                "Slider bulunamadı.",
                "ERR_NOT_FOUND");

        // 🔥 HARD DELETE (dosya + DB)
        await _fileStorageService.DeleteAsync(
            slider.ImagePath,
            ct);

        await _sliderRepository.DeleteAsync(
            slider,
            ct);

        return Result.Success("Slider silindi.");
    }
}
