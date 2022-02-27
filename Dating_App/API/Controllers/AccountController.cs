using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context
        , ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _context = context;
        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            using HMACSHA512 hmac = new HMACSHA512();
            if (await UserExists(registerDto.Username)) return BadRequest("Username Already exists");

            AppUser user = _mapper.Map<AppUser>(registerDto);
            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,

            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if (user == null) return Unauthorized("Invalid username");

            using HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);

            var computedhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedhash.Length; i++)
            {
                if (computedhash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("invalid password");
                }
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
            };
        }
    }
}