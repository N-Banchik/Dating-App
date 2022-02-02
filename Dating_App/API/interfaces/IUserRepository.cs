using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.interfaces
{
    public interface IUserRepository
    {
        public void update(AppUser user);

        public Task<bool> SaveAllAsync();

        public Task<IEnumerable<AppUser>> GetUsersAsync();
        public Task<AppUser> GetUserByIdAsync(int id);
        public Task<AppUser> GetUserByUsernameAsync(string Username);
        public Task<IEnumerable<MemberDto>> GetMembersAsync();
        public Task<MemberDto> GetMemberAsync(string Username);



    }
}