using System;
using System.Text;

namespace ConsoleApp2
{
    public class Program
    {
        private static void PrintMessage(string message, string stringToinclude = "", char delimiter = ':')
        {
            if (stringToinclude != string.Empty)
            {
                Console.WriteLine($"{message} {delimiter} {stringToinclude}");
            }
            else
            {
                Console.WriteLine(message);
            }

        }

        private static string GetStringOfBytes(byte[] bytes)
        {
            var sb = new StringBuilder();

            foreach (byte b in bytes)
            {
                sb.Append($"{b} ");
            }

            return sb.ToString();
        }

        public static void Main()
        {
            EncryptorDecryptor encryptorDecryptor =
                new EncryptorDecryptor("AzYVa1xuFHxWOyXLPsOwbU9HDhsS9PyP", "wpHSiAWAQBBmBZNP");

            Console.Write("Input the string to be encrypted: ");
            string stringToEncrypt = Console.ReadLine();

            ValueTuple <byte[], string> encryptedBytesAndString = 
                encryptorDecryptor.GetEncryptedBytesAndString(stringToEncrypt);

            PrintMessage("Encrypted string", encryptedBytesAndString.Item2);
            PrintMessage("Encrypted string in bytes", GetStringOfBytes(encryptedBytesAndString.Item1));
            PrintMessage("Decrypted string", encryptorDecryptor.GetDecryptedString(encryptedBytesAndString.Item2));

            Console.ReadKey();
        }
    }
}
