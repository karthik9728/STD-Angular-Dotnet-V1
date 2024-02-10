using DatingApp.Domain.Models;
using DatingApp.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<AppUser>>> Get()
        {
            var users = await _dbContext.Users.ToListAsync();

            return Ok(users);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> Get(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x=>x.Id == id);

            return Ok(user);
        }
    }
}
