using System.Security.Cryptography;
using System.Text;

namespace Fsw.Enterprise.AuthCentral.Crypto
{
    class PasswordGenerator
    {
        // http://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings-in-c
        internal static string GeneratePasswordOfLength(int length)
        {
            byte[] data = new byte[length];
            char[] chars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=[]\{}|;':./,<>?".ToCharArray();

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(length);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
    }
}