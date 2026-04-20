using AppCore.Dto;
using AppCore.Interfaces;
using AppCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfGenericRepository<T>(DbSet<T> set) : IGenericRepositoryAsync<T>
    where T : EntityBase
{
    public virtual async Task<T?> FindByIdAsync(Guid id)
    {
        return await set.FindAsync(id);
    }

    public async Task<IEnumerable<T>> FindAllAsync()
    {
        return await set.ToListAsync();
    }

    public async Task<PagedResult<T>> FindPagedAsync(int page, int pageSize)
    {
        if (page <= 0)
            throw new ArgumentOutOfRangeException(nameof(page), "Page must be >= 1.");
        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "PageSize must be >= 1.");

        var totalCount = await set.CountAsync();
        var items = await set
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>(items, totalCount, page, pageSize);
    }

    public async Task<T> AddAsync(T entity)
    {
        var entry = await set.AddAsync(entity);
        return entry.Entity;
    }

    public Task<T> UpdateAsync(T entity)
    {
        var entityEntry = set.Update(entity);
        return Task.FromResult(entityEntry.Entity);
    }

    public async Task RemoveByIdAsync(Guid id)
    {
        var entity = await set.FindAsync(id);
        if (entity is null)
            return;

        set.Remove(entity);
    }
}
