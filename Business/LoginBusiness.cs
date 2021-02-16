using System;
using System.Collections.Generic;
using System.Security.Claims;
using loginjwt.Configurations;
using loginjwt.Model.ValueObjects;
using loginjwt.Repository;
using loginjwt.Services;
using Microsoft.IdentityModel.JsonWebTokens;

namespace loginjwt.Business
{
    public class LoginBusiness : ILoginBusiness
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private TokenConfiguration _configuration;

        private IUserRepository _repository;
        private readonly ITokenService _tokenService;

        public LoginBusiness(TokenConfiguration configuration, IUserRepository repository, ITokenService tokenService)
        {
            _configuration = configuration;
            _repository = repository;
            _tokenService = tokenService;
        }

        public TokenVO ValidateCredentials(UserVO userCredentials)
        {
            //Gets credentials and Validates in DB
            var user = _repository.ValidateCredentials(userCredentials);
            if (user == null) return null;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            //Sets in the user the new generated values
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configuration.DaysToExpiry);

            //Persist in DataBase
            _repository.RefreshUserInfo(user);
            
            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);
            
            //Setting Token Info, if he reaches this point, the user is authenticated.
            return new TokenVO(
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
            );
        }
    }
}