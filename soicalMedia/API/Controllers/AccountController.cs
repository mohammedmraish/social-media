using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using social_media.Data;
using social_media.Entity;
using soicalMedia.DTOs;
using soicalMedia.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace soicalMedia.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }



        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
                if (await UserExists(registerDto.Username))
                    return BadRequest("userName is taken");

           

            var user = mapper.Map<RegisterDto, AppUser>(registerDto);

            user.UserName = registerDto.Username;


            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            //add role
            var roleResult = await userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            return new UserDto
            {
                Username = user.UserName,
                Token = await tokenService.CreateToken(user),
                KnownAs= user.KnownAs,
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(u => u.UserName == loginDto.Username.ToLower());

            if (user == null)
                return Unauthorized("Invalid userName");

            var result = await signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return Unauthorized();


            return new UserDto
            {
                Username = user.UserName,
                Token =await tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,

            };
        }


        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(u => u.UserName == username.ToLower());
        }
    }
}
