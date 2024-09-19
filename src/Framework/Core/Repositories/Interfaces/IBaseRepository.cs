using System.Linq.Expressions;
using Framework.Core.Helpers.Pagination;
using Framework.Core.Models.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Framework.Core.Repositories.Interfaces;

public interface IBaseRepository<TEntity, TId> where TEntity : IBaseEntity<TId>
{
    #region Async

    Task<TEntity> GetAsync(TId id, List<string> includeProperties = null, bool withDeleted = false);
    Task<TEntity> GetAsync(string name, List<string> includeProperties = null, bool withDeleted = false);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, bool? withDeleted = null, bool asNoTracking = false);
    Task<List<TEntity>> LoadAsync(Expression<Func<TEntity, bool>> filter, bool? withDeleted = null);
    Task<PagedList<TEntity>> LoadAsync(string qtx = null!, int page = 1, int size = 10,  int? status=null, bool withDeleted = false);

    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task DeleteAsync(TId id);

    Task<bool> Complete();



    #endregion

    TEntity Get(TId id, List<string> includeProperties, bool withDeleted = false);
    TEntity Get(string name, List<string> includeProperties, bool withDeleted = false);
    TEntity Get(Expression<Func<TEntity, bool>> filter, bool? withDeleted);

    List<TEntity> Load(Expression<Func<TEntity, bool>> filter, bool? withDeleted);
    PagedList<TEntity> Load(string qtx = null!, int page = 1, int size = 10,  int? status=null, bool withDeleted = false);

    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void Delete(TId id);

    long ExecuteQueryCount<T>(IQueryable<T> query);





    IQueryable<TEntity> Query(bool? isActive=null, bool? isInclude=null, List<string> includeProperties=null, bool? withDeleted = false);

    #region Data Context

    public Task<IDbContextTransaction> BeginTransaction();


    #endregion
}