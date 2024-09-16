using AutoMapper;
using Framework.Core.Helpers.Pagination;
using Framework.Core.Models.Dtos;
using Framework.Core.Models.Entities;
using Framework.Core.Services;
using Framework.Repositories.Interfaces;
using Framework.Services.Interfaces;

namespace Framework.Services;

public class UserService : BaseService<AppUser, long>, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repo, IMapper mapper) : base(repo)
    {
        _userRepository = repo;
        _mapper = mapper;
    }

    public async Task<Pagination<AppUserDetailsDto>> LoadAsync(string qtx = null, int page = 1, int size = 10,
        int? status = null, bool withDeleted = false)
    {
        var userList = await _userRepository.LoadAsync(qtx, page, size, status, withDeleted);

        // var userDtoLIst = userList.Select(user =>
        // {
        //     AppUserDetailsDto userDto = new()
        //     {
        //         Id = user.Id,
        //         Name = user.UserName!,
        //         UserName = user.UserName!,
        //         Email = user.Email!,
        //         Status = user.Status,
        //
        //         CreatedTime = user.CreatedTime,
        //         UpdatedTime = user.UpdatedTime,
        //         LastActive = user.LastActive
        //     };
        //
        //     return userDto;
        // }).ToList();
        
        var userToReturn = _mapper.Map<List<AppUserDetailsDto>>(userList);

        return new Pagination<AppUserDetailsDto>(userList.CurrentPage, userList.PageSize, userList.TotalCount, userList.TotalPage, userToReturn);
    }


    public async Task<AppUserDetailsDto> GetAsync(long id, List<string> includeProperties = null, bool withDeleted = false)
    {
        var user = await _userRepository.GetAsync(id, includeProperties, withDeleted);

        var userToReturn = _mapper.Map<AppUserDetailsDto>(user);

        return userToReturn;
    }


    #region Admin

    public async Task<Pagination<UserWithRoleDto>> LoadUserWithRolesAsync(string qtx = null, int page = 1, int size = 10,
        int? status = null, bool withDeleted = false)
    {
        var userList = await _userRepository.LoadUserWithRolesAsync(qtx, page, size, status, withDeleted);

        var result = new Pagination<UserWithRoleDto>(userList.CurrentPage, userList.PageSize, userList.TotalCount, userList.TotalPage, userList.Select(u => new UserWithRoleDto()
        {
            Id = u.Id,
            Name = u.Name,
            UserName = u.UserName,
            Email = u.Email,
            Status = u.Status,
            CreatedTime = u.CreatedTime,
            UpdatedTime = u.UpdatedTime,
            LastActive = u.LastActive,

            Roles = u.UserRoles
                .Where(ur => ur is not null)
                .Select(ur => new RoleDto()
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name
                }).ToList()
        }).ToList());

        return result;
    }
    
    
    public async Task<UserWithRoleDto> GetUserWithRolesAsync(long id, List<string> includeProperties = null, bool withDeleted = false)
    {
        var user = await _userRepository.GetUserWithRolesAsync(id, null, withDeleted);

        var userToReturn = _mapper.Map<UserWithRoleDto>(user);

        return userToReturn;
    }

    #endregion
}