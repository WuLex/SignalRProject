using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace AutoLogout.Common
{
    public class JwtHelper
    {// 在适当的地方解析JWT令牌并获取用户名
        public static string GetUsernameFromToken(string jwtToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(jwtToken);

                // 获取JWT中的Claims
                var claims = token.Claims;

                // 查找包含用户名的Claim
                var usernameClaim = claims.FirstOrDefault(c => c.Type == "sub");

                if (usernameClaim != null)
                {
                    // 获取用户名
                    string username = usernameClaim.Value;
                    return username;
                }
                else
                {
                    // 如果找不到用户名的Claim，返回null或者抛出异常
                    // 根据您的需求来决定
                    return null;
                }
            }
            catch (Exception ex)
            {
                // 处理解析异常
                // 根据您的需求来决定如何处理异常
                return null;
            }
        }
    }
}