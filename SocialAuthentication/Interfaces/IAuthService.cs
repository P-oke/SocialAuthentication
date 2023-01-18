using SocialAuthentication.DTOs;
using SocialAuthentication.Util;

namespace SocialAuthentication.Interfaces
{
    public interface IAuthService 
    {
        Task<BaseResponse<string>> SignInWithGoogle(GoogleSignInVM model);
    }
}
