using Framework.Core.Helpers.Pagination;
using Framework.Core.Models.Dtos;
using Framework.Core.Models.Entities;
using Framework.Core.Services.Interfaces;

namespace Framework.Services.Interfaces;

public interface IUserService : IBaseService<AppUser, long>
{
    Task<Pagination<AppUserDetailsDto>> LoadAsync(string qtx = null!, int page = 1, int size = 10,
        int? status = null, bool withDeleted = false);

    Task<AppUserDetailsDto> GetAsync(long id, List<string> includeProperties = null, bool withDeleted = false);


    #region Admin

    Task<Pagination<UserWithRoleDto>> LoadUserWithRolesAsync(string qtx = null!, int page = 1, int size = 10,
        int? status = null, bool withDeleted = false);

    Task<UserWithRoleDto> GetUserWithRolesAsync(long id, List<string> includeProperties = null,
        bool withDeleted = false);

    #endregion
}