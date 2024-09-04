using AutoMapper;
using HouseInventory.Data.Entities;
using HouseInventory.Models.DTOs;

namespace HouseInventory.Mappings
{
    public class UserForRegistrationProfile : Profile
    {
        protected UserForRegistrationProfile()
        {
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
