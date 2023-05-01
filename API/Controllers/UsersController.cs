using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Helpers;
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
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        //AppUser type is also valid. But how? because we are returning Ok. but that is wrong
        {
            //Straightforward way of fetching AppUser List and convert to MemberDto type and return
            // var users = await _userRepository.GetUsersAsync();
            // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            // return Ok(usersToReturn);

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var currentUser = await _userRepository.GetUserByUsernameAsync(username);


            if(string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = currentUser.Gender.ToLower() == "male" ? "female" : "male";
            } 



            var users = await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);


        }


        //Synchronous code
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            //Straightforward way of fetching AppUser and convert to MemberDto type and return
            // var user = await _userRepository.GetUserByUsernameAsync(username);
            // var usersToReturn = _mapper.Map<MemberDto>(user);            
            // return usersToReturn;

            //Add automapper logic in UserRepository.
            return await _userRepository.GetMemberAsync(username);



        }


        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userRepository.GetUserByIdAsync(int.Parse(username)); //current userdata of type AppUser


            if (user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user); //will update user AppUser with updated values

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }


        //Make it async for scalability

        //To make a function async you need to use async await
        [Authorize(Roles = "Admin")]
        // [AllowAnonymous]
        [HttpGet("dbcheck")]

        public async Task<ActionResult<List<MemberDto>>> GetUsersAsync()
        {
            //Data fetching is dont inside repository.
            //In controller we get the Enumerable type which can be converted toList ok Action Result
            var users = await _userRepository.GetUsersAsync();

            //Just for testing interating over the enumerable list
            foreach (var user in users)
            {
                Console.WriteLine(user.UserName);
            }

            //Since we can't return 
            return Ok(users);   //same as users.ToList();
        }


        // [HttpGet("{id}")]
        // public async Task<ActionResult<AppUser>> GetUserAsync(int id)
        // {
        //     var user = await _context.Users.FindAsync(id);
        //     return user;

        // }
    }
}