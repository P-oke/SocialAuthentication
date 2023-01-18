using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Extensions;
using SocialAuthentication.Configuration;
using SocialAuthentication.Context;
using SocialAuthentication.DTOs;
using SocialAuthentication.Entities;
using SocialAuthentication.Enum;
using SocialAuthentication.Util;
using System.ComponentModel.DataAnnotations;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace SocialAuthentication.GoogleAuthentication
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly GoogleAuthConfig _googleAuthConfig;

        public GoogleAuthService(UserManager<User> userManager, ApplicationDbContext context, IOptions<GoogleAuthConfig> googleAuthConfig)
        {
            _userManager = userManager;
            _context = context;
            _googleAuthConfig = googleAuthConfig.Value;
        }

        public async Task<BaseResponse<User>> GoogleSignIn(GoogleSignInVM model)
        {

            Payload payload = new();

            try
            {
                payload = await ValidateAsync(model.IdToken, new ValidationSettings
                {
                    Audience = new[] { _googleAuthConfig.ClientId }
                });

            }
            catch (Exception ex)
            {
                return new BaseResponse<User>(ex.Message, new List<string> { ex.Message });
            }

           var user = await CreateUserFromSocialLogin(payload, LoginProvider.Google);

            if (user is not null)
                return new BaseResponse<User>(user);

            else
                return new BaseResponse<User>(null, new List<string> { "Unable to link a Local User to a Provider" });
        }

        private async Task<User> CreateUserFromSocialLogin(Payload payload, LoginProvider loginProvider)  
        {
           
            var user = await _userManager.FindByLoginAsync(loginProvider.GetDisplayName(), payload.Subject);

            if (user is not null)
                return user; //USER ALREADY EXISTS.

            user = await _userManager.FindByEmailAsync(payload.Email);

            if (user is null)
            {
                user = new User
                {
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    Email = payload.Email,
                    UserName = payload.Email,
                    ProfilePicture = payload.Picture
                };

                await _userManager.CreateAsync(user);

                //EMAIL IS CONFIRMED BECAUSE IT IS COMING FROM A SECURED AUTHENTICATION PROVIDER
                user.EmailConfirmed = true;

                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();      
            }

            UserLoginInfo userLoginInfo = null;
            switch (loginProvider)
            {
                case LoginProvider.Google:
                    {
                        userLoginInfo = new UserLoginInfo(loginProvider.GetDisplayName(), payload.Subject, loginProvider.GetDisplayName().ToUpper());
                    }
                    break;
                case LoginProvider.Facebook:
                    {
                        userLoginInfo = new UserLoginInfo(loginProvider.GetDisplayName(), payload.Subject, loginProvider.GetDisplayName().ToUpper());
                    }
                    break;
                default:
                    break;
            }

            var result = await _userManager.AddLoginAsync(user, userLoginInfo);

            if (result.Succeeded)
                return user;

            else
                return null;
        }
    }
}
