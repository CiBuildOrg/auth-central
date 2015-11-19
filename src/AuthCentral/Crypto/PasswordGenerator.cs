using System;
using System.Security.Cryptography;
using System.Text;

namespace Fsw.Enterprise.AuthCentral.Crypto
{
    class PasswordGenerator
    {
        // http://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings-in-c
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

            // Fisher-Yates shuffle (with a really fancy string-builder technique)
            for (int oldIndex = 0; oldIndex < length; oldIndex++)
            {
                int newIndex = data[oldIndex + length] % length;
                char oldChar = result[oldIndex];
                char newChar = result[newIndex];

                result.Remove(oldIndex, 1);
                result.Insert(oldIndex, newChar);
                result.Remove(newIndex, 1);
                result.Insert(newIndex, oldChar);
            }
            
            return result.ToString();
        }
    }
}