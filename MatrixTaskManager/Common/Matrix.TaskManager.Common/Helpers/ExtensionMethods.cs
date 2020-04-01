using Matrix.TaskManager.Common.Model;
using System.Collections.Generic;
using System.Linq;

namespace Matrix.TaskManager.Common.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<UserInfo> WithoutPasswords(this IEnumerable<UserInfo> users) {
            return users.Select(x => x.WithoutPassword());
        }

        public static UserInfo WithoutPassword(this UserInfo user) {
            user.Password = null;
            return user;
        }
    }
}