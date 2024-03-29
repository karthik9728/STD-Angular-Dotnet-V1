﻿using DatingApp.Application.DTO.Photo;
using DatingApp.Application.DTO.User;
using DatingApp.Application.InputModels;
using DatingApp.Application.Services.Interface;
using DatingApp.Application.ViewModels;
using DatingApp.Web.Extensions;
using DatingApp.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace DatingApp.Web.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IPhotoService _photoService;

        public UserController(IUserService userService, IPhotoService photoService)
        {
            _userService = userService;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AppUserDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            var username = User.GetUserName();

            var users = await _userService.GetUsersAsync(userParams,username);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPage));

            return Ok(users);
        }


        [HttpGet("{username}")]
        public async Task<ActionResult<AppUserDto>> GetUser(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);

            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(AppUserUpdateDto appUserUpdateDto)
        {
            var username = User.GetUserName();

            await _userService.UpdateUserAsync(appUserUpdateDto, username);

            if (await _userService.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");

        }

        [HttpPost]
        [Route("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {

            var username = User.GetUserName();

            var user = await _userService.GetUserByUsernameAsync(username);

            if (user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photoDto = new PhotoDto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            if (user.Photos.Count == 0)
            {
                photoDto.IsMain = true;
            }

            await _userService.UpdateUserPhotoAsync(photoDto, username);

            if (!await _userService.SaveAllAsync()) return BadRequest();

            return CreatedAtAction(nameof(GetUser), new { username = user.UserName }, photoDto);
        }

        [HttpPut]
        [Route("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var username = User.GetUserName();

            var user = await _userService.GetUserByUsernameAsync(username);

            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("this is already your main photo");

            await _userService.SetMainPhotoAsync(username, photoId);

            if (!await _userService.SaveAllAsync()) return BadRequest("Problem Setting Main Photo");

            return NoContent();

        }

        [HttpDelete]
        [Route("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var username = User.GetUserName();

            var user = await _userService.GetUserByUsernameAsync(username);

            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return BadRequest("this is photo is already deleted");

            if (photo.IsMain) return BadRequest("Main photo is not allowed to delete");

            var result = await _userService.DeletePhotoAsync(username, photoId);

            if (!result) return BadRequest("Something went wrong");

            if (!await _userService.SaveAllAsync()) return BadRequest("Problem deleting Photo");

            return Ok();

        }

    }
}
