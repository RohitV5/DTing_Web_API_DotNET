using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        public IMapper _mapper { get; set; }
        public UserManager<AppUser> _userManager { get; }

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            // Framework will figure out which implmentation of these service needs to be injected.
            // That we will inject in program.cs
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }


        [HttpPost("register")] // POST: api/account/register

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            //During initial development we returned AppUser instead of UserDto because we didnt had UserDto
            //UserDto returns data in token format inside on plain text data

            if (await UserExists(registerDto.Username))
            {
                return BadRequest("Username is taken");
            }
            //using keyword will dispose the variable once its unused. //for garbage collection
            var user = _mapper.Map<AppUser>(registerDto);


            user.UserName = registerDto.Username.ToLower();
            user.Gender = registerDto.Gender.ToLower();

            //creating an instance of hmac
            // using var hmac = new HMACSHA512();


            //the computed hash is specific to the salt that was generated in this instance
            //every time this function runs a new instance of hmac is generated and a hash is generated against a new salt
            //and that is why we are storing the salt in db for each user.
            // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            // user.PasswordSalt = hmac.Key;

     

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded) return BadRequest(result.Errors);


            /** Way 1 on creating dto object from class*/
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };

            /** Classic Way 2 on creating dto object from class*/
            //  UserDto userDto = new UserDto();

            //  userDto.Username = user.UserName;
            //  userDto.Token = _tokenService.CreateToken(user);
            //  return userDto;

        }


        private async Task<bool> UserExists(string username)
        {

            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null) return Unauthorized("invalid username");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result) return Unauthorized("Invalid password");

            return new UserDto
            {
                Username = user.UserName,
                Token =  _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }
    }
}