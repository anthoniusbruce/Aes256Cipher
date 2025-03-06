using Aes256Cipher;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

if (args.Length == 0)
{
    OutDamnedHelp();
    return;
}

var requestHelp = false;
var createKey = false;
var encrypt = false;
var decrypt = false;
var filePath = string.Empty;

var filePathActive = false;
foreach (var arg in args)
{
    if (filePathActive)
    {
        filePath = arg;
        filePathActive = false;
    }
    else if (arg.ToLower() == "--help" || arg.ToLower() == "-h" || arg.ToLower() == "/h")
    {
        requestHelp = true;
    }
    else if (arg.ToLower() == "create")
    {
        createKey = true;
    }
    else if (arg.ToLower() == "encrypt")
    {
        encrypt = true;
    }
    else if (arg.ToLower() == "decrypt")
    {
        decrypt = true;
    }
    else if (arg.ToLower() == "--filename" || arg.ToLower() == "-f" || arg.ToLower() == "/f")
    {
        filePathActive = true;
    }
}

if (requestHelp)
{
    OutDamnedHelp();
    return;
}

if ((encrypt || decrypt) && string.IsNullOrWhiteSpace(filePath))
{
    Console.WriteLine("--filename is required for encrypt or decrypt");
    OutDamnedHelp();
    return;
}

if (createKey)
{
    var newKey = EncryptionDecryption.CreateNewKey();
    Console.WriteLine($"Key: {newKey}");
    return;
}

var kvUri = "https://shirt-storm-vaults.vault.azure.net/";
var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
var keySecret = client.GetSecret("appsettingscipher");

if (encrypt)
{
    EncryptionDecryption.EncryptSecuredJsonFields(filePath, keySecret.Value.Value);
    Console.WriteLine("Complete");
    return;
}

if (decrypt)
{
    EncryptionDecryption.DecryptSecuredJsonFields(filePath, keySecret.Value.Value);
    Console.WriteLine("Complete");
    return;
}

void OutDamnedHelp()
{
    Console.WriteLine("Creates a new Aes256 key or encrypts a file or decrypts a file. The key for encrypting and decrypting is in the secret vault.\nWhen encrypting or decrypting a file, the application with modify the values found after 'CipherText:'. In the\nexample \"Connection_string\":\"CipherText:<data>\" just <data> will be changed.\n");
    Console.WriteLine("Aes256Cipher.exe create|encrypt|decrypt [--filename|-f|/f \"file path\"] [--help|-h|/h]\n");
    Console.WriteLine("    create|encrypt|decrypt\tThe action to be taken, either create or encrypt or decrypt");
    Console.WriteLine("    --filename \"file name\"\tThe file name and path with the data to be encrypted or decrypted");
    Console.WriteLine("    --help\t\t\tThis help\n");
}