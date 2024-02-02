using System.Text;
using Services;

namespace TestProject.BackendTests;

public class EncryptionTests
{
    [Theory]
    [InlineData("password", "Pls work")]
    [InlineData("anotherPassword", "Pls work again")]
    [InlineData("andAnotherOne", "No errors pls")]
    public async Task CompareEncryptedTextToOriginalText(string password, string text)
    {
        EncryptFileService service = new EncryptFileService();

        MemoryStream output = await service.EncryptFile(password, Encoding.Unicode.GetBytes(text));

        string encryptedText = Encoding.Unicode.GetString(output.ToArray());
        
        Assert.NotEqual(text, encryptedText);
    }
    
    [Theory]
    [InlineData("password", "Pls work")]
    [InlineData("anotherPassword", "Pls work again")]
    [InlineData("andAnotherOne", "No errors pls")]
    public async Task CompareDecryptedTextToOriginalText(string password, string text)
    {
        EncryptFileService service = new EncryptFileService();

        MemoryStream encryptedOutput = await service.EncryptFile(password, Encoding.Unicode.GetBytes(text));
        MemoryStream decryptedOutput = await service.DecryptFile(password, encryptedOutput.ToArray());
        string decryptedText = Encoding.Unicode.GetString(decryptedOutput.ToArray());
        
        Assert.Equal(text, decryptedText);
    }
}