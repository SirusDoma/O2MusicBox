using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Genode.Cryptography
{
    /// <summary>
    /// Provides MD5 Cryptography Functions.
    /// </summary>
    public static class MD5
    {
        /// <summary>
        /// Compute MD5 Hash.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>MD5 Hash.</returns>
        public static string Compute(string input)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));

            return sb.ToString();
        }

        /// <summary>
        /// Compute MD5 Hash.
        /// </summary>
        /// <param name="input">Input raw bytes.</param>
        /// <returns>MD5 Hash.</returns>
        public static string Compute(byte[] input)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] hash = md5.ComputeHash(input);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));

            return sb.ToString();
        }
    }
}
