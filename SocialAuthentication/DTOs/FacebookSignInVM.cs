using System.ComponentModel.DataAnnotations;

namespace SocialAuthentication.DTOs
{
    public class FacebookSignInVM
    {
        /// <summary>
        /// This token is generated from the client side. i.e. react, angular, flutter etc.
        /// </summary>
        [Required]
        public string AccessToken { get; set; }
    }
}
