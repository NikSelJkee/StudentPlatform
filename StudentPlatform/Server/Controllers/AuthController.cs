using AutoMapper;
using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace StudentPlatform.Server.Controllers
{
    [ApiController]
    [Route("/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpPost("{groupId}/register")]
        public async Task<IActionResult> Register(int groupId, [FromBody]UserForCreationDto user)
        {
            var userEntity = _mapper.Map<User>(user);
            userEntity.GroupId = groupId;

            var result = await _userManager.CreateAsync(userEntity, user.Password);

            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()?.Description);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserDto user)
        {
            var userEntity = await _userManager.FindByNameAsync(user.UserName);

            if (userEntity == null) return BadRequest("User doesn't exists in the database");

            var result = await _signInManager.PasswordSignInAsync(userEntity, user.Password, false, false);

            if (!result.Succeeded) return BadRequest("Invalid password");

            return Ok();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
