using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Domain.Entities;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Infrastructure.Repositories;

public class SliderRepository : ISliderRepository
{
    private readonly PortalDbContext _context;

    public SliderRepository(PortalDbContext context)
    {
        _context = context;
    }

    public async Task<List<Slider>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Sliders
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<Slider?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Sliders
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task AddAsync(Slider slider, CancellationToken ct = default)
    {
        await _context.Sliders.AddAsync(slider, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Slider slider, CancellationToken ct = default)
    {
        _context.Sliders.Remove(slider);
        await _context.SaveChangesAsync(ct);
    }
}
