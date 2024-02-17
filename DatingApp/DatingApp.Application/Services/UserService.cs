using AutoMapper;
using DatingApp.Application.DTO.Photo;
using DatingApp.Application.DTO.User;
using DatingApp.Application.Services.Interface;
using DatingApp.Domain.Models;
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
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
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

        public async Task<IEnumerable<AppUserDto>> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();

            return _mapper.Map<List<AppUserDto>>(users);
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
    }
}
