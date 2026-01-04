using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Slider;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Infrastructure.Services;

public class SliderService : ISliderService
{
    private readonly PortalDbContext _db;

    public SliderService(PortalDbContext db)
    {
        _db = db;
    }

    public async Task<Result<List<SliderDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await _db.Sliders
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new SliderDto
            {
                Id = x.Id,
                ImagePath = x.ImagePath,
                Title = x.Title,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(ct);

        return Result<List<SliderDto>>.Success(items);
    }

    public async Task<Result> CreateAsync(
        CreateSliderDto dto,
        string imagePath,
        CancellationToken ct = default)
    {
        var slider = new Domain.Entities.Slider(imagePath, dto.Title);

        _db.Sliders.Add(slider);
        await _db.SaveChangesAsync(ct);

        return Result.Success("Slider eklendi.");
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var slider = await _db.Sliders.FindAsync(new object[] { id }, ct);
        if (slider == null)
            return Result.Failure("Slider bulunamadı.");

        _db.Sliders.Remove(slider);
        await _db.SaveChangesAsync(ct);

        return Result.Success("Slider silindi.");
    }
}
