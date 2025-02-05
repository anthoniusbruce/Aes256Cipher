namespace Aes256Cipher.Tests;

[TestClass]
public class EncryptionDecryptionTests
{
    [TestMethod]
    public void CreateNewKey_ReturnsNonEmptyStringEndingwithEqual()
    {
        var key = EncryptionDecryption.CreateNewKey();

        Assert.IsTrue(key.EndsWith("="));
    }

    [TestMethod]
    public void ReplaceCipherText_ReturnsToUpperText()
    {
        var text = @"{""tag"":""CipherText:upper me""}";
        var modifier = (string input, byte[] x) =>
        {
            return input.ToUpper();
        };
        var expected = @"{""tag"":""CipherText:UPPER ME""}";

        var actual = EncryptionDecryption.ReplaceCipherText(text, Array.Empty<byte>(), modifier);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ReplaceCipherText_MultipleReplacements_ReturnsMultpleToUpper()
    {
        var text = @"{
                           ""tag"":""CipherText:upper me"",
                           ""tag2"":""nothing"",
                           ""tag3"":""CipherText:upper me too"",
                           ""tag4"":""CipherText:and me""
                         }";
        var modifier = (string input, byte[] x) =>
        {
            return input.ToUpper();
        };
        var expected = @"{
                           ""tag"":""CipherText:UPPER ME"",
                           ""tag2"":""nothing"",
                           ""tag3"":""CipherText:UPPER ME TOO"",
                           ""tag4"":""CipherText:AND ME""
                         }";

        var actual = EncryptionDecryption.ReplaceCipherText(text, Array.Empty<byte>(), modifier);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(@"{""tag"":""CipherText:""}")]
    [DataRow(@"{""tag"":""no tag""}")]
    [DataRow(@"{""tag"":""""}")]
    [DataRow("")]
    public void ReplaceEmptyCipherText_ReturnsOriginalText(string text)
    {
        var modifier = (string input, byte[] x) =>
        {
            return input.ToUpper();
        };
        var expected = text;

        var actual = EncryptionDecryption.ReplaceCipherText(text, Array.Empty<byte>(), modifier);

        Assert.AreEqual(expected, actual);
    }
}
