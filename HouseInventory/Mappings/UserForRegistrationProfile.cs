using AutoMapper;
using HouseInventory.Data.Entities;
using HouseInventory.Models.DTOs;
using System.Diagnostics.CodeAnalysis;

namespace HouseInventory.Mappings
{
    [ExcludeFromCodeCoverage]
    public class UserForRegistrationProfile : Profile
    {
        public UserForRegistrationProfile()
        {
            CreateMap<UserRegistrationDto, User>();
        }
    }
}
