using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGPera.Web.Models.Ddr
{
    public class CipherDecipher
    {
        public string Encrypt(string txt)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[txt.Length];
            encode = Encoding.UTF8.GetBytes(txt);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        public string Decrypt(string txt)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(txt);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }
    }
}
