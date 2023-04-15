using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    //  api/users
    //  We created BaseApiController to put some controller boilerplate
    //  https://learn.microsoft.com/en-us/aspnet/core/security/authorization/simple?view=aspnetcore-7.0
    [Authorize]  //This guard can be used at method level or controller level     
    public class UsersController : BaseApiController
    {

        /* One way of initililaizing */
        // private readonly DataContext context;
        // public UsersController(DataContext context)
        // {
        //     this.context = context;

        // }

        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;

        }

        //Synchronous code
        [HttpGet("sync")]
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        {
            var users = _context.Users.ToList();

            return users;
        }


        //Synchronous code
        [HttpGet("sync/{id}")]
        public ActionResult<AppUser> GetUser(int id)
        {
            var user = _context.Users.Find(id);
            return user;

        }


        //Make it async for scalability

        //To make a function async you need to use async await
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;

        }
    }
}