using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.interfaces;
using API.DTOs;
using AutoMapper;

namespace API.Controllers
{

    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UsersController(IUserRepository userRepository,IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> Getusers()
        {
           IEnumerable<MemberDto> users = await userRepository.GetMembersAsync();
            return Ok(users);
        }

        [HttpGet("{username}")]//:username route paramater: api/users/3
        public async Task<ActionResult<MemberDto>> Getuser(string username)
        {
            MemberDto user = await userRepository.GetMemberAsync(username);
            return user;
        }


    }
}