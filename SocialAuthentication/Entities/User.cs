using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace SocialAuthentication.Entities 
{
    [Table(nameof(User))]
    public class User : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{LastName} {FirstName}";
            }
        }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }

    public class Role : IdentityRole<long>
    {
    }

    public class UserClaim : IdentityUserClaim<long> { }

    public class UserRole : IdentityUserRole<long> { }

    public class UserLogin : IdentityUserLogin<long> { }

    public class RoleClaim : IdentityRoleClaim<long> { }

    public class UserToken : IdentityUserToken<long> { }
}