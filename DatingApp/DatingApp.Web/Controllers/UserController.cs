using DatingApp.Application.DTO.Photo;
using DatingApp.Application.DTO.User;
using DatingApp.Application.Services.Interface;
using DatingApp.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace DatingApp.Web.Controllers
{
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
        public async Task<ActionResult<List<AppUserDto>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();

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
    }
}
