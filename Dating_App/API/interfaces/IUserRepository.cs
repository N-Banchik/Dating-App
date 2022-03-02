using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.interfaces
{
    public interface IUserRepository
    {
        public void update(AppUser user);

        public Task<bool> SaveAllAsync();

        public Task<IEnumerable<AppUser>> GetUsersAsync();
        public Task<AppUser> GetUserByIdAsync(int id);
        public Task<AppUser> GetUserByUsernameAsync(string Username);
        public Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        public Task<MemberDto> GetMemberAsync(string Username);



    }
}