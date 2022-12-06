using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SantasWishList.Data.Models;

namespace SantasWishList.Data.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly DatabaseContext _context;
    protected readonly DbSet<T> _entity;

    protected BaseRepository(DatabaseContext context)
    {
        _context = context;
        _entity = _context.Set<T>();
    }

    public virtual async Task<T?> Get(params Expression<Func<T, bool>>[] whereStatements)
    {
        return await whereStatements
            .Aggregate(
                _entity.AsQueryable(),
                (current, where) => current.Where(where)
            )
            .FirstOrDefaultAsync();
    }

    public virtual async Task<List<T>> GetAll(params Expression<Func<T, bool>>[] whereStatements)
    {
        return await whereStatements
            .Aggregate(
                _entity.AsQueryable(),
                (current, where) => current.Where(where)
            )
            .ToListAsync();
    } 

    public virtual async Task<bool> Create(T entity)
    {
        try
        {
            await _context.AddAsync<T>(entity);
            await _context.SaveChangesAsync();
        }
        catch
        { }

        return true;
    }

    public async Task<bool> Update(T entity)
    {
        try
        {
            _context.Update<T>(entity);
            await _context.SaveChangesAsync();
        }
        catch
        { }

        return true;
    }

    public async Task<bool> Delete(T entity)
    {
        try
        {
            _context.Remove<T>(entity);
            await _context.SaveChangesAsync();
        }
        catch
        { }

        return true;
    }
}
