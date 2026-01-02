using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Domain.Entities;
using IzmPortal.Domain.Enums;

namespace IzmPortal.Application.Services;

public class SliderService : ISliderService
{
    private readonly ISliderRepository _sliderRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IAuditService _auditService;

    public SliderService(
        ISliderRepository sliderRepository,
        IFileStorageService fileStorageService,
        IAuditService auditService)
    {
        _sliderRepository = sliderRepository;
        _fileStorageService = fileStorageService;
        _auditService = auditService;
    }

    // --------------------
    // GET ALL (PUBLIC)
    // --------------------
    public async Task<Result<List<Slider>>> GetAllAsync(
        CancellationToken ct = default)
    {
        var sliders = await _sliderRepository.GetAllAsync(ct);

        return Result<List<Slider>>.Success(sliders);
    }

    // --------------------
    // CREATE
    // --------------------
    public async Task<Result> CreateAsync(
        string imagePath,
        CancellationToken ct = default)
    {
        var slider = new Slider(imagePath);

        await _sliderRepository.AddAsync(slider, ct);

        await _auditService.LogAsync(
            AuditAction.Create,
            AuditEntity.Slider,
            slider.Id.ToString());

        return Result.Success("Slider eklendi.");
    }

    // --------------------
    // DELETE (HARD)
    // --------------------
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

        await _auditService.LogAsync(
            AuditAction.Delete,
            AuditEntity.Slider,
            id.ToString());

        return Result.Success("Slider silindi.");
    }
}
