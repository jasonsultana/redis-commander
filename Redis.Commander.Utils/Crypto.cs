using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DeviceId;

namespace Redis.Commander.Utils
{
    public static class Crypto
    {
        private static string GetKey()
        {
            // Use a unique fingerprint of the user's device as a symmetrical encryption key.
            // This doesn't guarantee that your creds are safe if your machine is compromised,
            // but it does mean that your SQLite DB can't just be copied from one machine
            // to another.

            // Also, if you change your machine name, change network cards, update your OS or change your username,
            // you'll need to reconfigure your connections. I think that's acceptable.

            // To a certain extent, the onus of protecting access to your physical machine is
            // on the user.
            string deviceId = new DeviceIdBuilder()
                .AddMachineName()
                .AddMacAddress()
                .AddOsVersion()
                .AddUserName()
                .ToString()
                .Substring(0, 16);

            return deviceId;
        }

        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
                return plainText;

            var cypherText = EncryptString(GetKey(), plainText);
            return cypherText;
        }

        public static string Decrypt(string cypherText)
        {
            if (string.IsNullOrWhiteSpace(cypherText))
                return cypherText;

            var plainText = DecryptString(GetKey(), cypherText);
            return plainText;
        }

        private static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        private static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}