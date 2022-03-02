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
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using API.Extensions;
using CloudinaryDotNet.Actions;
using API.Helpers;

namespace API.Controllers
{

    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            this.userRepository = userRepository;
            this.mapper = mapper;

        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            string username = User.GetUsername();
            AppUser user = await userRepository.GetUserByUsernameAsync(username);
            mapper.Map(memberUpdateDto, user);
            userRepository.update(user);
            if (await userRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Could not update user");
        }


        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> Getusers([FromQuery] UserParams userParams)
        
        {
            var users = await userRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }

        [HttpGet("{username}", Name = "GetUser")]//:username route paramater: api/users/3
        public async Task<ActionResult<MemberDto>> Getuser(string username)
        {
            MemberDto user = await userRepository.GetMemberAsync(username);
            return Ok(user);
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            string username = User.GetUsername();
            AppUser user = await userRepository.GetUserByUsernameAsync(username);
            ImageUploadResult result = await _photoService.UploadPhotoAsync(file);
            if (result.Error != null)
            {
                return BadRequest("Could not upload photo");
            };
            Photo photo = new Photo()
            {
                Url = result.SecureUrl.AbsoluteUri.ToString(),
                PublicId = result.PublicId
            };
            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }
            user.Photos.Add(photo);
            if (await userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUser", new { username = user.UserName }, mapper.Map<PhotoDto>(photo));
                //return mapper.Map<PhotoDto>(photo);

            }
            return BadRequest("Could not add photo");



        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            string username = User.GetUsername();
            AppUser user = await userRepository.GetUserByUsernameAsync(username);
            Photo photo = user.Photos.FirstOrDefault(p => p.id == photoId);
            if (photo.IsMain)
            {
                return NoContent();
            }
            Photo currentmainPhoto = user.Photos.FirstOrDefault(p => p.IsMain);
            currentmainPhoto.IsMain = false;
            photo.IsMain = true;
            if (await userRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Could not set photo to main");
        }


        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            string username = User.GetUsername();
            AppUser user = await userRepository.GetUserByUsernameAsync(username);
            Photo photo = user.Photos.FirstOrDefault(p => p.id == photoId);
            if(photo == null)
            {
                return BadRequest("Could not find photo");
            }
            if (photo.IsMain)
            {
                return BadRequest("You cannot delete your main photo");
            }
            if (photo.PublicId != null)
            {
                DeletionResult result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Result == "ok")
                {
                    user.Photos.Remove(photo);
                    userRepository.update(user);
                    if (await userRepository.SaveAllAsync())
                    {
                        return Ok();
                    }
                }
                else return BadRequest("Could not delete photo");
            }
            else
            {
                user.Photos.Remove(photo);
                userRepository.update(user);
                if (await userRepository.SaveAllAsync())
                {
                    return Ok();
                }
            }
            return BadRequest("Could not delete photo");
        }
    }

}