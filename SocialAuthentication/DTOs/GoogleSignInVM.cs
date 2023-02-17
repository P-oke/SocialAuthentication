using System.ComponentModel.DataAnnotations;

namespace SocialAuthentication.DTOs
{
    public class GoogleSignInVM
    {
        /// <summary>
        /// This token being passed here is generated from the client side when a request is made  to 
        /// i.e. react, angular, flutter etc. It is being returned as A jwt from google oauth server. 
        /// </summary>
        [Required]
        public string IdToken { get; set; } 
    }
}
