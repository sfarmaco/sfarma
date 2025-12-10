using Microsoft.EntityFrameworkCore;
using Sfarma.Api.Data;
using Sfarma.Api.Interfaces;
using System.Linq.Expressions;

namespace Sfarma.Api.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly SfarmaContext _context;
    private readonly DbSet<T> _set;

    public GenericRepository(SfarmaContext context)
    {
        _context = context;
        _set = context.Set<T>();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _set.AddAsync(entity);
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _set.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await _set.Where(predicate).ToListAsync();

    public async Task<IEnumerable<T>> GetAllAsync() => await _set.ToListAsync();

    public async Task<T?> GetByIdAsync(int id) => await _set.FindAsync(id);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task UpdateAsync(T entity)
    {
        _set.Update(entity);
        await Task.CompletedTask;
    }
}
