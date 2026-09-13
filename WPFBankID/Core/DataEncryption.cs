using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WPFBankID
{
    public static class DataEncryption
    {
        public static string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        public static string Decrypt(string base64Text)
        {
            if (string.IsNullOrEmpty(base64Text)) return "";
            byte[] bytes = Convert.FromBase64String(base64Text);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
