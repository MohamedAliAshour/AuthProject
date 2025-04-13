using AuthProject.Dtos;
using AuthProject.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using System.Security.Claims;

namespace AuthProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAuth _auth;
        public AuthController(UserManager<IdentityUser> userManager,IAuth auth)
        {
            _userManager = userManager;
            _auth = auth;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDtos model)
        {
            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (model.roles != null && model.roles.Any())
                {
                    var roleresualt = await _userManager.AddToRolesAsync(user, model.roles);


                    if (roleresualt.Succeeded)
                    {
                        return Ok(new { message = "User registered successfully" });
                    }

                }
                  
            }
                return BadRequest(new { message = "User registration failed", errors = result.Errors });
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDtos model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = await _userManager.CheckPasswordAsync(user, model.Password);
                if (result)
                {
                    var role = await _userManager.GetRolesAsync(user);

                    if (role != null)
                    {
                        // Create claims, including the NameIdentifier (user ID)
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id),  // Add the user ID as the NameIdentifier claim
                            new Claim(ClaimTypes.Email, user.Email),        // Add the user's email
                            new Claim(ClaimTypes.Role, string.Join(",", role))  // Add the roles as claims
                        };

                        // Create the JWT token using the claims
                        var jwttoken = _auth.CreateToken(user, claims); // Pass claims instead of just the user and role list

                        var response = new LoginTokenDtos
                        {
                            JwtToken = jwttoken
                        };

                        return Ok(response);
                    }

                    return BadRequest(new { message = "User has no roles" });
                }
            }

            return BadRequest(new { message = "Invalid username or password" });
        }



    }
}
