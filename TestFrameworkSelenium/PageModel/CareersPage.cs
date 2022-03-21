using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using SeleniumExtras.PageObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace TestSelenium.PageModel
{
    public class CareersPage
    {
        public CareersPage(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//a[contains(@id,'hyper')]")]
        public IList<IWebElement> LocJobList { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'container')]/h1")]
        public IWebElement LocCareersHeader { get; set; }

        [FindsBy(How = How.XPath, Using = "//p[contains(@class,'jobtitle')]/span")]
        public IWebElement LocJobTitle { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='rightcol']/ul[2]/li/span/a")]
        public IWebElement LocManagerJobList { get; set; }

        public void ValidateCareersPageHeader()
        {
            Assert.AreEqual("CAREERS", LocCareersHeader.Text);
        }        
    }
}

