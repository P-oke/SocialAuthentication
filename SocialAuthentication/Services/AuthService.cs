using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialAuthentication.Configuration;
using SocialAuthentication.Context;
using SocialAuthentication.DTOs;
using SocialAuthentication.Entities;
using SocialAuthentication.GoogleAuthentication;
using SocialAuthentication.Interfaces;
using SocialAuthentication.Util;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialAuthentication.Services
{
    public class AuthService : IAuthService 
    {
        private readonly ApplicationDbContext _context;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly Jwt _jwt; 

        public AuthService(ApplicationDbContext context, IGoogleAuthService googleAuthService, IOptions<Jwt>jwt)
        {
            _context = context; 
            _googleAuthService = googleAuthService;
            _jwt = jwt.Value;
        }

        /// <summary>
        /// Google SignIn 
        /// </summary>
        /// <param name="model">The model</param>
        /// <returns>Task&lt;ResultModel&lt;JwtResponseVM&gt;&gt;</returns>
        public async Task<BaseResponse<JwtResponseVM>> SignInWithGoogle(GoogleSignInVM model) 
        {

            var response = await _googleAuthService.GoogleSignIn(model);

            if (response.Errors.Any())
                return new BaseResponse<JwtResponseVM>(response.ResponseMessage, response.Errors);
                 
            var jwtResponse = CreateJwtToken(response.Data);

            var data = new JwtResponseVM
            {
                Token = jwtResponse,
            };
               
            return new BaseResponse<JwtResponseVM>(data);
        }

        private string CreateJwtToken(User user)
        { 

            var key = Encoding.ASCII.GetBytes(_jwt.Secret);

            var userClaims = BuildUserClaims(user);

            var signKey = new SymmetricSecurityKey(key);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.ValidIssuer,
                notBefore: DateTime.UtcNow,
                audience: _jwt.ValidAudience,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_jwt.DurationInMinutes)),
                claims: userClaims,
                signingCredentials: new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private List<Claim> BuildUserClaims(User user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.GivenName, user.FirstName),
                new Claim(JwtClaimTypes.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            return userClaims;
        }

        
    }
}
