using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;

namespace TestSelenium.Common
{
    class BrowserFactory
    {
       
        public static IWebDriver GetDriverInstance(string browserName, string commonFilePath,string projectPath, string driverPath)
        {  
            switch (browserName)
            {

                case "CHROME":
                    return new ChromeDriver(SetDriverPath(commonFilePath, driverPath), SetChromesProperties(projectPath));               
                default:
                    return null;
            }

        }

        private static ChromeOptions SetChromesProperties(string path)
        {
           
            ChromeOptions chromeOptions = new ChromeOptions();            
            chromeOptions.AddArgument("test-type");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("start-maximized");
            chromeOptions.AddArguments("--window-size=1920,1080");
            //chromeOptions.AddArguments("-incognito");
            chromeOptions.AddArguments("--enable-precise-memory-info");
            chromeOptions.AddArguments("--disable-popup-blocking");
            chromeOptions.AddArguments("--disable-default-apps");
            
            //This is used for easily able to see the videos in Zalenium
            //chromeOptions.AddAdditionalCapability("name",TestContext.CurrentContext.Test.Name, true);

            chromeOptions.AddArguments("test-type=browser");           
            chromeOptions.AddUserProfilePreference("download.default_directory",  path+"Downloads\\");

            //This is the http auth pass extension
            //chromeOptions.AddExtension(CommonTest.GetProjectPath() + "\\Extension\\0.9.1_0.crx");

            chromeOptions.AcceptInsecureCertificates = true;            

            return chromeOptions;
        }
        private static string SetDriverPath(string commonFilePath, string driverPath)
        {
            string chromeDriver = driverPath + "chromedriver.exe";
            if (File.Exists(chromeDriver))
            {
                return driverPath;
            }
            else
            {
                return commonFilePath + "TestFrameworkSelenium/TestFrameworkSelenium/Driver";
            }
        }

    }
}
