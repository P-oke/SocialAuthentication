using SocialAuthentication.DTOs;
using SocialAuthentication.Util;

namespace SocialAuthentication.Interfaces
{
    public interface IAuthService 
    {
        Task<BaseResponse<JwtResponseVM>> SignInWithGoogle(GoogleSignInVM model);
    }
}
