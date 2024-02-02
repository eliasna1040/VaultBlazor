using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestProject.FrontendTests;

public class SeleniumTests
{
    [Theory]
    [InlineData("EncryptedFileNo1", "selenium test", "selenium")]
    [InlineData("EncryptedFileNo2", "selenium test1", "selenium1")]
    public void TestEncryptUI(string fileName, string text, string password)
    {
        string filePath = $@"C:\FilesToEncrypt\{fileName}.txt";
        using (FileStream fileStream = File.Create(filePath))
            fileStream.Write(Encoding.Unicode.GetBytes(text));
        
        using IWebDriver webDriver = CreateWebdriver();
        webDriver.Navigate().GoToUrl("https://localhost:7279/");
        webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        IWebElement? tabContent = webDriver.FindElement(By.Id("encrypt-tab-content"));
        tabContent.FindElement(By.ClassName("form-control")).SendKeys(password);
        tabContent.FindElement(By.ClassName("upload-file-button")).SendKeys(filePath);
        tabContent.FindElement(By.TagName("button")).Click();
        Task.Delay(3000).Wait();
        using FileStream outputFile = File.OpenRead($@"C:\Users\elias\Downloads\{fileName}.txt");
        
        Assert.True(outputFile.Length > 0);
    }

    public async Task TestDecryptUI(string fileName, string text, string password)
    {
        string filePath = $@"C:\EncryptedFiles\{fileName}.txt";
        await using (FileStream fileStream = File.Create(filePath))
            await fileStream.WriteAsync(Encoding.Unicode.GetBytes(text));
        
        using IWebDriver webDriver = CreateWebdriver();
        webDriver.Navigate().GoToUrl("https://localhost:7279");

        IWebElement? tabContent = webDriver.FindElement(By.Id("decrypt-tab-content"));
        tabContent.FindElement(By.ClassName("form-control")).SendKeys(password);
        tabContent.FindElement(By.ClassName("form-control")).SendKeys(filePath);
        tabContent.FindElement(By.TagName("button")).Click();
        await using FileStream outputFile = File.OpenRead($@"C:\Users\elias\Downloads\{fileName}");
        byte[] buffer = new byte[outputFile.Length];
        await outputFile.ReadAsync(buffer);
        
        Assert.Equal(text, Encoding.Unicode.GetString(buffer));
    }

    private static IWebDriver CreateWebdriver()
    {
        return new ChromeDriver(new ChromeOptions()
        {
            BrowserVersion = "121"
        });
    }
}