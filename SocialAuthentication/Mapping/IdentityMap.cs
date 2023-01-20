using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SocialAuthentication.Entities;

namespace SocialAuthentication.Mapping
{
    public class IdentityMap 
    { 

        public class UserMap : IEntityTypeConfiguration<User>
        {
           
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.ToTable(nameof(User));
            }
        }

        public class RoleMap : IEntityTypeConfiguration<Role>
        {
            
            public void Configure(EntityTypeBuilder<Role> builder)
            {
                builder.ToTable(nameof(Role));
            }
        }


        public class UserClaimMap : IEntityTypeConfiguration<UserClaim>
        {
            
            public void Configure(EntityTypeBuilder<UserClaim> builder)
            {
                builder.ToTable(nameof(UserClaim));
            }
        }

        
        public class RoleClaimMap : IEntityTypeConfiguration<RoleClaim>
        {
           
            public void Configure(EntityTypeBuilder<RoleClaim> builder)
            {
                builder.ToTable(nameof(RoleClaim));

            }

        }

        
        public class UserLoginMap : IEntityTypeConfiguration<UserLogin>
        {
            
            public void Configure(EntityTypeBuilder<UserLogin> builder)
            {
                builder.ToTable(nameof(UserLogin));
            }
        }

        
        public class UserRoleMap : IEntityTypeConfiguration<UserRole>
        {
            
            public void Configure(EntityTypeBuilder<UserRole> builder)
            {
                builder.ToTable(nameof(UserRole));

            }
        }

       
        public class UserTokenMap : IEntityTypeConfiguration<UserToken>
        {
            
            public void Configure(EntityTypeBuilder<UserToken> builder)
            {
                builder.ToTable(nameof(UserToken));
            }
        }
    }
}
