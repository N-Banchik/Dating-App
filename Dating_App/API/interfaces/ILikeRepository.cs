using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.interfaces
{
    public interface ILikeRepository
    {
        public Task<UserLike> GetLike(int sourceUserId, int targetUserId);
        public Task<PagedList<LikeDto>> GetUserlikesAsync(LikeParams likeParams);
        public Task<AppUser> GetLikedByUsersAsync(int userId);
        public void Add(UserLike entity);

        public Task<bool> SaveAllAsync();



    }
}