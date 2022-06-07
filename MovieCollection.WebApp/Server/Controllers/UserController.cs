using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MovieCollection.Domain;
using MovieCollection.Domain.Exceptions;
using MovieCollection.Services;
using MovieCollection.WebApp.Shared.Requests;
using System.Net;
using System.Security.Claims;

namespace MovieCollection.WebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginAsync(LoginRequest request)
        {
            var user = await _userService.AuthenticateAndGetUserAsync(request.UserName, request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            var claim = new Claim(ClaimTypes.Name, user.UserName);
            var claimsIdentity = new ClaimsIdentity(new[] { claim }, "serverAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
            
            return Ok(user);
        }

        [HttpPost("logout")]
        public async Task<ActionResult> LogoutAsync()
        {
            if (User.Identity != null
                && User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                return Ok();
            }

            return Unauthorized();
        }

        [HttpGet("current")]
        public async Task<ActionResult<User>> GetCurrentUserAsync()
        {
            if (User.Identity != null 
                && User.Identity.IsAuthenticated)
            {
                return await Task.FromResult(
                    Ok(new User() { UserName = User.FindFirstValue(ClaimTypes.Name)}));
            }

            return await Task.FromResult(Unauthorized("Not logged in"));
        }

        [HttpPost("create")]
        public async Task<ActionResult<User>> CreateAsync(CreateUserRequest request)
        {
            var user = new User() { UserName = request.UserName, Email = request.Email };

            try
            {
                await _userService.CreateUserAsync(user, request.Password);
            }
            catch (UserAlreadyExistsException)
            {
                return Conflict("UserName already exists");
            }

            return Ok(user);
        }
    }
}
