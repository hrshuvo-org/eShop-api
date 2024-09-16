using System.Linq.Expressions;
using Framework.Core.Helpers.Pagination;
using Microsoft.EntityFrameworkCore.Storage;

namespace Framework.Core.Services.Interfaces;

public interface IBaseService<TEntity, TId>
{
    Task<TEntity> GetAsync(TId id, List<string> includeProperties = null, bool withDeleted = false);
    Task<TEntity> GetAsync(string name, List<string> includeProperties = null, bool withDeleted = false);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, bool withDeleted = false);
    Task<List<TEntity>> LoadAsync(Expression<Func<TEntity, bool>> filter, bool withDeleted = false);
    Task<PagedList<TEntity>> LoadAsync(string qtx = null!, int page = 1, int size = 10,  int? status=null, bool withDeleted = false);

    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task DeleteAsync(TId id);
    
    Task<bool> Complete();

    Task<IDbContextTransaction> BeginTransaction();
}