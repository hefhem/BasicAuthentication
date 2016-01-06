using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;


namespace WebForTraining.Models
{
    public class Security 
    {
        private static string stringEncryptionKey = "p@ssw0rd1".PadRight(32);
        private static byte[] InitializationVector = new byte[] { 2, 18, 6, 8, 10, 12, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        //this method is used to encrypt a registering users password
        //before storing it in the database
        public static string Encrypt(string RawPassword)
        {
            byte[] BytesToEncrypt = Encoding.UTF8.GetBytes(RawPassword);
            byte[] EncryptionKey = Encoding.UTF8.GetBytes(stringEncryptionKey);
            string EncryptedPassword = null;
            Aes AESAlgorithm = Aes.Create();
            AESAlgorithm.Key = EncryptionKey;
            AESAlgorithm.IV = InitializationVector;

            using (MemoryStream StreamIntoMemory = new MemoryStream())
            {
                using (CryptoStream EncryptionStream = new CryptoStream(StreamIntoMemory, AESAlgorithm.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    EncryptionStream.Write(BytesToEncrypt, 0, BytesToEncrypt.Length);
                }

                byte[] EncryptedBytes = StreamIntoMemory.ToArray();
                EncryptedPassword = Convert.ToBase64String(EncryptedBytes);
            }

            return EncryptedPassword;

        }

        public string Decrypt(string EncryptedPassword)
        {
            byte[] BytesToDecrypt = Convert.FromBase64String(EncryptedPassword);
            byte[] EncryptionKey = Encoding.UTF8.GetBytes(stringEncryptionKey);
            string RawPassword = null;
            Aes AESAlgorithm = Aes.Create();
            AESAlgorithm.Key = EncryptionKey;
            AESAlgorithm.IV = InitializationVector;
            using (MemoryStream StreamIntoMemory = new MemoryStream())
            {
                using (CryptoStream DecryptionStream = new CryptoStream(StreamIntoMemory, AESAlgorithm.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    DecryptionStream.Write(BytesToDecrypt, 0, BytesToDecrypt.Length);
                }
                byte[] DecryptedBytes = StreamIntoMemory.ToArray();
                string DecryptedString = Encoding.UTF8.GetString(DecryptedBytes);
                RawPassword = DecryptedString;
            }

            return RawPassword;
        }
    }


}