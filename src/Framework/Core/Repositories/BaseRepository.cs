using System.Linq.Expressions;
using Framework.Core.Data;
using Framework.Core.Helpers.Pagination;
using Framework.Core.Models;
using Framework.Core.Models.Entities;
using Framework.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Framework.Core.Repositories;

public class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId> where TEntity : class, IBaseEntity<TId>
{
    private DbContext Context { get; }
    protected DbSet<TEntity> DbSet { get; }

    public BaseRepository(DataContext context)
    {
        Context = context;
        DbSet = Context.Set<TEntity>();
    }


    #region Async Methods
    
    public async Task<TEntity> GetAsync(TId id, List<string> includeProperties=null, bool withDeleted = false)
    {
        IQueryable<TEntity> query = Query(null, null, includeProperties, withDeleted);

        query = query.Where(x => x.Id!.Equals(id));
        return await ExecuteQueryAsync(query);
    }
    
    public async Task<TEntity> GetAsync(string name, List<string> includeProperties, bool withDeleted = false)
    {
        IQueryable<TEntity> tempDbSet = DbSet.AsNoTracking();

        tempDbSet = tempDbSet.Where(x => x.Name == name);

        if (includeProperties != null!)
        {
            tempDbSet = includeProperties.Aggregate(tempDbSet, (current, item) => current.Include(item));
        }

        if (withDeleted == false)
            tempDbSet = tempDbSet.Where(x => x.Status != EntityStatus.Deleted);

        return await ExecuteQueryAsync(tempDbSet);
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, bool? withDeleted)
    {
        var queryable = Query(withDeleted:withDeleted).Where(filter);
        var entity = await ExecuteQueryAsync(queryable);
        return entity!;
    }

    public async Task<List<TEntity>> LoadAsync(Expression<Func<TEntity, bool>> filter, bool? withDeleted)
    {
        var queryable = Query(withDeleted:withDeleted).Where(filter);
        var entityList = await ExecuteQueryListAsync(queryable);
        return entityList;
    }
    
    public virtual async Task<PagedList<TEntity>> LoadAsync(string qtx = null!, int page = 1, int size = 10, int? status=null, bool withDeleted = false)
    {
        IQueryable<TEntity> tempDbSet = DbSet;

        if (status is not null)
        {
            tempDbSet = tempDbSet.Where(x => x.Status == status);
        }

        if (withDeleted == false)
        {
            tempDbSet = tempDbSet.Where(x => x.Status != EntityStatus.Deleted);
        }

        if(qtx is not null)
            tempDbSet = tempDbSet.Where(x => x.Name!.ToLower().Contains(qtx.ToLower()));

        IQueryable<TEntity> query = tempDbSet.AsQueryable().AsNoTracking();

        var result = await PagedList<TEntity>.CreateAsync(query, page, size);

        return result;
    }

    public async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        DbSet.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
    }

    public async Task DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public async Task DeleteAsync(TId id)
    {
        var entity = await GetAsync(id);
         await DeleteAsync(entity!);
    }

    public async Task<bool> Complete()
    {
        return await Context.SaveChangesAsync() > 0;
    }


    #region Private Methods

    private async Task<T> ExecuteQueryAsync<T>(IQueryable<T> query)
    {
        return await query.FirstOrDefaultAsync();
    }

    private async Task<List<T>> ExecuteQueryListAsync<T>(IQueryable<T> query)
    {
        return await query.ToListAsync();
    }

    #endregion
    
    #endregion

    public TEntity Get(TId id, List<string> includeProperties = null, bool withDeleted = false)
    {
        IQueryable<TEntity> query = Query(null, null, includeProperties, withDeleted);

        query = query.Where(x => x.Id!.Equals(id));
        return ExecuteQuery(query);
    }

    public TEntity Get(string name, List<string> includeProperties, bool withDeleted = false)
    {
        IQueryable<TEntity> tempDbSet = DbSet;

        if (includeProperties != null!)
        {
            tempDbSet = includeProperties.Aggregate(tempDbSet, (current, item) => current.Include(item));
        }

        if (withDeleted == false)
            tempDbSet = tempDbSet.Where(x => x.Status != EntityStatus.Deleted);

        return ExecuteQuery(tempDbSet);
    }

    public TEntity Get(Expression<Func<TEntity, bool>> filter, bool? withDeleted)
    {
        var queryable = Query(withDeleted:withDeleted).Where(filter);
        var entity = ExecuteQueryList(queryable).FirstOrDefault();
        return entity!;
    }

    public List<TEntity> Load(Expression<Func<TEntity, bool>> filter, bool? withDeleted)
    {
        var queryable = Query(withDeleted:withDeleted).Where(filter);
        var entity = ExecuteQueryList(queryable);
        return entity;
    }

    public PagedList<TEntity> Load(string qtx = null!, int page = 1, int size = 10, int? status=null, bool withDeleted = false)
    {
        IQueryable<TEntity> tempDbSet = DbSet;

        if (status is not null)
        {
            tempDbSet = tempDbSet.Where(x => x.Status == status);
        }

        if (withDeleted == false)
        {
            tempDbSet = tempDbSet.Where(x => x.Status != EntityStatus.Deleted);
        }
        
        if(qtx is not null)
            tempDbSet = tempDbSet.Where(x => x.Name!.ToLower().Contains(qtx.ToLower()));

        IQueryable<TEntity> query = tempDbSet.AsQueryable().AsNoTracking();

        var result = PagedList<TEntity>.Create(query, page, size);

        return result;
    }

    public void Add(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public void Update(TEntity entity)
    {
        DbSet.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public void Delete(TId id)
    {
        var entity = Get(id);
        Delete(entity);
    }


    #region Private Methods

    private T ExecuteQuery<T>(IQueryable<T> query)
    {
        return query.FirstOrDefault()!;
    }

    private List<T> ExecuteQueryList<T>(IQueryable<T> query)
    {
        return query.ToList();
    }

    public long ExecuteQueryCount<T>(IQueryable<T> query)
    {
        return query.Count();;
    }
    

    #endregion

    #region Helper Method
    
    public IQueryable<TEntity> Query(bool? isActive=null!, bool? isInclude=null, List<string> includeProperties=null,
        bool? withDeleted = false)
    {
        IQueryable<TEntity> query = DbSet;
        
        if (isActive is not null)
        {
            query = isActive.Value
                ? query.Where(x => x.Status == EntityStatus.Active)
                : query.Where(x => x.Status == EntityStatus.Inactive);
        }
        if (includeProperties is not null)
        {
            // foreach (var includeProp in includeProperties.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries))
            // {
            //     query = query.Include(includeProp);
            // }
            query = includeProperties.Aggregate(query, (current, includeProp) => current.Include(includeProp.Trim()));
        }

        if (withDeleted is false)
        {
            query = query.Where(x => x.Status != EntityStatus.Deleted);
        }

        return query;
    }

    #endregion

    #region DataContext

    public async Task<IDbContextTransaction> BeginTransaction()
    {
        return await Context.Database.BeginTransactionAsync();
    }

    #endregion
    
}