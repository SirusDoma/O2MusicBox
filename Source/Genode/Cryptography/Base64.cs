using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Genode.Cryptography
{
    /// <summary>
    /// Provides Base64 Cryptography Functions.
    /// </summary>
    public static class Base64
    {
        /// <summary>
        /// Encrypt string using Base64.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Base64 Hash.</returns>
        public static string Encrypt(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }

        /// <summary>
        /// Decrypt string using Base64.
        /// </summary>
        /// <param name="input">Input Base64 String.</param>
        /// <returns>Decrypted String.</returns>
        public static string Decrypt(string input)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(input));
        }
    }
}
