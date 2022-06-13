using AutoMapper;
using Geolocator.Data.Models;
using Geolocator.DTO;

namespace Geolocator.Services.Mappings.Profiles;

public class IpInfoProfile : Profile
{
    public IpInfoProfile()
    {
        CreateMap<IpInfo, IpInfoDto>()
            .ReverseMap();
    }
}