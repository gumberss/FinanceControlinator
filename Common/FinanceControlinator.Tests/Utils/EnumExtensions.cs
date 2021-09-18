using System;
using System.ComponentModel;

namespace FinanceControlinator.Tests.Utils
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T enumValue)
             where T : Enum
        {
            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(description);

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return description;
        }
    }
}
