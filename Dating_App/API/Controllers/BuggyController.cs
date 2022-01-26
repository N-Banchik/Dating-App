using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext __context;
        public BuggyController(DataContext _context)
        {
            __context = _context;

        }

        // 401 unauthorized
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "Secret string";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var user = __context.Users.Find(-1);
            if (user == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetSErverError()
        {
            var user = __context.Users.Find(-1);
            var usertostring = user.ToString();
            return "string";
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("NOPE!");
        }


    }
}