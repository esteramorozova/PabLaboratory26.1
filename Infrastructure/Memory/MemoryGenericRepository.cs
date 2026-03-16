using System.Reflection;
using AppCore.Dto;
using AppCore.Interfaces;

namespace Infrastructure.Memory;

public class MemoryGenericRepository<T> : IGenericRepositoryAsync<T> where T : class
{
    protected readonly Dictionary<Guid, T> _data = new();
    private readonly PropertyInfo _idProperty;

    public MemoryGenericRepository()
    {
        _idProperty = typeof(T).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)
                      ?? throw new InvalidOperationException(
                          $"Type {typeof(T).FullName} must have a public instance property named 'Id'.");

        if (_idProperty.PropertyType != typeof(Guid) || !_idProperty.CanRead || !_idProperty.CanWrite)
        {
            throw new InvalidOperationException(
                $"Type {typeof(T).FullName} must have a readable/writable Guid 'Id' property.");
        }
    }

    public Task<T?> FindByIdAsync(Guid id)
    {
        var result = _data.TryGetValue(id, out var value) ? value : null;
        return Task.FromResult(result);
    }

    public Task<IEnumerable<T>> FindAllAsync()
    {
        IEnumerable<T> result = _data.Values.ToList();
        return Task.FromResult(result);
    }

    public Task<PagedResult<T>> FindPagedAsync(int page, int pageSize)
    {
        if (page <= 0) throw new ArgumentOutOfRangeException(nameof(page), "Page must be >= 1.");
        if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize), "PageSize must be >= 1.");

        var all = _data.Values.ToList();
        var total = all.Count;
        var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return Task.FromResult(new PagedResult<T>(items, total, page, pageSize));
    }

    public Task<T> AddAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var id = GetId(entity);
        if (id == Guid.Empty)
        {
            id = Guid.NewGuid();
            SetId(entity, id);
        }

        if (_data.ContainsKey(id))
        {
            throw new InvalidOperationException($"Entity with id '{id}' already exists.");
        }

        _data[id] = entity;
        return Task.FromResult(entity);
    }

    public Task<T> UpdateAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var id = GetId(entity);
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Entity Id cannot be empty for update.", nameof(entity));
        }

        if (!_data.ContainsKey(id))
        {
            throw new KeyNotFoundException($"Entity with id '{id}' was not found.");
        }

        _data[id] = entity;
        return Task.FromResult(entity);
    }

    public Task RemoveByIdAsync(Guid id)
    {
        if (!_data.Remove(id))
        {
            throw new KeyNotFoundException($"Entity with id '{id}' was not found.");
        }

        return Task.CompletedTask;
    }

    private Guid GetId(T entity) => (Guid)_idProperty.GetValue(entity)!;

    private void SetId(T entity, Guid id) => _idProperty.SetValue(entity, id);
}

