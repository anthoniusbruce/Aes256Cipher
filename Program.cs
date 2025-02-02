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
var keyName = string.Empty;

var filePathActive = false;
var keyNameActive = false;
foreach (var arg in args)
{
    if (filePathActive)
    {
        filePath = arg;
        filePathActive = false;
    }
    else if (keyNameActive)
    {
        keyName = arg;
        keyNameActive = false;
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
    else if (arg.ToLower() == "--keyname" || arg.ToLower() == "-k" || arg.ToLower() == "/k")
    {
        keyNameActive = true;
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

if ((encrypt || decrypt) && string.IsNullOrWhiteSpace(keyName))
{
    Console.WriteLine("--keyname is required for encrypt or decrypt");
    OutDamnedHelp();
    return;
}

if (createKey)
{
    var newKey = Cipher.CreateNewKey();
    Console.WriteLine($"Key: {newKey}");
}

void OutDamnedHelp()
{
    Console.WriteLine("Creates a new Aes256 key or encrypts a file or decrypts a file. When creating a new key of a key name is given,\nthe application will replace the environment variable value with the new key. When encrypting or decrypting a\nfile, the application with modify the values found after 'CipherText:'. In the example of\n\"Connection_string\":\"CipherText:<data>\" will encrypt/decrypt just <data>.\n");
    Console.WriteLine("Aes256Cipher.exe create|encrypt|decrypt [--keyname|-k|/k \"key name\"] [--filename|-f|/f \"file path\"] [--help|-h|/h]\n");
    Console.WriteLine("    create|encrypt|decrypt\tThe action to be taken, either create or encrypt or decrypt");
    Console.WriteLine("    --keyname \"key name\"\tThe name of the environment variable where the key resides, not used\n\t\t\t\t    for create but required for encrypt or decrypt");
    Console.WriteLine("    --filename \"file name\"\tThe file name and path with the data to be encrypted or decrypted");
    Console.WriteLine("    --help\t\t\tThis help");
}