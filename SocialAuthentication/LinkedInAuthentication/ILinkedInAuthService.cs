using SocialAuthentication.FacebookAuthentication;
using SocialAuthentication.Util;

namespace SocialAuthentication.LinkedInAuthentication
{
    public interface ILinkedInAuthService
    {
        Task<BaseResponse<LinkedInAuthResponse>> GenerateLinkedInAccessToken(string code);
        Task<BaseResponse<LinkedInUserInfo>> GetLinkedInUserInfo(string accessToken); 
    }
}
