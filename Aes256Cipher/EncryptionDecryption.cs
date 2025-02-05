using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Aes256Cipher
{
    public class EncryptionDecryption
    {
        public static string CreateNewKey()
        {
            using var aes = Aes.Create();
            aes.GenerateKey();
            return Convert.ToBase64String(aes.Key);
        }

        internal static void EncryptSecuredJsonFields(string fileName, string key)
        {
            var jsonFile = new FileInfo(fileName);
            var json = File.ReadAllText(jsonFile.FullName);
            var byteKey = Convert.FromBase64String(key);
            var result = ReplaceCipherText(json, byteKey, Cipher.Encrypt);

            File.WriteAllText(jsonFile.FullName, result);
        }

        internal static void DecryptSecuredJsonFields(string fileName, string key)
        {
            var jsonFile = new FileInfo(fileName);
            var json = File.ReadAllText(jsonFile.FullName);
            var byteKey = Convert.FromBase64String(key);
            var result = ReplaceCipherText(json, byteKey, Cipher.Decrypt);

            File.WriteAllText(jsonFile.FullName, result);
        }

        public static string ReplaceCipherText(string json, byte[] byteData, Func<string, byte[], string> modifier)
        {
            var result = Regex.Replace(json,
                @"""CipherText:(?<Text>[^""]+)""",
                m =>
                {
                    var text = m.Groups["Text"].Value;
                    try
                    {
                        var cipherText = modifier(text, byteData);
                        return $@"""CipherText:{cipherText}""";
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($@"Failed ""{text}""");
                        return m.Value;
                    }
                });
            return result;
        }

    }
}
