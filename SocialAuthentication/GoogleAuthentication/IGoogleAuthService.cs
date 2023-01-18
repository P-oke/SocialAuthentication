using SocialAuthentication.DTOs;
using SocialAuthentication.Entities;
using SocialAuthentication.Util;

namespace SocialAuthentication.GoogleAuthentication
{
    public interface IGoogleAuthService
    {
        Task<BaseResponse<User>> GoogleSignIn(GoogleSignInVM model);
    }
}
