using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]  // api/users
    public class UsersController : ControllerBase
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