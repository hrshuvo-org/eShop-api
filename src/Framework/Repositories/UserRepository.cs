using Framework.Core.Data;
using Framework.Core.Helpers.Pagination;
using Framework.Core.Models;
using Framework.Core.Models.Entities;
using Framework.Core.Repositories;
using Framework.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Framework.Repositories;

public class UserRepository: BaseRepository<AppUser, long>, IUserRepository
{
    public UserRepository(DataContext context) : base(context)
    {
    }

    // public override async Task<PagedList<AppUser>> LoadAsync(string qtx = null!, int page = 1, int size = 10, int? status=null, bool withDeleted = false)
    // {
    //     IQueryable<AppUser> tempDbSet = DbSet;
    //
    //     if (status is not null)
    //     {
    //         tempDbSet = tempDbSet.Where(x => x.Status == status);
    //     }
    //
    //     if (withDeleted == false)
    //     {
    //         tempDbSet = tempDbSet.Where(x => x.Status != EntityStatus.Deleted);
    //     }
    //
    //     if((qtx!) is not null)
    //         tempDbSet = tempDbSet.Where(x => x.Name!.ToLower().Contains(qtx.ToLower()));
    //
    //     IQueryable<AppUser> query = tempDbSet.AsQueryable().AsNoTracking();
    //
    //     // query = query.Include(u => u.Photos);
    //
    //     var result = await PagedList<AppUser>.CreateAsync(query, page, size);
    //
    //     return result;
    // }

    public async Task<PagedList<AppUser>> LoadUserWithRolesAsync(string qtx = null, int page = 1, int size = 10, int? status = null,
        bool withDeleted = false)
    {
        IQueryable<AppUser> tempDbSet = DbSet;

        if (status is not null)
        {
            tempDbSet = tempDbSet.Where(x => x.Status == status);
        }

        if (withDeleted == false)
        {
            tempDbSet = tempDbSet.Where(x => x.Status != EntityStatus.Deleted);
        }
    
        if((qtx!) is not null)
            tempDbSet = tempDbSet.Where(x => x.Name!.ToLower().Contains(qtx.ToLower()));

        IQueryable<AppUser> query = tempDbSet.AsQueryable().AsNoTracking();

        query = query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role);
        
        var result = await PagedList<AppUser>.CreateAsync(query, page, size);

        return result;
    }

    public async Task<AppUser> GetUserWithRolesAsync(long id, List<string> includeProperties = null, bool withDeleted = false)
    {
        IQueryable<AppUser> query = Query(null, null, includeProperties, withDeleted);

        query = query.Where(x => x.Id!.Equals(id));
        
        query = query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).AsNoTracking();

        var result = await query.FirstOrDefaultAsync();

        return result;
    }
}