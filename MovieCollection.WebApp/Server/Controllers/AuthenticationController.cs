using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MovieCollection.Domain;
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
        public async Task<ActionResult<User>> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userService.AuthenticateAndGetUserAsync(loginRequest.UserName, loginRequest.Password);

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

        //[HttpPost("create")]
        //public async Task<ActionResult<User>> CreateAsync(LoginRequest loginRequest)
        //{
        //    var user = new User() { UserName = loginRequest.UserName };
        //    await _userService.CreateUserAsync(user, loginRequest.Password);

        //    return await Task.FromResult(Ok());
        //}
    }
}
