using AutoMapper;
using MAV.Chat.Common.DTOs;
using MAV.Chat.Common.Errors;
using MAV.Chat.Common.Extensions;
using MAV.Chat.Common.Helpers;
using MAV.Chat.Core.Entities;
using MAV.Chat.Core.Interfaces;
using MAV.Chat.Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MAV.Chat.API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [Cached(600)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Pagination<IReadOnlyList<MemberDto>>>> GetUsers([FromQuery] UserSpecParams userSpecParams)
        {
            var spec = new UserSpecification(userSpecParams);

            var specCount = new UserSpecificationForCount(userSpecParams);

            var totalItems = await _unitOfWork.Repository<MavUser>().CountAsync(specCount);
            var users = await _unitOfWork.Repository<MavUser>().ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<MavUser>, IReadOnlyList<MemberDto>>(users);

            return Ok(new Pagination<MemberDto>(userSpecParams.PageIndex, userSpecParams.PageSize, totalItems, data));
        }

        // api/users/username
        [Cached(600)]
        [HttpGet("{username}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var spec = new UserSpecification(username);
            var user = await _unitOfWork.Repository<MavUser>().GetEntityWithSpec(spec);

            if (user == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<MavUser, MemberDto>(user);
        }

        [HttpPut]
        public async Task<ActionResult<MemberDto>> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var spec = new UserSpecification(User.GetUserName());
            var user = await _unitOfWork.Repository<MavUser>().GetEntityWithSpec(spec);

            _mapper.Map(memberUpdateDto, user); 
            if (memberUpdateDto.ProfilePhoto!=null && memberUpdateDto.ProfilePhoto.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    memberUpdateDto.ProfilePhoto.CopyTo(ms);
                    byte[] fileBytes = ms.ToArray();
                    if (fileBytes!=null && fileBytes.Length>0)
                    {
                        user.ProfilePhoto = fileBytes;
                    }
                }
            }

            _unitOfWork.Repository<MavUser>().Update(user);



            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                var returnUser = GetUser(user.UserName);
                return Ok(returnUser);
            }

            return BadRequest("Failed to update user");
        }
    }
}
