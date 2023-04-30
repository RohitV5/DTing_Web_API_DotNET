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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public IMapper _mapper { get; set; }

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            // Framework will figure out which implmentation of these service needs to be injected.
            // That we will inject in program.cs
            _context = context;
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
            using var hmac = new HMACSHA512();


            //the computed hash is specific to the salt that was generated in this instance
            //every time this function runs a new instance of hmac is generated and a hash is generated against a new salt
            //and that is why we are storing the salt in db for each user.
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            _context.Users.Add(user);

            await _context.SaveChangesAsync();


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

            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if (user == null) return Unauthorized("invalid username");


            //this returns a byte array
            var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            /**Note: Wrong way to compare two byte array 
            **  This will never return true*/
            // if (user.PasswordHash == computedHash)
            // {
            //     return user;
            // }


            for (int i = 0; i < user.PasswordHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    var jsonResponse = JsonSerializer.Serialize(new
                    {
                        error = "invalid password"
                    });
                    return Unauthorized(jsonResponse);
                }

            }



            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };


            // In actual production its better to show a common error message if unathorized instead of being explicit
            // That way spammers won't know which value they are giving is incorrect.


        }
    }
}