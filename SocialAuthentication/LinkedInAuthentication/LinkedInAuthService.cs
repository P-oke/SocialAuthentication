using log4net;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SocialAuthentication.Configuration;
using SocialAuthentication.Util;
using System.Net;
using System.Net.Http.Headers;

namespace SocialAuthentication.LinkedInAuthentication
{
    /// <summary>
    /// Class LinkedIn Auth Service.
    /// Implements the <see cref="SocialAuthentication.LinkedInAuthentication.ILinkedInAuthService" />
    /// </summary>
    /// <seealso cref="SocialAuthentication.LinkedInAuthentication.ILinkedInAuthService" />
    public class LinkedInAuthService : ILinkedInAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly LinkedInAuthConfig _linkedInAuthConfig;
        private readonly ILog _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedInAuthService"/> class.
        /// </summary>
        /// <param name="linkedInAuthConfig"></param>
        /// <param name="httpClientFactory"></param>
        public LinkedInAuthService
        (
            IOptions<LinkedInAuthConfig> linkedInAuthConfig,
            IHttpClientFactory httpClientFactory
        ) 
        {
            _linkedInAuthConfig = linkedInAuthConfig.Value;
            _httpClient = httpClientFactory.CreateClient();
            _logger = LogManager.GetLogger(typeof(LinkedInAuthService));
        }

        /// <summary>
        /// Generate LinkedIn Access Token
        /// </summary>
        /// <param name="code">the authorization code</param>
        /// <returns>Task&lt;BaseResponse&lt;LinkedInAuthResponse&gt;&gt;</returns>
        public async Task<BaseResponse<LinkedInAuthResponse>> GenerateLinkedInAccessToken(string code)
        {
            try
            {
                var formData = new Dictionary<string, string>
                {
                   
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", _linkedInAuthConfig.RedirectUri },
                    { "client_id", _linkedInAuthConfig.ClientId },
                    { "client_secret", _linkedInAuthConfig.ClientSecret },
                    { "code", code }
                };

                var content = new FormUrlEncodedContent(formData);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var response = await _httpClient.PostAsync($"{_linkedInAuthConfig.OauthBaseUrl}/v2/accessToken", content);

                var responseAsString = await response.Content.ReadAsStringAsync();

                _logger.InfoFormat("LinkedIn Auth Response {0}", @responseAsString);

                if (response.IsSuccessStatusCode)
                {
                    var linkedInAuthResponse = JsonConvert.DeserializeObject<LinkedInAuthResponse>(responseAsString); 
                    return new BaseResponse<LinkedInAuthResponse>(linkedInAuthResponse);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorResponse = JsonConvert.DeserializeObject<LinkedInErrorResponse>(responseAsString); 
                    return new BaseResponse<LinkedInAuthResponse>(errorResponse.ErrorDescription );
                }
      
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace, ex);
            }

            return new BaseResponse<LinkedInAuthResponse>(null, "Failed to get response");
         
        }

        /// <summary>
        /// Get LinkedIn User Info
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns>Task&lt;BaseResponse&lt;LinkedInUserInfo&gt;&gt;</returns>
        public async Task<BaseResponse<LinkedInUserInfo>> GetLinkedInUserInfo(string accessToken)   
        {
            try
            {

                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {accessToken}");

                var response = await _httpClient.GetAsync($"{_linkedInAuthConfig.BaseUrl}/v2/userinfo");

                var responseAsString = await response.Content.ReadAsStringAsync();

                _logger.InfoFormat("LinkedIn User Info Response {0}", @responseAsString);

                if (response.IsSuccessStatusCode)
                {
                    var linkedInAuthResponse = JsonConvert.DeserializeObject<LinkedInUserInfo>(responseAsString);
                    return new BaseResponse<LinkedInUserInfo>(linkedInAuthResponse);
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace, ex);
            }

            return new BaseResponse<LinkedInUserInfo>(null, "Failed to get response");

        }
    }
}
