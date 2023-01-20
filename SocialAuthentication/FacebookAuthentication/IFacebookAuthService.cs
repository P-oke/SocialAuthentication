using SocialAuthentication.Util;

namespace SocialAuthentication.FacebookAuthentication
{
    public interface IFacebookAuthService
    {
        Task<BaseResponse<FacebookTokenValidationResponse>> ValidateFacebookToken(string accessToken);
        Task<BaseResponse<FacebookUserInfoResponse>> GetFacebookUserInformation(string accessToken);
    } 
}
