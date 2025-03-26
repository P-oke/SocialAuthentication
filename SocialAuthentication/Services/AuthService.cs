using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using SocialAuthentication.Configuration;
using SocialAuthentication.Context;
using SocialAuthentication.DTOs;
using SocialAuthentication.Entities;
using SocialAuthentication.Enum;
using SocialAuthentication.FacebookAuthentication;
using SocialAuthentication.GoogleAuthentication;
using SocialAuthentication.Interfaces;
using SocialAuthentication.LinkedInAuthentication;
using SocialAuthentication.Util;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Google.Apis.Requests.BatchRequest;

namespace SocialAuthentication.Services
{
    /// <summary>
    /// Class Auth Service.
    /// Implements the <see cref="SocialAuthentication.Interfaces.IAuthService" />
    /// </summary>
    /// <seealso cref="SocialAuthentication.Interfaces.IAuthService" />
    public class AuthService : IAuthService 
    {
        private readonly ApplicationDbContext _context;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly ILinkedInAuthService _linkedInAuthService;
        private readonly UserManager<User> _userManager;
        private readonly Jwt _jwt; 

        public AuthService(
            ApplicationDbContext context, 
            IGoogleAuthService googleAuthService, 
            IFacebookAuthService  facebookAuthService, 
            ILinkedInAuthService linkedInAuthService,
            UserManager<User> userManager,
            IOptions<Jwt>jwt)
        {
            _context = context; 
            _googleAuthService = googleAuthService;
            _facebookAuthService = facebookAuthService;
            _linkedInAuthService = linkedInAuthService;
            _userManager = userManager;
            _jwt = jwt.Value;
        }

        /// <summary>
        /// Google SignIn 
        /// </summary>
        /// <param name="model">the view model</param>
        /// <returns>Task&lt;BaseResponse&lt;JwtResponseVM&gt;&gt;</returns>
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

        /// <summary>
        /// Facebook SignIn
        /// </summary>
        /// <param name="model">the view model</param>
        /// <returns>Task&lt;BaseResponse&lt;JwtResponseVM&gt;&gt;</returns>
        public async Task<BaseResponse<JwtResponseVM>> SignInWithFacebook(FacebookSignInVM model)
        {
            var validatedFbToken = await _facebookAuthService.ValidateFacebookToken(model.AccessToken);

            if(validatedFbToken.Errors.Any())
                return new BaseResponse<JwtResponseVM>(validatedFbToken.ResponseMessage, validatedFbToken.Errors);

            var userInfo = await _facebookAuthService.GetFacebookUserInformation(model.AccessToken);

            if (userInfo.Errors.Any())
                return new BaseResponse<JwtResponseVM>(null, userInfo.Errors);

            var userToBeCreated = new CreateUserFromSocialLogin
            {
                FirstName = userInfo.Data.FirstName,
                LastName = userInfo.Data.LastName,
                Email = userInfo.Data.Email,
                ProfilePicture = userInfo.Data.Picture.Data.Url.AbsoluteUri,
                LoginProviderSubject = userInfo.Data.Id,
            };

            var user = await _userManager.CreateUserFromSocialLogin(_context, userToBeCreated, LoginProvider.Facebook);

            if (user is not null)
            {
                var jwtResponse = CreateJwtToken(user);

                var data = new JwtResponseVM
                {
                    Token = jwtResponse,
                };

                return new BaseResponse<JwtResponseVM>(data);
            }

            return new BaseResponse<JwtResponseVM>(null, userInfo.Errors);

        }

        /// <summary>
        /// LinkedIn SignIn
        /// </summary>
        /// <param name="model">the view model</param>
        /// <returns>Task&lt;BaseResponse&lt;JwtResponseVM&gt;&gt;</returns>
        public async Task<BaseResponse<JwtResponseVM>> SignInWithLinkedIn(LinkedInSignInVM model) 
        {
            var linkedInAccessToken = await _linkedInAuthService.GenerateLinkedInAccessToken(model.AuthorizationCode); 

            if (linkedInAccessToken.Errors.Any())
                return new BaseResponse<JwtResponseVM>(linkedInAccessToken.ResponseMessage, linkedInAccessToken.Errors);

            var userInfo = await _linkedInAuthService.GetLinkedInUserInfo(linkedInAccessToken.Data.AccessToken);

            if (userInfo.Errors.Any())
                return new BaseResponse<JwtResponseVM>(null, userInfo.Errors);

            var userToBeCreated = new CreateUserFromSocialLogin
            {
                FirstName = userInfo.Data.GivenName,
                LastName = userInfo.Data.FamilyName,
                Email = userInfo.Data.Email,
                ProfilePicture = userInfo.Data.Picture,
                LoginProviderSubject = userInfo.Data.Sub,
            };

            var user = await _userManager.CreateUserFromSocialLogin(_context, userToBeCreated, LoginProvider.LinkedIn);

            if (user is not null)
            {
                var jwtResponse = CreateJwtToken(user);

                var data = new JwtResponseVM
                {
                    Token = jwtResponse,
                };

                return new BaseResponse<JwtResponseVM>(data);
            }

            return new BaseResponse<JwtResponseVM>(null, userInfo.Errors);

        }

        /// <summary>
        /// Creates JWT Token
        /// </summary>
        /// <param name="user">the user</param>
        /// <returns>System.String</returns>
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

        /// <summary>
        /// Builds the UserClaims
        /// </summary>
        /// <param name="user">the User</param>
        /// <returns>List&lt;System.Security.Claims&gt;</returns>
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
