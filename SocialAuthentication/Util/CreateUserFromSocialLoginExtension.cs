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
        /// <summary>
        /// Creates user from social login
        /// </summary>
        /// <param name="userManager">the usermanager</param>
        /// <param name="context">the context</param>
        /// <param name="model">the model</param>
        /// <param name="loginProvider">the login provider</param>
        /// <returns>System.Threading.Tasks.Task&lt;User&gt;</returns>
        
        public static async Task<User> CreateUserFromSocialLogin(this UserManager<User> userManager, ApplicationDbContext context, CreateUserFromSocialLogin model, LoginProvider loginProvider)
        {
            //CHECKS IF THE USER HAS NOT ALREADY BEEN LINKED TO AN IDENTITY PROVIDER
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

                //EMAIL IS CONFIRMED; IT IS COMING FROM AN IDENTITY PROVIDER
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

            //ADDS THE USER TO AN IDENTITY PROVIDER
            var result = await userManager.AddLoginAsync(user, userLoginInfo);

            if (result.Succeeded)
                return user;

            else
                return null;
        }
    }
}
