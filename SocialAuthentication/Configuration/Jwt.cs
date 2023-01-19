namespace SocialAuthentication.Configuration
{
    public class Jwt
    {
        public string Secret { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string DurationInMinutes { get; set; }
        public string RefreshTokenExpiration { get; set; }
    }
}
