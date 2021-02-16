using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using loginjwt.Configurations;
using Microsoft.IdentityModel.Tokens;

/// What are claims? Claims is object payload, JSON object that contains informations.
/// There are 3 types of claims in payloads, reserved, private, public.
/// Reserved: Non-mandatory attributes but recomended (issuer, issuer-at, expiration, subject),
/// Private: Used to share informationm between applications.
/// Public: Defines useful information about the token for someone who will use.

namespace loginjwt.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private TokenConfiguration _configuration;

        public TokenService(TokenConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));

            var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_configuration.Minutes),
                signingCredentials: signInCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public string GenerateRefreshToken()
        {
            throw new System.NotImplementedException();
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            throw new System.NotImplementedException();
        }
    }
}