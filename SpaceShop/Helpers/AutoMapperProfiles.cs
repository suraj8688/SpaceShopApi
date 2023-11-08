using AutoMapper;
using SpaceShop.Dto;
using SpaceShop.Models;

namespace SpaceShop.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<City, CityNameUpdateDto>().ReverseMap();
            CreateMap<Property, PropertyListDto>()
                .ForMember(x => x.City, opt => opt.MapFrom(x => x.City.Name))
                .ForMember(x => x.Country, opt => opt.MapFrom(x => x.City.Country))
                .ForMember(x => x.PropertyType, opt => opt.MapFrom(x => x.PropertyType.Name))
                .ForMember(x => x.FurnishingType, opt => opt.MapFrom(x => x.FurnishingType.Name));
           
        }
    }
}
