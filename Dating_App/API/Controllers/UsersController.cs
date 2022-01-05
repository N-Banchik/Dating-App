using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{

    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AppUser>>> Getusers()
        {

            var users = await _context.Users.ToListAsync();
            return users;
        }

        [Authorize]
        [HttpGet("{id}")]//:id route paramater: api/users/3
        public async Task<ActionResult<AppUser>> Getuser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }


    }
}