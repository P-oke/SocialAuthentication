using log4net;
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
    /// <summary>
    /// Class Facebook Auth Service.
    /// Implements the <see cref="SocialAuthentication.GoogleAuthentication.IGoogleAuthService" />
    /// </summary>
    /// <seealso cref="SocialAuthentication.GoogleAuthentication.IGoogleAuthService" />
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly GoogleAuthConfig _googleAuthConfig;
        private readonly ILog _logger;

        public GoogleAuthService(
            UserManager<User> userManager, 
            ApplicationDbContext context, 
            IOptions<GoogleAuthConfig> googleAuthConfig
            )
        {
            _userManager = userManager;
            _context = context;
            _googleAuthConfig = googleAuthConfig.Value;
            _logger = LogManager.GetLogger(typeof(GoogleAuthService));
        }

        /// <summary>
        /// Google SignIn
        /// </summary>
        /// <param name="model">the model</param>
        /// <returns>Task&lt;BaseResponse&lt;User&gt;&gt;</returns>
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
                _logger.Error(ex.Message, ex);
                return new BaseResponse<User>(null, new List<string> { "Failed to get a response" });
            }

            var userToBeCreated = new CreateUserFromSocialLogin
            {
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Email = payload.Email,
                ProfilePicture = payload.Picture,
                LoginProviderSubject = payload.Subject,
            };
            
           var user = await _userManager.CreateUserFromSocialLogin(_context, userToBeCreated, LoginProvider.Google);

            if (user is not null)
                return new BaseResponse<User>(user);

            else
                return new BaseResponse<User>(null, new List<string> { "Unable to link a Local User to a Provider" });
        }

        
    }
}
