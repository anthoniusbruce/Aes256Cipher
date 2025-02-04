using Aes256Cipher;

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
var key = string.Empty;

var filePathActive = false;
var keyActive = false;
foreach (var arg in args)
{
    if (filePathActive)
    {
        filePath = arg;
        filePathActive = false;
    }
    else if (keyActive)
    {
        key = arg;
        keyActive = false;
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
    else if (arg.ToLower() == "--key" || arg.ToLower() == "-k" || arg.ToLower() == "/k")
    {
        keyActive = true;
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

if ((encrypt || decrypt) && string.IsNullOrWhiteSpace(key))
{
    Console.WriteLine("--key is required for encrypt or decrypt");
    OutDamnedHelp();
    return;
}

if (createKey)
{
    var newKey = Cipher.CreateNewKey();
    Console.WriteLine($"Key: {newKey}");
    return;
}

if (encrypt)
{
    Cipher.EncryptSecuredJsonFields(filePath, key);
    Console.WriteLine("Complete");
    return;
}

if (decrypt)
{
    Cipher.DecryptSecuredJsonFields(filePath, key);
    Console.WriteLine("Complete");
    return;
}

void OutDamnedHelp()
{
    Console.WriteLine("Creates a new Aes256 key or encrypts a file or decrypts a file. A key is required when encrypting or decrypting.\nWhen encrypting or decrypting a file, the application with modify the values found after 'CipherText:'. In the\nexample \"Connection_string\":\"CipherText:<data>\" just <data> will be changed.\n");
    Console.WriteLine("Aes256Cipher.exe create|encrypt|decrypt [--key|-k|/k \"key\"] [--filename|-f|/f \"file path\"] [--help|-h|/h]\n");
    Console.WriteLine("    create|encrypt|decrypt\tThe action to be taken, either create or encrypt or decrypt");
    Console.WriteLine("    --key \"key\"\tThe key that will encrypt or decrypt the given file");
    Console.WriteLine("    --filename \"file name\"\tThe file name and path with the data to be encrypted or decrypted");
    Console.WriteLine("    --help\t\t\tThis help\n");
}