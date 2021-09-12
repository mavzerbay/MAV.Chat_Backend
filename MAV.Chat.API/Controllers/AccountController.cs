using System;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using MAV.Chat.Common.DTOs;
using MAV.Chat.Core.Entities;
using MAV.Chat.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MAV.Chat.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly SignInManager<MavUser> _signInManager;
        private readonly UserManager<MavUser> _userManager;

        public AccountController(ITokenService tokenService, IMapper mapper, SignInManager<MavUser> signInManager, UserManager<MavUser> userManager)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Email))
                return BadRequest("Email Kullanılıyor");

            if (!registerDto.Password.Equals(registerDto.PasswordConfirm))
                return BadRequest("Şifreler Uyuşmuyor");

            var user = _mapper.Map<MavUser>(registerDto);

            user.UserName = registerDto.Email.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDto
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber=user.PhoneNumber,
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            MavUser user = await _userManager.Users
                    .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

            if (user == null) return Unauthorized("Invalid username");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized();

            return new UserDto
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                ProfilePhoto = user.ProfilePhoto
            };
        }

        private async Task<bool> UserExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == email.ToLower());
        }
    }
}