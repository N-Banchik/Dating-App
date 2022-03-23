using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }

        public async Task<MemberDto> GetMemberAsync(string Username)
        {
            return await _context.Users.Where(u => u.UserName == Username).ProjectTo<MemberDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();

        }



        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();
            query = query.Where(u => u.UserName != userParams.CurrentUser);
            query = query.Where(u => u.Gender == userParams.Gender);

            var mindob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxdob = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(u => u.DateOfBirth >= mindob && u.DateOfBirth <= maxdob);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                "age" => query.OrderByDescending(u => u.DateOfBirth),
                _ => query.OrderByDescending(u => u.LastActive),

            };


            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(), userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string Username)
        {
            return await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(U => U.UserName == Username);
        }

        public Task<int> GetUserIdByUsernameAsync(string Username)
        {
            return _context.Users.Where(u => u.UserName == Username).Select(u => u.Id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {

            return await _context.SaveChangesAsync() > 0;
        }

        public void update(AppUser user)
        {
            _context.Entry<AppUser>(user).State = EntityState.Modified;
        }
    }
}