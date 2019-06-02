using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BTC.Common.Util.Extension
{
    public static class ExtensionMethods
    {

        public static string ConvertToString(this int[] value)
        {
            if (value == null || value.Length <= 0)
            {
                return null;
            }
            string result = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (i == value.Length - 1)
                    result += value[i];
                else
                    result += value[i] + ",";
            }
            return result;
        }

        public static string ReplacePhoneChar(this string value)
        {
            return value != null ? value.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "") : value;
        }

        public static string ToUiFormatString(this DateTime? value)
        {

            if (value == null)
                return "";

            string val = value.Value.ToString("dd.MM.yyyy");
            return val;

        }

        public static decimal? ToDecimal(this string obj)
        {
            decimal? val = null;
            try
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("tr-TR");
                obj = obj.Replace(ci.NumberFormat.CurrencyGroupSeparator, ci.NumberFormat.CurrencyDecimalSeparator);
                val = Convert.ToDecimal(obj, ci);
            }
            catch { }

            return val;
        }

        public static string ToReplacePhoneMask(this string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
                return "";
            try
            {
                return obj.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
            }
            catch { return ""; }
        }

        public static decimal? ToDecimalFromServerCulture(this string obj)
        {
            decimal? val = null;
            try
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("tr-TR");
                val = Convert.ToDecimal(obj, ci);
            }
            catch { }

            return val;
        }
        public static decimal? ToDecimalFromTime(this string obj)
        {
            decimal? val = null;
            try
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("tr-TR");
                obj = obj.Replace(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator, ci.NumberFormat.CurrencyDecimalSeparator);
                obj = obj.Replace(ci.NumberFormat.CurrencyGroupSeparator, ci.NumberFormat.CurrencyDecimalSeparator);

                obj = obj.Replace(":", ci.NumberFormat.CurrencyDecimalSeparator);

                val = Convert.ToDecimal(obj, ci);
            }
            catch { }

            return val;
        }

        public static int ToIntFromDecimalValue(this decimal? obj)
        {
            if (obj > 0 && obj != null)
            {
                string str = obj.ToString();
                str = str.Replace(",", "");
                int val = 0;
                int result = 0;
                if (str == null) return 0;
                if (int.TryParse(str, out val))
                {
                    return result = val;
                }
                return 0;
            }
            else
                return 0;
        }

        public static decimal ToDecimalFromIntValue(this int obj)
        {
            return obj / 100;
        }

        public static DateTime ToDateTimeDatFormat(this object value)
        {
            int day = 0;
            int month = 0;
            int year = 0;
            string val = value as string;
            string[] parts = val.Split('.');
            day = Convert.ToInt32(parts[0]);
            month = Convert.ToInt32(parts[1]);
            year = Convert.ToInt32(parts[2]);
            DateTime date = new DateTime(year, month, day);
            return date;
        }
        public static int? ToInt32(this object value)
        {
            int val = 0;
            int? result = null;
            if (value == null) return null;
            if (int.TryParse(value.ToString(), out val))
            {
                result = val;
            }

            return result;
        }
        public static byte? ToByte(this object value)
        {
            byte val = 0;
            byte? result = null;
            if (value == null) return null;
            if (byte.TryParse(value.ToString(), out val))
            {
                result = val;
            }

            return result;
        }
        public static decimal? ToDecimal(this object value)
        {
            decimal val = 0;
            decimal? result = null;
            if (value == null) return null;

            if (value.ToString().Contains('.') || value.ToString().Contains(','))
            {
                string groupChar = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                string decimalChar = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;

                #region convert source to decimal string
                if (value.ToString().Contains(','))
                {
                    string[] arr = value.ToString().Split(',');
                    value = arr[0] + "," + arr[1].PadRight(2, '0');
                }
                else if (value.ToString().Contains('.'))
                {
                    string[] arr = value.ToString().Split('.');
                    value = arr[0] + "." + arr[1].PadRight(2, '0');
                }
                string s = value.ToString().Replace(",", "").Replace(".", "");
                s = s.Insert(s.Length - 2, decimalChar);
                #endregion

                s = s.Replace(groupChar, decimalChar);

                if (decimal.TryParse(s, out val))
                {
                    result = val;
                }
            }
            else
            {
                if (decimal.TryParse(value.ToString(), out val))
                {
                    result = val;
                }
            }

            return result;
        }
        public static DateTime? ToDateTime(this object value)
        {
            DateTime val = DateTime.MinValue;
            DateTime? result = null;
            if (value == null) return null;

            if (DateTime.TryParse(value.ToString(), out val))
            {
                result = val;
            }

            return result;
        }
        public static bool? ToBoolean(this object value)
        {
            bool val = false;
            bool? result = null;
            if (value == null) return null;
            if (bool.TryParse(value.ToString(), out val))
            {
                result = val;
            }

            return result;
        }

        public static byte[] ToByteArray(this HttpPostedFileBase value)
        {
            if (value != null)
            {
                HttpPostedFileWrapper img = value as HttpPostedFileWrapper;
                byte[] imgByteArr = new BinaryReader(img.InputStream)
                               .ReadBytes((int)img.InputStream.Length);
                return imgByteArr;
            }
            else
            {
                return null;
            }

        }
        public static string ToDescription<TEnum>(this TEnum value)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var attributes = (System.ComponentModel.DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static string ToUniversalString(this string Source)
        {
            var tmp = Source.ToLower().Replace("ğ", "g").Replace("ü", "u").Replace("ş", "s").Replace("İ", "i").Replace("ö", "o").Replace("ç", "c").Replace(" ", "-").Replace("ı", "i").Replace(",", "-").Replace(".", "").Replace("?", "").Replace("&", "-").Replace("/", "-").Replace("\\", "-").Replace(";", "-");

            int counter = 0;
            while (counter < tmp.Length)
            {
                char c = tmp[counter];
                if (!char.IsLetter(c) && c != '-')
                {
                    tmp = tmp.Replace(c.ToString(), "");
                }
                ++counter;
            }

            while (tmp.EndsWith("-"))
            {
                tmp = tmp.Substring(0, tmp.Length - 1);
            }

            return tmp;
        }

        public static string RemoveHtmlTags(this string Source)
        {
            var tmp = Source.Replace("<p>", "").Replace("</p>", "").Replace("<br>", "").Replace("<br/>", "").Replace("<br />", "").Replace("<br >", "").Replace("<b>", "").Replace("</b>", "").Replace("<span>", "").Replace("</span>", "");
            return tmp;
        }
        public static string TRCharToHtmlCode(this string Source)
        {
            var tmp = Source.Replace("ü", "&#252;").Replace("İ", "&#304;").Replace("ı", "&#305;").Replace("ö", "&#246;").Replace("ç", "&#231;").Replace("ğ", "&#287;").Replace("ş", "&#351;").Replace("Ş", "&#350;").Replace("Ö", "&#214;").Replace("Ü", "&#220;").Replace("Ç", "&#199;").Replace("Ğ", "&#286;").Replace("Ş", "&#350;");

            return tmp;
        }
        public static string ClearNonAlphaNumericChars(this string Source)
        {
            string str = string.Empty;
            foreach (char c in Source)
            {
                if (char.IsNumber(c) == false && char.IsLetter(c) == false)
                {
                    continue;
                }

                str += c;
            }
            return str;
        }
        public static string ToParsedPartReference(this string Source)
        {
            if (Source == null) return string.Empty;
            return Source.Replace(" ", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("$", "").Replace("#", "").Replace("_", "").Replace("'", "").Replace("&", "").Replace("KMPSYD", "").Replace("AXASYD", "");
        }

    }
}
