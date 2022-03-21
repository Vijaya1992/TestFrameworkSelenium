using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Interactions;
using TestSelenium;

namespace TestFrameworkSelenium
{
    [TestClass]
    public class TestCases : CommonTest
    {        
        [TestMethod]
        public void VerifyTheManagerPostIsAvailableOnCarrersPage()
        {
            SetUp();
            //Validate the page has loaded properly
            IsElementDisplayed(_homePage.LocSiteBranding);
            _homePage.ValidateSiteBranding();
            ClickElement(_homePage.LocCompany);
            _homePage.NavigateToCareers();
            _careersPage.ValidateCareersPageHeader();
            driver.SwitchTo().Frame("HBIFRAME");
            IsElementDisplayed(_careersPage.LocManagerJobList);
            ScrollToElement(_careersPage.LocManagerJobList);
            Actions actions = new Actions(driver);
            actions.MoveToElement(_careersPage.LocManagerJobList).Click().Perform();
            _careersPage.LocManagerJobList.Click();
            IsElementDisplayed(_careersPage.LocJobTitle);
            ScrollToElement(_careersPage.LocJobTitle);
            Assert.AreEqual("Quality Assurance Manager", _careersPage.LocJobTitle.Text);
        }

        [TestCleanup]
        public void CleanUp()
        {
            AfterTest();
        }
    }
}
