using HouseInventory.Data.Entities;
using HouseInventory.Models.DTOs;

namespace HouseInventory.Services.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<User> GoogleSignInAsync(GoogleSignInVMDto googleSignInDto);
    }
}
