using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using TaskManager.Domain;

namespace TaskManager.WebUI.Infrastructure
{
    public class AppUserManager
        : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store)
            : base(store)
        {
        }

        public static AppUserManager Create(
               IdentityFactoryOptions<AppUserManager> options,
               IOwinContext context)
        {
            TaskManagerContext db = context.Get<TaskManagerContext>();
            AppUserManager manager = new AppUserManager(new UserStore<AppUser>(db));

            manager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false
                };

            manager.UserValidator = new UserValidator<AppUser>(manager)
                {
                    AllowOnlyAlphanumericUserNames = false
                };

            return manager;
        }
    }
}