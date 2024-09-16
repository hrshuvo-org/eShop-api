using AutoMapper;
using Framework.Core.Models.Dtos;
using Framework.Core.Models.Entities;

namespace Framework.Core.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<AppUser, AppUserDetailsDto>();
    }
}