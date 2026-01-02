using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.ApplicationShortcut;
using IzmPortal.Domain.Entities;



namespace IzmPortal.Application.Abstractions.Services;

public class ApplicationShortcutService : IApplicationShortcutService
{
    private readonly IApplicationShortcutRepository _repository;
   

    public ApplicationShortcutService(
        IApplicationShortcutRepository repository)
    {
        _repository = repository;
   
    }

    // --------------------------------------------------
    // PUBLIC
    // --------------------------------------------------

    public async Task<List<ApplicationShortcutPublicDto>> GetPublicAsync()
    {
        var items = await _repository.GetActiveAsync();

        return items.Select(x => new ApplicationShortcutPublicDto
        {
            Title = x.Title,
            Icon = x.Icon,
            Url = x.Url,
            IsExternal = x.IsExternal
        }).ToList();
    }

    // --------------------------------------------------
    // ADMIN
    // --------------------------------------------------

    public async Task<List<ApplicationShortcutAdminDto>> GetAdminAsync()
    {
        var items = await _repository.GetAllAsync();

        return items.Select(x => new ApplicationShortcutAdminDto
        {
            Id = x.Id,
            Title = x.Title,
            Icon = x.Icon,
            Url = x.Url,
            IsExternal = x.IsExternal,
            IsActive = x.IsActive,
            Order = x.Order
        }).ToList();
    }

    public async Task CreateAsync(CreateUpdateApplicationShortcutDto dto)
    {
        var maxOrder = await _repository.GetMaxOrderAsync();

        var entity = new ApplicationShortcut
        {
            Title = dto.Title,
            Icon = dto.Icon,
            Url = dto.Url,
            IsExternal = dto.IsExternal,
            IsActive = dto.IsActive,
            Order = maxOrder + 1
        };

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, CreateUpdateApplicationShortcutDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("ApplicationShortcut not found.");

        entity.Title = dto.Title;
        entity.Icon = dto.Icon;
        entity.Url = dto.Url;
        entity.IsExternal = dto.IsExternal;
        entity.IsActive = dto.IsActive;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();
    }

    public async Task ActivateAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("ApplicationShortcut not found.");

        entity.IsActive = true;
        await _repository.SaveChangesAsync();
    }

    public async Task DeactivateAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("ApplicationShortcut not found.");

        entity.IsActive = false;
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("ApplicationShortcut not found.");

        _repository.Delete(entity);
        await _repository.SaveChangesAsync();
    }


}
