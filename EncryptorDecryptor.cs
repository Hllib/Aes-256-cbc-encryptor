using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp2
{
    public class EncryptorDecryptor
    {
        private string _encryptionKey;
        private string _encryptionIv;

        public EncryptorDecryptor(string key, string iv)
        {
            _encryptionKey = key;
            _encryptionIv = iv;
        }

        public ValueTuple<byte[], string> GetEncryptedBytesAndString(string textToEncrypt)
        {
            byte[] encryptedBytes;
            byte[] ivBytes;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
                ivBytes = _encryptionIv == string.Empty ? aes.IV 
                    : aes.IV = Encoding.UTF8.GetBytes(_encryptionIv);

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(textToEncrypt);
                        }

                        encryptedBytes = memoryStream.ToArray();
                    }
                }
            }

            string encryptedString = Convert.ToBase64String(ivBytes.Concat(encryptedBytes).ToArray());
            return new ValueTuple<byte[], string>(encryptedBytes, encryptedString);
        }

        public string GetDecryptedString(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] ivBytes = _encryptionIv == string.Empty ? encryptedBytes.Take(16).ToArray()
                : Encoding.UTF8.GetBytes(_encryptionIv);
            byte[] encrypted = encryptedBytes.Skip(16).ToArray();

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
                aes.IV = ivBytes;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(encrypted))
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

