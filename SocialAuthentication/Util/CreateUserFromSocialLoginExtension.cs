using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using SocialAuthentication.Context;
using SocialAuthentication.DTOs;
using SocialAuthentication.Entities;
using SocialAuthentication.Enum;

namespace SocialAuthentication.Util
{
    public static class CreateUserFromSocialLoginExtension
    {
        
        public static async Task<User> CreateUserFromSocialLogin(this UserManager<User> userManager, ApplicationDbContext context, CreateUserFromSocialLogin model, LoginProvider loginProvider)
        {
            var user = await userManager.FindByLoginAsync(loginProvider.GetDisplayName(), model.LoginProviderSubject);

            if (user is not null)
                return user; //USER ALREADY EXISTS.

            user = await userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email,
                    ProfilePicture = model.ProfilePicture
                };

                await userManager.CreateAsync(user);

                //EMAIL IS CONFIRMED BECAUSE IT IS COMING FROM A SECURED AUTHENTICATION PROVIDER
                user.EmailConfirmed = true;

                await userManager.UpdateAsync(user);
                await context.SaveChangesAsync();
            }

            UserLoginInfo userLoginInfo = null;
            switch (loginProvider)
            {
                case LoginProvider.Google:
                    {
                        userLoginInfo = new UserLoginInfo(loginProvider.GetDisplayName(), model.LoginProviderSubject, loginProvider.GetDisplayName().ToUpper());
                    }
                    break;
                case LoginProvider.Facebook:
                    {
                        userLoginInfo = new UserLoginInfo(loginProvider.GetDisplayName(), model.LoginProviderSubject, loginProvider.GetDisplayName().ToUpper());
                    }
                    break;
                default:
                    break;
            }

            var result = await userManager.AddLoginAsync(user, userLoginInfo);

            if (result.Succeeded)
                return user;

            else
                return null;
        }
    }
}
