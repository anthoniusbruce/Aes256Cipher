
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Aes256Cipher
{
    public static class Cipher
    {
        public static string CreateNewKey()
        {
            using var aes = Aes.Create();
            aes.GenerateKey();
            return Convert.ToBase64String(aes.Key);
        }

        public static void EncryptSecuredJsonFields(string fileName, string key)
        {
            var jsonFile = new FileInfo(fileName);
            var json =  File.ReadAllText(jsonFile.FullName);
            var byteKey = Convert.FromBase64String(key);
            var result = ReplaceCipherText(json, byteKey, Encrypt);

            File.WriteAllText(jsonFile.FullName, result);
        }

        public static string Encrypt(string text, byte[] key)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV();
            using var cipher = aes.CreateEncryptor();
            var byteText = Encoding.UTF8.GetBytes(text);
            var cipherText = cipher.TransformFinalBlock(byteText, 0, byteText.Length);
            return Convert.ToBase64String(aes.IV.Concat(cipherText).ToArray());
        }

        public static void DecryptSecuredJsonFields(string fileName, string key)
        {
            var jsonFile = new FileInfo(fileName);
            var json = File.ReadAllText(jsonFile.FullName);
            var byteKey = Convert.FromBase64String(key);
            var result = ReplaceCipherText(json, byteKey, Decrypt);

            File.WriteAllText(jsonFile.FullName, result);
        }

        public static string Decrypt(string text, byte[] key)
        {
            var ivAndCipherText = Convert.FromBase64String(text);
            using var aes = Aes.Create();
            var ivLength = aes.BlockSize / 8;
            if (ivLength > ivAndCipherText.Length)
            {
                return string.Empty;
            }
            aes.IV = ivAndCipherText.Take(ivLength).ToArray();
            aes.Key = key;
            using var cipher = aes.CreateDecryptor();
            var cipherText = ivAndCipherText.Skip(ivLength).ToArray();
            var result = cipher.TransformFinalBlock(cipherText, 0, cipherText.Length);
            return Encoding.UTF8.GetString(result);
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
