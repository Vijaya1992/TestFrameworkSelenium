using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestSelenium.PageModel
{
    public class HomePage
    {
        public HomePage(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//a[contains(.,'Company')]")]
        public IWebElement LocCompany { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(.,'Careers')]")]
        public IWebElement LocCareersLink { get; set; } 

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'site-branding')]/a")]
        public IWebElement LocSiteBranding{ get; set; }

        /// <summary>
        /// This method is to click on careers link
        /// </summary>
        public void NavigateToCareers()
        {
            LocCareersLink.Click();
        }

        public void ValidateSiteBranding()
        {
            Assert.AreEqual("AGDATA", LocSiteBranding.Text);
        }



    }
}
