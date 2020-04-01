using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Matrix.TaskManager.Common.Extensions
{
    public class AttributesExtensions
    {
        public static string GetDisplayName<T>()
        {
            var displayName = typeof(T)
              .GetCustomAttributes(typeof(DisplayNameAttribute), true)
              .FirstOrDefault() as DisplayNameAttribute;

            if (displayName != null)
                return displayName.DisplayName;

            return "";
        }
    }
}
