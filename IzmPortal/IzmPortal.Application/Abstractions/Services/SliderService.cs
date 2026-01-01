using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Slider;
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

    public async Task<Result<List<SliderDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var sliders = await _sliderRepository.GetAllAsync(ct);

        var list = sliders.Select(x => new SliderDto
        {
            Id = x.Id,
            ImageUrl = x.ImagePath
        }).ToList();

        return Result<List<SliderDto>>.Success(list);
    }

    public async Task<Result> CreateAsync(string imagePath, CancellationToken ct = default)
    {
        var slider = new Slider(imagePath);
        await _sliderRepository.AddAsync(slider, ct);
        return Result.Success("Slider eklendi.");
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var slider = await _sliderRepository.GetByIdAsync(id, ct);
        if (slider is null)
            return Result.Failure("Slider bulunamadı.");

        // 🔥 HARD DELETE (dosya + DB)
        await _fileStorageService.DeleteAsync(slider.ImagePath, ct);
        await _sliderRepository.DeleteAsync(slider, ct);

        return Result.Success("Slider silindi.");
    }
}
