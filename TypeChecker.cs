using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Zdd.Utility
{
    public static class TypeChecker
    {
        public static bool IsInt32(string tbString)
        {
            int number;
            return Int32.TryParse(tbString.Trim(), out number);
        }

        public static int ToInt32(string tbString)
        {
            if (String.IsNullOrEmpty(tbString))
            {
                return 0;
            }

            int number;
            if (Int32.TryParse(tbString.Trim(), out number))
            {
                return number;
            }
            else
            {
                return 0;
            }
        }

        public static bool IsValidCustomerNumber(string str)
        {
            return IsInt32(str);
        }

        public static bool IsValidSONumber(string str)
        {
            return IsInt32(str);
        }

        public static bool IsValidTrackingNumber(string str)
        {
            return !String.IsNullOrEmpty(str);
        }

        public static bool IsValidRMANumber(string str)
        {
            return IsInt32(str);
        }

        /// <summary>
        /// IP地址验证
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool IPCheck(string strInput)
        {
            Regex re = new Regex(@"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$");
            if (!re.IsMatch(strInput))
            {
                return false;
            }
            return true;
        }
    }
}
