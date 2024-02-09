using DatingApp.Application.DTO.User;
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

        public AccountController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<AppUser>> Register([FromBody] RegisterDto registerDto)
        {
            using var hmac = new HMACSHA512();

            if (await IsUserExists(registerDto.UserName)) return BadRequest(new { message = "Username is already taken,try different username" });


            var user = new AppUser
            {
                UserName = registerDto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<AppUser>> Login([FromBody] LoginDto loginDto)
        {
            if (!await IsUserExists(loginDto.UserName)) return Unauthorized(new { message = "Invalid Username" });

            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);


            //Genearte hash based user password and user password salt
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            //compare computed hash password with stored hash password of user
            for (int i = 0; i < computedHash.Length; i++)
            {
                //compare if computed hash password is different from stored hash password then its incorrect password
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized(new { message = "Incorrect Password" });  
            }

            return Ok(user);
        }

        private async Task<bool> IsUserExists(string userName)
        {
            return await _dbContext.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}
