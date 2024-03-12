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
using CommerceCore.Application.Constant;

namespace CommerceInfra.Service.Authorization
{
    public class JWTAuthService<T, T2> : IAuthService<T, T2>
        where T: Shop
        where T2: Keys
    {
        private readonly JwtConfig keyConfig;
        private ISQLService<T> _sqlService;
        private ISQLService<T2> _addService;
        private string PublicKey = "";

        public JWTAuthService(ISQLService<T> sqlService,
            IOptions<JwtConfig> _config, ISQLService<T2> service)
        {
            _sqlService = sqlService;
            keyConfig = _config.Value;
            _addService = service;
        }

        public async Task<T2> SignUpUser(T item)
        {
            item.Role.Add(RoleConstant.SHOP); await _sqlService.Add(item);
            // Generate both Token
            var AccessToken = GenerateAccessToken(keyConfig, item);
            var RefreshToken = GenerateRefreshToken();
            var result = new Keys()
            {
                AccessToken = AccessToken,
                /*
                 PublicKey = PublicKey,
                 */
                PublicKey = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)), // SImple version
                UserID = item.Id
            };
            result.SetTokenInList(RefreshToken);
            // Add this object into DB
            await _addService.Add((T2)result);
            return (T2)result;
        }

        private static string GenerateRefreshToken()
        {
            byte[] secretKey = RandomNumberGenerator.GetBytes(64);
            return Convert.ToHexString(secretKey);
        }

        private string GenerateAccessToken(JwtConfig config, Shop item)
        {
            /*
             * Ver 1: using Rsa asymmetric algorithm
            RsaSecurityKey rsaSecurityKey = GetRsaKey(out PublicKey);
            var signCredential = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256); 
             */
            // ver 2: using normal Random string version
            var signCrendential2 = new SigningCredentials(new SymmetricSecurityKey(RandomNumberGenerator.GetBytes(64)),
                SecurityAlgorithms.HmacSha256Signature); // private key as random string

            var token = new SecurityTokenDescriptor()
            {
                Issuer = config.Issuer,
                Audience = config.Audience,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, item.Name),
                    new Claim(ClaimTypes.Role, CombineRole(item.Role)),
                    new Claim(ClaimTypes.Email, item.Email)
                }),
                Expires = DateTime.Now.AddMinutes(15),
                // SigningCredentials = signCredential
                SigningCredentials = signCrendential2
            };
            var tokenHandle = new JwtSecurityTokenHandler();
            var preToken = tokenHandle.CreateToken(token);
            var encryptToken = tokenHandle.WriteToken(preToken);
            return encryptToken;
        }

        private RSAToken GenerateSecurityTokens()
        {
            RSAToken result = new();
            using (RSACryptoServiceProvider seed = new RSACryptoServiceProvider())
            {
                result.PrivateKey = seed.ExportRSAPrivateKeyPem();
                result.PublicKey = seed.ExportRSAPublicKeyPem();
            };
            return result;
        }

        private RsaSecurityKey GetRsaKey(out string pK)
        {
            var rsaSeed = RSA.Create();
            RSAToken key = GenerateSecurityTokens();
            // Generatew Private Key
            rsaSeed.ImportFromPem(key.PrivateKey);
            var securityKey = new RsaSecurityKey(rsaSeed);
            pK = key.PublicKey;
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

        private static string CombineRole(IList<string> Role) 
        {
            string result = "";
            for (int i = 0; i< Role.Count; ++i)
            {
                if (i == Role.Count - 1)
                {
                    result += GetRoleBased(Role[i]);
                }
                result += string.Join(',', GetRoleBased(Role[i]));
            }
            return result;
        }

    }
}
