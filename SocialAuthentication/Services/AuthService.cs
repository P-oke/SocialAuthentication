using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using SocialAuthentication.Context;
using SocialAuthentication.DTOs;
using SocialAuthentication.GoogleAuthentication;
using SocialAuthentication.Interfaces;
using SocialAuthentication.Util;
using System.ComponentModel.DataAnnotations;

namespace SocialAuthentication.Services
{
    public class AuthService : IAuthService 
    {
        private readonly ApplicationDbContext _context;
        private readonly IGoogleAuthService _googleAuthService;

        public AuthService(ApplicationDbContext context, IGoogleAuthService googleAuthService)
        {
            _context = context; 
            _googleAuthService = googleAuthService; 
        }

        /// <summary>
        /// Google SignIn 
        /// </summary>
        /// <param name="model">The model</param>
        /// <returns>Task&lt;ResultModel&lt;TokenModel&gt;&gt;</returns>
        public async Task<BaseResponse<string>> SignInWithGoogle(GoogleSignInVM model) 
        {

            var response = await _googleAuthService.GoogleSignIn(model);

            if (response.Errors.Any())
                return new BaseResponse<string>(response.ResponseMessage, response.Errors);
          
            else
            {

               //TO DO
               //GENERATE JSON WEB TOKEN FORM THE APPLICATION

            }

            return new BaseResponse<string>();
        }
    }
}
