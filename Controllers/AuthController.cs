using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.DTOs;
using TodoAPI.Services;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(TokenServices tokenService, UserManager<IdentityUser<int>> userManager) : ControllerBase
    {
        readonly TokenServices tokenSrv = tokenService;
        readonly UserManager<IdentityUser<int>> userMng = userManager;

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            //اتاكد لو اليوزر موجود 
            var user = await userMng.FindByEmailAsync(dto.Email);
            if (user == null || !await userMng.CheckPasswordAsync(user, dto.Password))
                return Unauthorized(new { message = "Invalid email or password" });
            //استخرج الrole
            var roles = await userMng.GetRolesAsync(user);
            //انشئ التوكن
            var accessToken = await tokenSrv.GenerateAccessToken(user);
            //ارجع اليوزر مع التوكن
            return Ok(
               new AuthResponse
               {
                   AccessToken = accessToken,
                   User = new UserSimple
                   {
                       Id = user.Id,
                       Email = user.Email ?? "",
                       Role = roles.FirstOrDefault() ?? "",
                   }
               }
            );
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            //اشوف الاول لو اليوزر موجود
            var existingUser = await userMng.FindByEmailAsync(dto.Email);
            if (existingUser != null) return BadRequest(new { message = "Email already exists!" });
            //انشئ instance
            var user = new IdentityUser<int>
            {
                Email = dto.Email,
                UserName = dto.Email,
            };
            //انشئ يوزر من ال instance
            var res = await userMng.CreateAsync(user, dto.Password);
            if (!res.Succeeded) return BadRequest(new { errors = res.Errors });
            //اضيفله role
            await userMng.AddToRoleAsync(user, dto.Role);
            //انشئ token
            var accessToken = await tokenSrv.GenerateAccessToken(user);
            //ارجع بيانات اليوزر مع التوكن
            return Ok(
                new AuthResponse
                {
                    AccessToken=accessToken,
                    User=new UserSimple
                    {
                        Id=user.Id,
                        Email=user.Email,
                        Role=dto.Role,
                    }
                }
            );
        }
    }
}
