using System;
using System.Collections.Generic;
using System.Text;

namespace MTLicense
{
    class Program
    {
        static void Main(string[] args)
        {
            string originalString = "Hello World!";

            // 加密
            string encryptedString = License.EncryptString(originalString);
            Console.WriteLine("Original String: {0}\nEncrypted String: {1}", originalString, encryptedString);

            // 解密
            string decryptedString = License.DecryptString(encryptedString);
            Console.WriteLine("Decrypted String: {0}", decryptedString);
            Console.ReadKey();
        }
    }
}
