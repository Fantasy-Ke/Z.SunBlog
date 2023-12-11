using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Z.Fantasy.Core.UserSession;
using Z.Fantasy.Core;
using Z.Fantasy.Core.Authorization.Dtos;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.Authorization;

public class JwtTokenProvider : IJwtTokenProvider
{
    private readonly JwtSettings _jwtConfig;
    public JwtTokenProvider(IConfiguration configuration)
    {
        _jwtConfig = configuration.GetSection("App:JWtSetting").Get<JwtSettings>() ?? throw new ArgumentException("请先检查appsetting中JWT配置");
    }
    /// <summary>
    /// 生成ZToken
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    public ZFantasyToken GenerateZToken(params Claim[] claims)
    {
        var accessToken = GenerateAccessToken(claims);
        var refreshToken = GenerateRefreshToken();
        return new ZFantasyToken { AccessToken = accessToken, RefreshToken = refreshToken };
    }
    public string GenerateAccessToken(params Claim[] claims)
    {
        claims.ToList().Add(new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddMinutes(_jwtConfig.AccessTokenExpirationMinutes)).ToUnixTimeSeconds()}"));

        //if (user.RoleIds != null && user.RoleIds.Any())
        //{
        //    claims.AddRange(user.RoleIds.Select(p => new Claim(ZClaimTypes.RoleIds, p.ToString())));
        //}
        //if (user.RoleNames != null && user.RoleNames.Any())
        //{
        //    claims.AddRange(user.RoleNames.Select(p => new Claim(ZClaimTypes.Role, p)));
        //}

        // 生成Token的密钥
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));

        // 生成Token的签名证书
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 设置Token的过期时间
        DateTime expires = DateTime.Now.AddMinutes(_jwtConfig.AccessTokenExpirationMinutes);

        // 创建Token
        JwtSecurityToken token = new JwtSecurityToken(
           issuer: _jwtConfig.Issuer,
           audience: _jwtConfig.Audience,
           claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        // 生成Token字符串
        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }
    // 创建刷新token
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            return tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtConfig.Issuer,
                ValidAudience = _jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(_jwtConfig.SecretKey))
            }, out var validatedToken);
        }
        catch (Exception x)
        {
            return null;
        }
    }

    /// <summary>
    /// token解码
    /// </summary>
    /// <param name="jwtToken"></param>
    /// <returns></returns>
    public Claim[] DecodeToken(string jwtToken)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(jwtToken);
        return jwtSecurityToken?.Claims?.ToArray();
    }

}

public interface IJwtTokenProvider : ITransientDependency
{
    ZFantasyToken GenerateZToken(params Claim[] claims);
    ClaimsPrincipal GetPrincipalToken(string token);

    Claim[] DecodeToken(string jwtToken);
}
