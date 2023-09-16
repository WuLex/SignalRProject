using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SignalRChat.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SignalRChat.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // 用户登录
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserCredentials credentials)
        {
            // 在实际应用中，验证用户身份，并生成并返回身份令牌
            // 以下是一个简单的模拟登录，实际情况可能涉及数据库查询和身份验证
            if (credentials.Username == "wu" && credentials.Password == "12345")
            {
                //var token = "your_generated_token"; // 实际应用中应生成令牌
                var token = GenerateJwtToken(credentials.Username);
                return Content(token);
                //return Ok(new { token });
            }
            else
            {
                return Unauthorized(); // 登录失败
            }
        }

        private string GenerateJwtToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]));

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // 用户登出（在实际应用中可能需要清除令牌）
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // 在实际应用中，撤销令牌或会话
            return Ok();
        }
    }
}