using AutoMapper;
using DatingApp.Application.DTO.Photo;
using DatingApp.Application.DTO.User;
using DatingApp.Application.InputModels;
using DatingApp.Application.Services.Interface;
using DatingApp.Domain.Models;
using DatingApp.Infrastructure.Helpers;
using DatingApp.Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IPhotoService photoService, IMapper mapper)
        {
            _userRepository = userRepository;
            _photoService = photoService;
            _mapper = mapper;
        }

        public async Task<AppUserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            return _mapper.Map<AppUserDto>(user);
        }

        public async Task<AppUserDto> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            return _mapper.Map<AppUserDto>(user);
        }

        public async Task<PagedList<AppUserDto>> GetUsersAsync(UserParams userParams,string username)
        {
            var currentUser = await _userRepository.GetUserByUsernameAsync(username);

            userParams.CurrentUsername = currentUser.UserName;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
            }

            var users = await _userRepository.GetUsersAsync(userParams.PageNumber, userParams.PageSize,userParams.CurrentUsername,userParams.Gender);

            var usersDtoPagedList = new PagedList<AppUserDto>(
                                      _mapper.Map<List<AppUserDto>>(users),
                                      users.TotalCount,
                                      users.CurrentPage,
                                      users.PageSize);

            return usersDtoPagedList;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _userRepository.SaveAllAsync();
        }

        public void Update(AppUserDto userDto)
        {
            var user = _mapper.Map<AppUser>(userDto);

            _userRepository.Update(user);
        }

        public async Task UpdateUserAsync(AppUserUpdateDto appUserUpdateDto, string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            _mapper.Map(appUserUpdateDto, user);

            _userRepository.Update(user);
        }

        public async Task UpdateUserPhotoAsync(PhotoDto photoDto, string username)
        {

            var user = await _userRepository.GetUserByUsernameAsync(username);

            var photo = _mapper.Map<Photo>(photoDto);

            user.Photos.Add(photo);

            _userRepository.Update(user);
        }

        public async Task<bool> SetMainPhotoAsync(string username, int photoId)
        {

            var user = await _userRepository.GetUserByUsernameAsync(username);

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return false;

            if (photo.IsMain) return false;

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain == null) return false;

            currentMain.IsMain = false;
            photo.IsMain = true;

            _userRepository.Update(user);

            return true;
        }

        public async Task<bool> DeletePhotoAsync(string username, int photoId)
        {

            var user = await _userRepository.GetUserByUsernameAsync(username);

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return false;

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);

                if (result.Error != null) return false;
            }

            user.Photos.Remove(photo);

            _userRepository.Update(user);

            return true;
        }
    }
}
