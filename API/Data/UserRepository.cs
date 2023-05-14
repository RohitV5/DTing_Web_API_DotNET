using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<MemberDto>> GetUsersAsync()
        {
            //Will give a possible object cycle was detected
            return await _context.Users.Include(p => p.Photos).ProjectTo<MemberDto>(_mapper.ConfigurationProvider).ToListAsync();
            // return await _context.Users.ToListAsync(); //without photos
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;

            // savechangesasync will return number of row got affected and we are returning boolean.
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }


        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                    .Where(x => x.UserName == username)
                    .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            // Not the best way are you can run query on memberdto type instead of user type
            // var query = _context.Users
            //       .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            //       .AsNoTracking();

            var query = _context.Users.AsQueryable();

            query = query.Where(user => user.Gender.ToLower() == userParams.Gender.ToLower());
            query = query.Where(user => user.UserName != userParams.CurrentUsername);

            var minDOb = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDOb = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge - 1));

            query = query.Where(user => user.DateOfBirth >= minDOb && user.DateOfBirth <= maxDOb);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)

            };

            //convert user query to memberdto query
            var mappedQuery = query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider);



            return await PagedList<MemberDto>.CreateAsync(mappedQuery, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<string> GetUserGender(string username)
        {
            return await _context.Users.Where(user => user.UserName == username).Select(x => x.Gender).FirstOrDefaultAsync();
        }
    }

}