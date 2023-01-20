namespace SocialAuthentication.DTOs
{
    public class CreateUserFromSocialLogin
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public string LoginProviderSubject { get; set; }
    }
}
