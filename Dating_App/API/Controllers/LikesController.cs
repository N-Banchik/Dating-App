using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserRepository _userRepository;

        public LikesController(ILikeRepository likeRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _likeRepository = likeRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            int userid = User.GetUserId();
            int targetUserid = await _userRepository.GetUserIdByUsernameAsync(username);

            if (userid == targetUserid)
            {
                return BadRequest("You cannot like yourself");
            }
            if (targetUserid == 0)
            {
                return BadRequest("User does not exist");
            }
            if (await _likeRepository.GetLike(userid, targetUserid) != null)
            {
                return BadRequest("You already liked this user");
            }



            UserLike userLike = new UserLike
            {
                SourceUserId = userid,
                TargetUserId = targetUserid
            };

            _likeRepository.Add(userLike);

            if (await _likeRepository.SaveAllAsync())
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to like user");
            }
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikesAsync([FromQuery] LikeParams likeParams)
        {

            likeParams.userId= User.GetUserId();
            var likes = await _likeRepository.GetUserlikesAsync(likeParams);
            Response.AddPaginationHeader(likes.CurrentPage, likes.PageSize, likes.TotalCount, likes.TotalPages);

            return Ok(likes);
        }

    }
}