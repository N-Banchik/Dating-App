using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public LikeRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<UserLike> GetLike(int sourceUserId, int targetUserId)
        {
            return await _context.likes.FirstOrDefaultAsync(x => x.SourceUserId == sourceUserId && x.TargetUserId == targetUserId);
        }

        public async Task<PagedList<LikeDto>> GetUserlikesAsync(LikeParams Paramas)
        {
            IQueryable<AppUser> users;
            IQueryable<UserLike> likes = _context.likes.AsQueryable();
            if (Paramas.predicate == "likedBy")
            {
                users = likes.Where(x => x.TargetUserId == Paramas.userId).Select(like => like.SourceUser);
            }
            else
            {
                users = likes.Where(x => x.SourceUserId == Paramas.userId).Select(like => like.TargetUser);
            }

            var likedusers= users.Select(users => new LikeDto
            {
                Id = users.Id,
                Username = users.UserName,
                Age = users.DateOfBirth.CalculateAge(),
                KnownAs = users.KnownAs,
                PhotoUrl = users.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = users.City,
                Country = users.Country
            });
            return await PagedList<LikeDto>.CreateAsync(likedusers, Paramas.PageNumber, Paramas.PageSize);
        }

        public async Task<AppUser> GetLikedByUsersAsync(int userId)
        {
            return await _context.Users.Include(x => x.LikedUsers).FirstOrDefaultAsync(x => x.Id == userId);
        }

        public void Add(UserLike like)
        {
            _context.likes.Add(like);
        }

        public async Task<bool> SaveAllAsync()
        {

            return await _context.SaveChangesAsync() > 0;
        }
    }
}