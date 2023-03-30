using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MTLicense
{
    public class License
    {
        private static string secretKey = "";
        //AesEncryption
        private static byte[] DeriveKey(string password, int keySize)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, 1000))
            {
                return deriveBytes.GetBytes(keySize / 8);
            }
        }

        public static string EncryptString(string plainText)
        {
            byte[] key = DeriveKey(secretKey, 256);
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string cipherText)
        {
            byte[] key = DeriveKey(secretKey, 256);
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

}
