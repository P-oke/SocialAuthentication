using System.ComponentModel.DataAnnotations;

namespace SocialAuthentication.DTOs
{
    public class GoogleSignInVM
    {
        /// <summary>
        /// This token is generated from the client side, it is being returned as A jwt from google oauth server. i.e. react, angular, flutter etc.
        /// </summary>
        [Required]
        public string IdToken { get; set; } 
    }
}
