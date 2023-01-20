using log4net;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SocialAuthentication.Configuration;
using SocialAuthentication.Util;
using System.Net.Http;

namespace SocialAuthentication.FacebookAuthentication
{
    public class FacebookAuthService : IFacebookAuthService
    {
     
        private readonly HttpClient _httpClient;
        private readonly FacebookAuthConfig _facebookAuthConfig;
        private readonly ILog _logger;

        public FacebookAuthService(
            IHttpClientFactory httpClientFactory, 
            IConfiguration configuration, 
            IOptions<FacebookAuthConfig> facebookAuthConfig)
        {
            _httpClient = httpClientFactory.CreateClient("Facebook");
            _facebookAuthConfig = facebookAuthConfig.Value;
            _logger = LogManager.GetLogger(typeof(FacebookAuthService));
        }


        public async Task<BaseResponse<FacebookTokenValidationResponse>> ValidateFacebookToken(string accessToken)
        {
            try
            {
                string TokenValidationUrl = _facebookAuthConfig.TokenValidationUrl;
                var url = string.Format(TokenValidationUrl, accessToken, _facebookAuthConfig.AppId, _facebookAuthConfig.AppSecret);
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseAsString = await response.Content.ReadAsStringAsync();

                    var tokenValidationResponse = JsonConvert.DeserializeObject<FacebookTokenValidationResponse>(responseAsString);
                    return new BaseResponse<FacebookTokenValidationResponse>(tokenValidationResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace, ex);
            }

            return new BaseResponse<FacebookTokenValidationResponse>(null, "Failed to get response");

        }

        public async Task<BaseResponse<FacebookUserInfoResponse>> GetFacebookUserInformation(string accessToken) 
        {
            try
            {
                string userInfoUrl = _facebookAuthConfig.UserInfoUrl;
                string url = string.Format(userInfoUrl, accessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseAsString = await response.Content.ReadAsStringAsync();
                    var userInfoResponse = JsonConvert.DeserializeObject<FacebookUserInfoResponse>(responseAsString);
                    return new BaseResponse<FacebookUserInfoResponse>(userInfoResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace, ex);
            }

            return new BaseResponse<FacebookUserInfoResponse>(null, "Failed to get response");

        }

    }
}
