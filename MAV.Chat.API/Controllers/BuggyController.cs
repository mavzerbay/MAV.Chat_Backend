using MAV.Chat.Common.Errors;
using MAV.Chat.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MAV.Chat.API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly MavDbContext _context;

        public BuggyController(MavDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet("testauth")]
        [Authorize]
        public ActionResult<string> GetSecretText()
        {
            return "gizli bilgi koçum";
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var thing = _context.Users.Find(42);

            if (thing == null) return NotFound(new ApiResponse(404));

            return Ok();
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var thing = _context.Users.Find(42);

            var thingToReturn = thing.ToString();

            return Ok();
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetNotFoundRequest(int id)
        {
            return BadRequest();
        }
    }
}