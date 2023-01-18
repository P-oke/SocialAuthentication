using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SocialAuthentication.Entities;

namespace SocialAuthentication.Mapping
{
    public class IdentityMap 
    { 

        public class UserMap : IEntityTypeConfiguration<User>
        {
            /// <summary>
            /// Configures the entity of type <typeparamref name="TEntity" />.
            /// </summary>
            /// <param name="builder">The builder to be used to configure the entity type.</param>
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.ToTable(nameof(User));
            }
        }

        public class RoleMap : IEntityTypeConfiguration<Role>
        {
            /// <summary>
            /// Configures the entity of type <typeparamref name="TEntity" />.
            /// </summary>
            /// <param name="builder">The builder to be used to configure the entity type.</param>
            public void Configure(EntityTypeBuilder<Role> builder)
            {
                builder.ToTable(nameof(Role));
            }
        }


        /// <summary>
        /// Class RoleMap.
        /// Implements the <see cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RapX.Entities.UserClaim}" />
        /// </summary>
        /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RapX.Entities.UserClaim}" />
        public class UserClaimMap : IEntityTypeConfiguration<UserClaim>
        {
            /// <summary>
            /// Configures the entity of type <typeparamref name="TEntity" />.
            /// </summary>
            /// <param name="builder">The builder to be used to configure the entity type.</param>
            public void Configure(EntityTypeBuilder<UserClaim> builder)
            {
                builder.ToTable(nameof(UserClaim));
            }
        }

        /// <summary>
        /// Class RoleMap.
        /// Implements the <see cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RapX.Entities.RoleClaim}" />
        /// </summary>
        /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RapX.Entities.RoleClaim}" />
        public class RoleClaimMap : IEntityTypeConfiguration<RoleClaim>
        {
            /// <summary>
            /// Configures the entity of type <typeparamref name="TEntity" />.
            /// </summary>
            /// <param name="builder">The builder to be used to configure the entity type.</param>
            public void Configure(EntityTypeBuilder<RoleClaim> builder)
            {
                builder.ToTable(nameof(RoleClaim));

            }

        }

        /// <summary>
        /// Class RoleMap.
        /// Implements the <see cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RapX.Entities.UserLogin}" />
        /// </summary>
        /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RapX.Entities.UserLogin}" />
        public class UserLoginMap : IEntityTypeConfiguration<UserLogin>
        {
            /// <summary>
            /// Configures the entity of type <typeparamref name="TEntity" />.
            /// </summary>
            /// <param name="builder">The builder to be used to configure the entity type.</param>
            public void Configure(EntityTypeBuilder<UserLogin> builder)
            {
                builder.ToTable(nameof(UserLogin));
            }
        }

        /// <summary>
        /// Class UserRoleMap.
        /// Implements the <see cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RapX.Entities.UserRole}" />
        /// </summary>
        /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RapX.Entities.UserRole}" />
        public class UserRoleMap : IEntityTypeConfiguration<UserRole>
        {
            /// <summary>
            /// Configures the entity of type <typeparamref name="TEntity" />.
            /// </summary>
            /// <param name="builder">The builder to be used to configure the entity type.</param>
            public void Configure(EntityTypeBuilder<UserRole> builder)
            {
                builder.ToTable(nameof(UserRole));

            }


        }

        /// <summary>
        /// Class UserTokenMap.
        /// Implements the <see cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RapX.Entities.UserToken}" />
        /// </summary>
        /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RapX.Entities.UserToken}" />
        public class UserTokenMap : IEntityTypeConfiguration<UserToken>
        {
            /// <summary>
            /// Configures the specified builder.
            /// </summary>
            /// <param name="builder">The builder.</param>
            public void Configure(EntityTypeBuilder<UserToken> builder)
            {
                builder.ToTable(nameof(UserToken));
            }
        }
    }
}
