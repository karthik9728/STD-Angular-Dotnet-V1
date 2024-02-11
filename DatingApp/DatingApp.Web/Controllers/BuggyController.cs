using DatingApp.Domain.Models;
using DatingApp.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BuggyController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Authorize]
        [HttpGet]
        [Route("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet]
        [Route("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);

            if (thing == null) return NotFound();

            return thing;
        }

        [HttpGet]
        [Route("server-error")]
        public ActionResult<string> GetServerError()
        {

            var thing = _context.Users.Find(-1);

            var thinkToReturn = thing.ToString();

            return thinkToReturn;

        }

        [HttpGet]
        [Route("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("this was not a good request");
        }
    }
}
