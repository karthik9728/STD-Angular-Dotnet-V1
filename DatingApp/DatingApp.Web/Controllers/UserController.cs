using DatingApp.Application.DTO.User;
using DatingApp.Application.Services.Interface;
using DatingApp.Domain.Models;
using DatingApp.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Web.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AppUserDto>>> Get()
        {
            var users = await _userService.GetUsersAsync();

            return Ok(users);
        }


        [HttpGet("{username}")]
        public async Task<ActionResult<AppUserDto>> Get(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);

            return Ok(user);
        }
    }
}
