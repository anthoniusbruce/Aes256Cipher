
using System.Security.Cryptography;
using System.Text;

namespace Aes256Cipher
{
    public static class Cipher
    {
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
    }
}
