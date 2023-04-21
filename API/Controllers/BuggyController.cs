using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "A";
        }


        [Authorize]
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            AppUser things = _context.Users.Find(-1);

            if (this is null) return NotFound();

            return things;
        }


        [Authorize]
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            AppUser thing = _context.Users.Find(-1);

            var thingsToReturn = thing.ToString();

            return thingsToReturn;
        }


        [Authorize]
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("this was not a good request");
        }
    }
}