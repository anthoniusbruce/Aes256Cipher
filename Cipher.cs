
using System.Security.Cryptography;

namespace Aes256Cipher
{
    internal static class Cipher
    {
        internal static string CreateNewKey()
        {
            using var aes = Aes.Create();
            aes.GenerateKey();
            return Convert.ToBase64String(aes.Key);
        }
    }
}
