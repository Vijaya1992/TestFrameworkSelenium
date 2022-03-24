using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFrameworkSelenium.PageModel
{
    public class APIUtil
    {
        public void ValidateResponseCode(string statusCode, string expStatus)
        {
            if (statusCode != expStatus)
            {
                Assert.Fail("Expected Response was : " + expStatus + " But Actual Response is : " + statusCode);
            }

        }
    }
}
