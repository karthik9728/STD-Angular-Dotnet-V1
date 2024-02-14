using DatingApp.Application.DTO.User;
using DatingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Application.Services.Interface
{
    public interface IUserService
    {
        void Update(AppUserDto userDto);

        Task<bool> SaveAllAsync();

        Task<IEnumerable<AppUserDto>> GetUsersAsync();

        Task<AppUserDto> GetUserByIdAsync(int id);

        Task<AppUserDto> GetUserByUsernameAsync(string username);

        Task UpdateUserAsync(AppUserUpdateDto appUserUpdateDto,string username);
    }
}
