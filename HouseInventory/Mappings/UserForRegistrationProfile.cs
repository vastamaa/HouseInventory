using AutoMapper;
using HouseInventory.Data.Entities;
using HouseInventory.Models.DTOs;

namespace HouseInventory.Mappings
{
    public class UserForRegistrationProfile : Profile
    {
        public UserForRegistrationProfile()
        {
            CreateMap<UserRegistrationDto, User>();
        }
    }
}
