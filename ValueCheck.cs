using System.Text;
using System.Text.RegularExpressions;
using System;

namespace Zdd.Utility
{
    /// <summary>
    /// 封装数据校验方法的类
    /// </summary>
    public static class ValueCheck
    {
        /// <summary>
        /// 计算字符串长度（一个双字节字符长度计2，ASCII字符计1）
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static int GetByteLength(string strInput)
        {
            if (string.IsNullOrEmpty(strInput))
                return 0;

            strInput = strInput.Trim();
            int byteLen = strInput.Length;
            foreach (char c in strInput)
            {
                if (Convert.ToInt32(c) < 0 || Convert.ToInt32(c) > 255)
                    byteLen++;
            }
            return byteLen;
        }

        /// <summary>
        ///验证字符串长度,验证通过返回true (一个双字节字符长度计2，ASCII字符计1)
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="iMin">最小值</param>
        /// <param name="iMax">最大值</param>
        /// <returns></returns>
        public static bool LengthCheck(string strInput, int iMin, int iMax)
        {
            int iLength = GetByteLength(strInput);
            if (iLength < iMin || iLength > iMax)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 邮政编码合法性检测
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool PostalCodeCheck(string strInput)
        {
            Regex re = new Regex(@"\d{6}");
            if (!re.IsMatch(strInput) || strInput.Length != 6)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 电话号码验证
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool PhoneCheck(string strInput)
        {
            Regex re = new Regex(@"(^[0-9]{3,4}\-[0-9]{7,8}$)|(^[0-9]{7,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)");
            if (!re.IsMatch(strInput))
            {
                return false;
            }
            return true;
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

        /// <summary>
        /// 密码验证
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool PwdCheck(string strInput)
        {
            Regex re = new Regex(@"^[a-zA-Z0-9|\w]+$");
            if (re.IsMatch(strInput) == false)
            {
                return false;
            }
            return true;
        }

        // <summary>
        /// 字符串输入的验证 允许字母数字中文开头
        /// 通常用于 用户名 密码 等字符串输入的验证
        /// 并可以控制字符串长度
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool ChineseStringCheck(string strInput)
        {
            Regex re = new Regex(@"^[a-zA-Z0-9|\u4e00-\u9fa5\w|（）()-]+$");
            if (re.IsMatch(strInput) == false)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Email地址合法性检查
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool EmailCheck(string strInput)
        {
            Regex re = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            if (!re.IsMatch(strInput))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 只能输入数字的验证
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool NumCheck(string strInput)
        {
            Regex re = new Regex(@"^[0-9]+$");
            if (!re.IsMatch(strInput))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Url验证
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool UrlCheck(string strInput)
        {
            /*
            var strRegex = "^((https|http|ftp|rtsp|mms)?://)" 
                            + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //ftp的user@ 
                            + "(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP形式的URL- 199.194.52.184 
                            + "|" // 允许IP和DOMAIN（域名）
                            + "([0-9a-z_!~*'()-]+\.)*" // 域名- www. 
                            + "([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\." // 二级域名 
                            + "[a-z]{2,6})" // first level domain- .com or .museum 
                            + "(:[0-9]{1,4})?" // 端口- :80 
                            + "((/?)|" // a slash isn't required if there is no file name 
                            + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$"; 
             */

            Regex re =
                new Regex(
                    @"^((https|http|ftp|rtsp|mms)?://)?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?(([0-9]{1,3}\.){3}[0-9]{1,3}|([0-9a-z_!~*'()-]+\.)*([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\.[a-z]{2,6})(:[0-9]{1,4})?((/?)|(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$");
            if (!re.IsMatch(strInput))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 包含[.$^{}[](|)*+?\~!@#%&amp;=`;:'<>/,&quot;]与空格 等字符的验证
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool TeShuCheck(string strInput)
        {
            Regex re = new Regex(@"^[^\.\$\^\{\}\[\]\(\|\)\*\+\?\\~!@#%&amp;\-=`;:'<>/,\x22\x26 \s]+$");
            if (!re.IsMatch(strInput))
            {
                return false;
            }
            return true;
        }

        public static bool NumRangeCheck(string strInput, int min, int max)
        {
            int inputNum;
            Int32.TryParse(strInput, out inputNum);
            if(inputNum < min || inputNum > max)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 中文 英文 字母 数字 下划线
        /// </summary>
        /// <param name="strInput">The STR input.</param>
        /// <returns>验证成功返回true</returns>
        public static bool ChineseNumLetter(string strInput)
        {
            Regex re = new Regex(@"^[a-zA-Z0-9|\u4e00-\u9fa5\w]+$");
            return re.IsMatch(strInput);
        }
    }
}