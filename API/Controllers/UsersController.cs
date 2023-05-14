using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
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

        // private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            // _userRepository = userRepository;           
            _mapper = mapper;
            _photoService = photoService;
        }

        //Synchronous code
        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        //AppUser type is also valid. But how? because we are returning Ok. but that is wrong
        {
            //Straightforward way of fetching AppUser List and convert to MemberDto type and return
            // var users = await _userRepository.GetUsersAsync();
            // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            // return Ok(usersToReturn);

            //Getting current user gender to search for users of opposite gender
            var gender = await _unitOfWork.UserRepository.GetUserGender(User.GetUsername());
            userParams.CurrentUsername = User.GetUsername();

            // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // var currentUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);


            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = gender?.ToLower() == "male" ? "female" : "male";
            }



            var users = await _unitOfWork.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);


        }


        //Synchronous code
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            //Straightforward way of fetching AppUser and convert to MemberDto type and return
            // var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            // var usersToReturn = _mapper.Map<MemberDto>(user);            
            // return usersToReturn;

            //Add automapper logic in UserRepository.
            return await _unitOfWork.UserRepository.GetMemberAsync(username);



        }


        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            //Using extenstion of claimprinciple instead direct
            // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = User.GetUserId();

            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId); //current userdata of type AppUser


            if (user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user); //will update user AppUser with updated values

            if (await _unitOfWork.Complete()) return NoContent();

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
            var users = await _unitOfWork.UserRepository.GetUsersAsync();

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

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0) photo.IsMain = true;

            user.Photos.Add(photo); //Entity framework is tracking this is memory.


            if (await _unitOfWork.Complete())

                // To send 201 Created response and Location header 
                return CreatedAtAction(nameof(GetUser),
                    new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));


            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("this is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Problem setting the main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem deleting photo");
        }
    }
}