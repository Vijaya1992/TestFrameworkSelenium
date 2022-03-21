using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;
using OpenQA.Selenium.Support.UI;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TestSelenium.Common;
using TestSelenium.PageModel;

namespace TestSelenium
{
    public class CommonTest
    {
        #region Datamember
        string machinename = System.Environment.MachineName;
        protected static IWebDriver driver;
        protected static string browserUsed;
        protected string testEnvironment;
        protected static string projectPath;
        protected static string driverPath;
        protected string commonFilePath;
        protected string testClassName;
        protected static string applicationUnderTest;
        protected CommonTestDataReader commonProperties;
        protected static ConfigReader _configReader;
        protected Dictionary<string, List<string>> testExecution = new Dictionary<string, List<string>>();
        public TestContext TestContext { get; set; }
        protected HomePage _homePage;
        protected CareersPage _careersPage;
        #endregion


        #region Framework SetuUp Methods
        /// <summary>
        ///  Set Up Method to open the browser and configuring the common properties
        /// </summary>
        public void SetUp()
        {
            SetEnvironment();
            InitDriver();
            InitializePageModel();
            NavigateToUrl(commonProperties.GetProperty("url").ToString());
        }

        /// <summary>
        /// driver initialization      
        /// Driver Path is for C:Drivers path outside project folder
        /// Commonfile path is IncontactAutomation/IncontactAutomation/Driver
        /// Project path is for project specific path (i.e individual applications)
        /// </summary>
        public void InitDriver()
        {
            commonFilePath = "C:/Doc/";
            browserUsed = commonProperties.GetProperty("browser").ToString();
            driverPath = commonProperties.GetProperty("driverPath").ToString();
            driver = BrowserFactory.GetDriverInstance(browserUsed, commonFilePath, projectPath, driverPath);
            DefaultWaitTime();
        }

        /// <summary>
        /// loading common properties browser value is setting here
        /// </summary>
        public void SetEnvironment()
        {
            commonProperties = CommonTestDataReader.GetInstance(commonFilePath + FrameworkConstants.filePath);
        }

        /// <summary>
        /// Responsible for Taking screenshot
        /// </summary>
        /// <param name="screenShotName"></param>
        /// <returns></returns>
        protected string CaptureScreenshot(string screenShotName)
        {
            string localpath = "";
            try
            {
                string finalpth = "";
                ITakesScreenshot ts = (ITakesScreenshot)driver;
                Screenshot screenshot = ts.GetScreenshot();
                string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
                var dir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", "");
                DirectoryInfo di = Directory.CreateDirectory(dir + "\\Defect_Screenshots\\");
                if (screenShotName.Contains("("))
                {
                    finalpth = pth.Substring(0, pth.LastIndexOf("bin")) + "\\Defect_Screenshots\\" + screenShotName.Substring(0, screenShotName.IndexOf("(")) + GetTimestamp(DateTime.Now) + ".png";
                }
                else
                {
                    finalpth = pth.Substring(0, pth.LastIndexOf("bin")) + "\\Defect_Screenshots\\" + screenShotName + GetTimestamp(DateTime.Now) + ".png";
                }
                localpath = new Uri(finalpth).LocalPath;
                screenshot.SaveAsFile(localpath);
            }
            catch (Exception e)
            {
                throw e;
            }
            return localpath;
        }

        /// <summary>
        /// Will execute after every test should be call from Teardown annotation in test class
        /// </summary>
        public void AfterTest()
        {
            try
            {
                var TestStatus = TestContext.CurrentTestOutcome;
                var stacktrace = "" + TestContext.TestLogsDir;
                var errorMessage = TestContext.TestName;
                switch (TestStatus)
                {
                    case UnitTestOutcome.Failed:
                        string screenShotPath = CaptureScreenshot(TestContext.TestName.ToString());
                        break;
                    case UnitTestOutcome.Aborted:
                        break;
                    case UnitTestOutcome.Passed:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                driver.Close();
            }
        }

        /// <summary>
        /// default wait time is setting here for page synchronization
        /// </summary>
        public void DefaultWaitTime()
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(120);
            driver.Manage().Window.Maximize();

        }

        #endregion

        #region Other Methods

        /// <summary>
        /// to get the current timestamp
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        public void InitializePageModel()
        {
            _homePage = new HomePage(driver);
            _careersPage = new CareersPage(driver);
        }
        #endregion

        #region Selenium Methods

        public string GetAttribute(IWebElement element, string attribute)
        {
            return element.GetAttribute(attribute);
        }

        public void ScrollToElement(IWebElement element)
        {

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true)", element);
        }

        /// <summary>
        ///  Method to open the url. Set bool to true if application have window authentication pop up
        ///  else set it to false
        /// </summary>
        /// <param name="uri"></param>       
        public void NavigateToUrl(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// This method wait till the element is present
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="element"></param>
        /// <param name="timeoutInSeconds"></param>
        public void ExplicitWait(IWebElement element, long timeoutInSeconds)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(d => element.Displayed);
        }

        public void WaitUntilElementIsClickable(IWebElement element, long timeOut)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));
        }

        /// <summary>
        /// Fluent wait we can set the polling time 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="timeout"></param>
        /// <param name="pollingTime"></param>
        public void FluentWait(IWebElement element, long timeout, long pollingTime)
        {
            DefaultWait<IWebElement> wait = new DefaultWait<IWebElement>(element);
            wait.Timeout = TimeSpan.FromSeconds(timeout);
            wait.PollingInterval = TimeSpan.FromMilliseconds(pollingTime);
        }

        /// <summary>
        /// to click the element
        /// </summary>
        /// <param name="element"></param>
        public void ClickElement(IWebElement element)
        {
            FluentWait(element, 30, 1000);
            LocateHiddenElement(element);
            element.Click();
        }

        public string GetText(IWebElement element)
        {

            LocateHiddenElement(element);
            return element.Text;
        }

        public static IWebElement LocateHiddenElement(IWebElement element)
        {
            ICoordinates cor = ((ILocatable)element).Coordinates;
            cor.LocationInViewport.ToString();
            return element;
        }        

        /// <summary>
        ///  use this method to check whether element is displayed on the page or not
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool IsElementDisplayed(IWebElement element)
        {
            try
            {
                return element.Displayed;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
        }
        
        #endregion


        #region API methods
        public RestClient SetUpRestClient(string baseurl, string username = null, string password = null)
        {
            RestClient restClient = new RestClient(baseurl);
            restClient.Authenticator = new HttpBasicAuthenticator(username, password);
            return restClient;
        }

        public RestRequest PostRequest(string endpoint, string acceptToken = null)
        {

            RestRequest restRequest = new RestRequest(endpoint, Method.POST);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", acceptToken);
            return restRequest;
        }
        public RestRequest PutRequest(string endpoint, string acceptToken = null)
        {

            RestRequest restRequest = new RestRequest(endpoint, Method.PUT);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", acceptToken);
            return restRequest;
        }


        public RestRequest AddParameter(RestRequest restRequest, Dictionary<string, object> parameter)
        {
            foreach (KeyValuePair<string, object> entry in parameter)
            {
                restRequest.AddParameter(entry.Key, entry.Value);
            }
            return restRequest;
        }


        public RestRequest GetRequest(string endpoint, Dictionary<string, object> parameter, string acceptToken = null)
        {
            RestRequest getRequest = new RestRequest(endpoint, Method.GET);
            getRequest = AddParameter(getRequest, parameter);
            getRequest.RequestFormat = DataFormat.Json;
            getRequest.AddHeader("Content-Type", "application/json");
            getRequest.AddHeader("Accept", acceptToken);
            return getRequest;
        }

        public RestRequest GetRequest(string endpoint, string acceptToken = null)
        {
            RestRequest getRequest = new RestRequest(endpoint, Method.GET);
            getRequest.RequestFormat = DataFormat.Json;
            getRequest.AddHeader("Content-Type", "application/json");
            getRequest.AddHeader("Accept", acceptToken);
            return getRequest;
        }
        #endregion









    }
}
