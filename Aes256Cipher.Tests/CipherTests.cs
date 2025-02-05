using System.Security.Cryptography;
using System.Text;

namespace Aes256Cipher.Tests
{
    [TestClass]
    public sealed class CipherTests
    {
        static readonly byte[] KEY = Convert.FromBase64String("r02fdzOj9ksNZAQk7KWHuRfMishwSXZarrTlzWRw1xc=");
        private string Decipher(string value)
        {
            var ivAndCipherText = Convert.FromBase64String(value);
            using var aes = Aes.Create();
            var ivLength = aes.BlockSize / 8;
            aes.IV = ivAndCipherText.Take(ivLength).ToArray();
            aes.Key = KEY;
            using var cipher = aes.CreateDecryptor();
            var cipherText = ivAndCipherText.Skip(ivLength).ToArray();
            var text = cipher.TransformFinalBlock(cipherText, 0, cipherText.Length);
            return Encoding.UTF8.GetString(text);
        }

        [TestMethod]
        public void Encrypt_Text_ReturnsEncryptedText()
        {
            var encryptMe = "The quick fox jumped over the lazy hound";

            var actual = Cipher.Encrypt(encryptMe, KEY);

            Assert.AreEqual(encryptMe, Decipher(actual));
        }

        [TestMethod]
        public void Encrypt_EmptyText_ReturnsEncryptedText()
        {
            var encryptMe = string.Empty;

            var actual = Cipher.Encrypt(encryptMe, KEY);

            Assert.AreEqual(encryptMe, Decipher(actual));
        }

        [TestMethod]
        public void Decrypt_Text_ReturnsDecryptedText()
        {
            var decryptMe = "Tk2zXsG2IBs2dnv7IWgzyFkiRimKp73G1iIwTt9tYqQVXPAcpM06wEXHRVeTgDE1BnkKE9mDHi2OspLVwvPT8g==";
            var expected = "The quick fox jumped over the lazy hound";

            var actual = Cipher.Decrypt(decryptMe, KEY);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("123456789abcdef0")]
        public void Decrypt_EmptyText_EmptyText(string decryptMe)
        {
            var expected = string.Empty;

            var actual = Cipher.Decrypt(decryptMe, KEY);

            Assert.AreEqual(expected, actual);
        }
    }
}
