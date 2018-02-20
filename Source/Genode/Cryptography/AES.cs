using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.IO;

namespace Genode.Cryptography
{
    /// <summary>
    /// Provides AES Cryptography Functions.
    /// </summary>
    public static class AES
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes("Live_nolimitO2"); // 

        /// <summary>
        /// Encrypt the given data.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <param name="password">A password used to generate a key for encryption.</param>
        public static byte[] Encrypt(byte[] data, string password)
        {
            byte[] result = null;                       // Encrypted data to return
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, _salt);

                // Create a RijndaelManaged object
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV
                    //msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (BinaryWriter swEncrypt = new BinaryWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(data);
                        }
                    }
                    result = msEncrypt.ToArray();
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return result;
        }

        /// <summary>
        /// Decrypt the given encrypted data.  
        /// Assumes the data was encrypted using AES.Encrypt() using an identical password.
        /// </summary>
        /// <param name="data">The data to decrypt.</param>
        /// <param name="password">A password used to generate a key for decryption.</param>
        public static byte[] Decrypt(byte[] data, string password)
        {

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            byte[] result = null;

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, _salt);

                using (MemoryStream msDecrypt = new MemoryStream(data))
                {
                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                    // Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(msDecrypt);

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (BinaryReader rdr = new BinaryReader(csDecrypt))
                        {
                            byte[] buffer = new byte[1024];
                            List<byte> res = new List<byte>();

                            int read = 0;
                            do
                            {
                                read = rdr.Read(buffer, 0, buffer.Length);

                                if (read > 0)
                                {
                                    byte[] loaded = new byte[read];
                                    Array.Copy(buffer, loaded, read);
                                    res.AddRange(loaded);
                                }
                            }
                            while (read > 0);
                            result = res.ToArray();
                        }
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return result;
        }

        private static byte[] ReadByteArray(Stream s)
        {
            //byte[] rawLength = new byte[sizeof(int)];
            //if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            //{
            //    Logger.Error("AES: Stream did not contain properly formatted byte array");
            //    return null;
            //}

            //byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            byte[] buffer = new byte[16];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                Logger.Error("AES: Did not read byte array properly");
                return null;
            }

            return buffer;
        }
    }
}
