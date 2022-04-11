using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models;
using HotelListing.API.Models.Users;

namespace HotelListing.API.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountrydto>().ReverseMap();
            CreateMap<Country, GetCountrydto>().ReverseMap();
            CreateMap<Country, UpdateCountrydto>().ReverseMap();
            CreateMap<Country, Countrydto>().ReverseMap();

            CreateMap<Hotel, Hoteldto>().ReverseMap();
            CreateMap<Hotel, CreateHoteldto>().ReverseMap();
            CreateMap<ApiUserdto, ApiUser>().ReverseMap();
        }

    }
}
