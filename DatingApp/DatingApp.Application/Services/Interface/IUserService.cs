using DatingApp.Application.DTO.Photo;
using DatingApp.Application.DTO.User;
using DatingApp.Application.InputModels;
using DatingApp.Domain.Models;
using DatingApp.Infrastructure.Helpers;
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

        Task<PagedList<AppUserDto>> GetUsersAsync(UserParams userParams, string username);

        Task<AppUserDto> GetUserByIdAsync(int id);

        Task<AppUserDto> GetUserByUsernameAsync(string username);

        Task UpdateUserAsync(AppUserUpdateDto appUserUpdateDto, string username);

        Task UpdateUserPhotoAsync(PhotoDto photoDto, string username);

        Task<bool> SetMainPhotoAsync(string username, int photoId);

        Task<bool> DeletePhotoAsync(string username, int photoId);
    }
}
