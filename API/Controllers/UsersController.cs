using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
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

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //Synchronous code
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() 
        //AppUser type is also valid. But how? because we are returning Ok. but that is wrong
        {
            var users = await _userRepository.GetUsersAsync();

            var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

            return Ok(usersToReturn);
        }


        //Synchronous code
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            var usersToReturn = _mapper.Map<MemberDto>(user);
            
            return usersToReturn;

        }


        //Make it async for scalability

        //To make a function async you need to use async await

        // [AllowAnonymous]
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<AppUser>>> GetUsersAsync()
        // {
        //     var users = await _context.Users.ToListAsync();

        //     return users;
        // }


        // [HttpGet("{id}")]
        // public async Task<ActionResult<AppUser>> GetUserAsync(int id)
        // {
        //     var user = await _context.Users.FindAsync(id);
        //     return user;

        // }
    }
}