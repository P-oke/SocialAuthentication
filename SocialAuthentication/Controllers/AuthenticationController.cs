using Microsoft.AspNetCore.Mvc;
using SocialAuthentication.DTOs;
using SocialAuthentication.Entities;
using SocialAuthentication.Interfaces;
using SocialAuthentication.Util;

namespace SocialAuthentication.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : BaseController 

    {
        private readonly IAuthService _authService;
        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// SIGN IN WITH GOOGLE
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        public async Task<IActionResult> GoogleSignIn(GoogleSignInVM model)
        {
            try
            {
                return ReturnResponse(await _authService.SignInWithGoogle(model));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        /// <summary>
        /// SIGN IN WITH FACEBOOK
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        public async Task<IActionResult> FacebookSignIn(FacebookSignInVM model) 
        {
            try
            {
                return ReturnResponse(await _authService.SignInWithFacebook(model));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}