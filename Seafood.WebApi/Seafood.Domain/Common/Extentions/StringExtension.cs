using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Seafood.Domain.Common.Extentions
{
    public static class StringExtension
    {
        /// <summary>
        /// Get description annotation of an enum value
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetDescription(this System.Enum enumValue)
        {
            Type type = enumValue.GetType();

            MemberInfo[] memberInfo = type.GetMember(enumValue.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            return enumValue.ToString();
        }
        /// <summary>
        /// Đưa kiểu unikey về không dấu
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetVnStringOnlyCharactersAndNumbers(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            StringBuilder sb = new StringBuilder();
            char[] ca = value.Trim().ToCharArray();
            foreach (char t in ca)
            {
                char x = GetValidVnChar(t);
                sb.Append(x);
            }
            return sb.ToString();
        }
        private static char GetValidVnChar(char x)
        {
            if ((x >= 'a' && x <= 'z') || (x >= '0' && x <= '9') || (x >= 'A' && x <= 'Z'))
            {
                return x;
            }
            string s = x.ToString();

            if ("àáạảãâầấậẩẫăắằặẳẵ".Contains(s)) return 'a';
            if ("èéẹẻẽêềếệểễ".Contains(s)) return 'e';
            if ("ìíịỉĩ".Contains(s)) return 'i';
            if ("đ".Contains(s)) return 'd';
            if ("òóọỏõôồốộổỗơờớợởỡ".Contains(s)) return 'o';
            if ("ùúụủũưừứựửữ".Contains(s)) return 'u';
            if ("ỳýỵỷỹ".Contains(s)) return 'y';
            if ("ÀÁẠẢÃÂẦẤẬẨẪĂẮẰẶẲẴ".Contains(s)) return 'A';
            if ("ÈÉẸẺẼÊỀẾỆỂỄ".Contains(s)) return 'E';
            if ("ÌÍỊỈĨ".Contains(s)) return 'I';
            if ("Đ".Contains(s)) return 'D';
            if ("ÒÓỌỎÕÔỒỐỘỔỖƠỜỚỢỞỠ".Contains(s)) return 'O';
            if ("ÙÚỤỦŨƯỪỨỰỬỮ".Contains(s)) return 'U';
            if ("ỲÝỴỶỸ".Contains(s)) return 'Y';
            return x;
        }
        public static string FormatDatetimeToDayMonthYear(this DateTime value)
        {
            return string.Format("{0}/{1}/{2}",value.Day, value.Month, value.Year);
        }
        public static string FormatDatetimeToHourMinDayMonthYear(this DateTime value)
        {
            return string.Format("{0}:{1} {2}/{3}/{4}", value.Hour, value.Minute, value.Day, value.Month, value.Year);
        }
    }
}
