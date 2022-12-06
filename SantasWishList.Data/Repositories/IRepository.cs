using System.Linq.Expressions;
using SantasWishList.Data.Models;

namespace SantasWishList.Data.Repositories;

    public interface IRepository<T> where T : class, IEntity
    {
        Task<T?> Get(params Expression<Func<T, bool>>[] whereStatements);
        Task<List<T>> GetAll(params Expression<Func<T, bool>>[] parameters);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
    }