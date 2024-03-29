﻿using AutoMapper;
using DatingApp.Application.DTO.User;
using DatingApp.Application.Services.Interface;
using DatingApp.Domain.Models;
using DatingApp.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(ApplicationDbContext dbContext, ITokenService tokenService, IMapper mapper)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _mapper = mapper;
        }


        /// <summary>
        /// Register User 
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            using var hmac = new HMACSHA512();

            if (await IsUserExists(registerDto.UserName)) return BadRequest("Username is already taken,try different username");

            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.UserName;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;


            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<AppUser>> Login([FromBody] LoginDto loginDto)
        {
            if (!await IsUserExists(loginDto.Username)) return Unauthorized("Invalid Username");

            var user = await _dbContext.Users.Include(x => x.Photos).SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            //Genearte hash based user password and user password salt
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            //compare computed hash password with stored hash password of user
            for (int i = 0; i < computedHash.Length; i++)
            {
                //compare if computed hash password is different from stored hash password then its incorrect password
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Incorrect Password");
            }

            var token = _tokenService.CreateToken(user);

            string photo = "";

            if (user.Photos.Any())
            {
                photo = user.Photos.FirstOrDefault(x => x.IsMain).Url;
            }

            UserDto userDto = new UserDto
            {
                Username = user.UserName,
                Token = token,
                PhotoUrl = photo,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };

            return Ok(userDto);
        }

        private async Task<bool> IsUserExists(string userName)
        {
            return await _dbContext.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}
