using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Common.Cryptology
{
    public class Encryption
    {
        public static string GenerateMD5(string value)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        public static string CreateDateTimeUniqeID()
        {
            string prefix = string.Empty;
            return prefix + ((Int64)(DateTime.UtcNow.Subtract(new DateTime(2000, 1, 1))).TotalMilliseconds).ToString();
        }

        public static string GenerateSixDigitNumber()
        {
            Random generator = new Random();
            String r = generator.Next(0, 999999).ToString("D6");
            return r.ToString();
        }
    }
}
