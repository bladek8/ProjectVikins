using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Assets.Script.Helpers
{
    public static class PropertyMethods
    {
        public static string GetDisplayName(this MemberInfo property)
        {
            var displayName = property.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault();
            if (displayName == null) return null;
            DisplayNameAttribute mI = (DisplayNameAttribute)displayName;
            return mI.DisplayName;
        }
    }
}
