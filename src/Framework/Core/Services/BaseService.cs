using System.Linq.Expressions;
using Framework.Core.Helpers.Pagination;
using Framework.Core.Models.Entities;
using Framework.Core.Repositories.Interfaces;
using Framework.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Framework.Core.Services;

public class BaseService<TEntity, TId>: IBaseService<TEntity, TId> where TEntity : IBaseEntity<TId>
{
    #region Initialization

    private readonly IBaseRepository<TEntity, TId> _repo;

    public BaseService(IBaseRepository<TEntity, TId> repo)
    {
        _repo = repo;
    }

    #endregion

    public virtual async Task<TEntity> GetAsync(TId id, List<string> includeProperties = null, bool withDeleted = false)
    {
        return await _repo.GetAsync(id, includeProperties, withDeleted);
    }

    public virtual async Task<TEntity> GetAsync(string name, List<string> includeProperties = null, bool withDeleted = false)
    {
        return await _repo.GetAsync(name, includeProperties, withDeleted);
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, bool withDeleted = false, bool asNoTracking = false)
    {
        return await _repo.GetAsync(filter, withDeleted, asNoTracking);
    }

    public async Task<List<TEntity>> LoadAsync(Expression<Func<TEntity, bool>> filter, bool withDeleted = false)
    {
        return await _repo.LoadAsync(filter, withDeleted);
    }

    public virtual async Task<PagedList<TEntity>> LoadAsync(string qtx = null!, int page = 1, int size = 10, int? status = null, bool withDeleted = false)
    {
        return await _repo.LoadAsync(qtx, page, size, status, withDeleted);
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        entity.CreatedTime = DateTime.Now;
        entity.UpdatedTime = DateTime.Now;
    
        await _repo.AddAsync(entity);
        await _repo.Complete();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        entity.UpdatedTime = DateTime.Now;
    
        await _repo.UpdateAsync(entity);
        await _repo.Complete();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        await _repo.DeleteAsync(entity);
        await _repo.Complete();
    }

    public async Task DeleteAsync(TId id)
    {
        await _repo.DeleteAsync(id);
        await _repo.Complete();
    }

    public async Task<bool> Complete()
    {
        return await _repo.Complete();
    }

    public async Task<IDbContextTransaction> BeginTransaction()
    {
        return await _repo.BeginTransaction();
    }
}