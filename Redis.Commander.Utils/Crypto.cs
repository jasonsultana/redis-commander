using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DeviceId;

namespace Redis.Commander.Utils
{
    public static class Crypto
    {
        public static string GetKey()
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

            var cypherText = EncryptString(plainText);
            return cypherText;
        }

        public static string Decrypt(string cypherText)
        {
            if (string.IsNullOrWhiteSpace(cypherText))
                return cypherText;

            var plainText = DecryptString(cypherText);
            return plainText;
        }

        private static string EncryptString(string plainText)
        {
            byte[] buffer;
            using var aes = GetAes();
            
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                // The writer needs to be disposed before calling memoryStream.ToArray(), so we need to keep this using block here.
                streamWriter.Write(plainText);
            }

            buffer = memoryStream.ToArray();

            return Convert.ToBase64String(buffer);
        }

        private static string DecryptString(string cipherText)
        {
            var buffer = Convert.FromBase64String(cipherText);
            using var aes = GetAes();

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(buffer);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }

        private static Aes GetAes()
        {
            var iv = new byte[16];
            var key = GetKey();

            var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;

            return aes;
        }
    }
}