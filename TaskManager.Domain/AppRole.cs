using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskManager.Domain
{
    public class AppRole
        : IdentityRole
    {
        public enum RoleType
        {
            User
        }

        public static string RoleTypeToString(RoleType roleType)
        {
            if (roleType == RoleType.User)
            {
                return "User";
            }

            throw new ArgumentException(
                    string.Format("Role '{0}' is not implemented", roleType.ToString()));
        }

        public AppRole()
            : base()
        {
        }

        public AppRole(string name)
            : base(name)
        {
        }
    }
}
