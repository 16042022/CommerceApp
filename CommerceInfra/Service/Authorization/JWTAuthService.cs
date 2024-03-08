using CommerceCore.Application.Interface;
using CommerceCore.Application.Models;
using CommerceCore.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;

namespace CommerceInfra.Service.Authorization
{
    public class JWTAuthService : IAuthService<ShopExample, KeyTokenStore>
    {
        private readonly string Path;
        private readonly JwtConfig keyConfig;
        private ISQLService<ShopExample> _sqlService;

        public JWTAuthService(ISQLService<ShopExample> sqlService,
            IOptions<JwtConfig> _config)
        {
            Path = Environment.GetEnvironmentVariable("KeyPath")!;
            _sqlService = sqlService;
            keyConfig = _config.Value;
        }

        public async Task<KeyTokenStore> SignUpUser(ShopExample item)
        {
            item.SetRoleInList("SHOP"); await _sqlService.Add(item);
            // Generate both Token
            var AccessToken = GenerateAccessToken(keyConfig, item);
            var RefreshToken = GenerateRefreshToken();
            var result = new KeyTokenStore()
            {
                AccessToken = AccessToken,
                UserID = item.Id
            };
            result.SetTokenInList(RefreshToken);
            return result;
        }

        private static string GenerateRefreshToken()
        {
            byte[] secretKey = RandomNumberGenerator.GetBytes(64);
            return Convert.ToHexString(secretKey);
        }

        private string GenerateAccessToken(JwtConfig config, ShopExample item)
        {
            RsaSecurityKey rsaSecurityKey = GetRsaKey();
            var signCredential = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256);

            var token = new SecurityTokenDescriptor()
            {
                Issuer = config.Issuer,
                Audience = config.Audience,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, item.Name),
                    new Claim(ClaimTypes.Role, GetRoleBased(item.GetRoleInList("SHOP"))),
                    new Claim(ClaimTypes.Email, item.Email)
                }),
                Expires = DateTime.Now.AddMinutes(10),
                SigningCredentials = signCredential
            };
            var tokenHandle = new JwtSecurityTokenHandler();
            var preToken = tokenHandle.CreateToken(token);
            var encryptToken = tokenHandle.WriteToken(preToken);
            return encryptToken;
        }

        private RsaSecurityKey GetRsaKey()
        {
            var rsaSeed = RSA.Create();
            string xmlKey = File.ReadAllText(keyConfig.KeyPath);
            rsaSeed.FromXmlString(xmlKey);
            var securityKey = new RsaSecurityKey(rsaSeed);
            return securityKey;
        }

        private static string GetRoleBased(string v) => v switch
        {
            "0x01" => "ADMIN",
            "0x02" => "WRIITER",
            "0x04" => "READER",
            "0x08" => "SHOP",
            _ => throw new ArgumentOutOfRangeException(nameof(v), $"Not expected role value: {v}"),
        };

    }
}
