using Framework.Core.Helpers.Pagination;
using Framework.Core.Models.Entities;
using Framework.Core.Repositories.Interfaces;

namespace Framework.Repositories.Interfaces;

public interface IUserRepository : IBaseRepository<AppUser, long>
{
    Task<PagedList<AppUser>> LoadUserWithRolesAsync(string qtx = null, int page = 1, int size = 10,
        int? status = null, bool withDeleted = false);

    Task<AppUser> GetUserWithRolesAsync(long id, List<string> includeProperties = null,
        bool withDeleted = false);

}