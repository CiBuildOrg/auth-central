using System;
using System.Security.Cryptography;
using System.Text;

namespace Fsw.Enterprise.AuthCentral.Crypto
{
    class PasswordGenerator
    {
        internal static string GeneratePasswordOfLength(int length)
        {
            // Technically the minimum length for this algorithm is 3, 
            // but AuthCentral requires 7 anyway so...
            if(length < 7)
            {
                throw new ArgumentException("Password length must be at least seven characters.", "length");
            }

            byte[] data = new byte[length * 2];
            char[] chars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=[]\{}|;':./,<>?".ToCharArray();

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
            }

            StringBuilder result = new StringBuilder(length);

            // lower case
            result.Append(chars[data[0] % 26]);

            // upper case
            result.Append(chars[data[1] % 26 + 26]);

            // digit
            result.Append(chars[data[2] % 10 + 52]);

            // totally random
            for (int i = 3; i < length; i++)
            {
                result.Append(chars[data[i] % chars.Length]);
            }

            // Fisher-Yates shuffle
            for (int i = 0; i < length; i++)
            {
                int oldIndex = data[i + length] % length;
                char oldChar = result[oldIndex];
                result.Remove(oldIndex, 1);

                char newChar = result[0];
                result.Remove(0, 1);
                result.Insert(0, oldChar);
                result.Insert(oldIndex, newChar);
            }
            
            return result.ToString();
        }
    }
}