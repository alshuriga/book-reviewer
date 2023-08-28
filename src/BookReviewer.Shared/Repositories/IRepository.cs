using System.Linq.Expressions;
using BookReviewer.Shared.Entities;

namespace BookReviewer.Shared.Repositories;

public interface IRepository<T> where T : Entity 
{
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAsync();
    Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}