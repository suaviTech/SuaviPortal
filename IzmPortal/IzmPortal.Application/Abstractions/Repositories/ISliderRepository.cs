using IzmPortal.Domain.Entities;

namespace IzmPortal.Application.Abstractions.Repositories;

public interface ISliderRepository
{
    Task<List<Slider>> GetAllAsync(CancellationToken ct = default);
    Task<Slider?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Slider slider, CancellationToken ct = default);
    Task DeleteAsync(Slider slider, CancellationToken ct = default);
}

