using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFrameworkSelenium.PageModel;
using TestSelenium;
using TestSelenium.Common;

namespace TestFrameworkSelenium
{   
    [TestClass]
    public class TestcasesAPI : CommonTest
    {
        protected static CommonTestDataReader properties;
        [TestMethod]
        public void GetAllThePosts()
        {
            _apiUtil = new APIUtil();
            var restClient = SetUpRestClient("https://jsonplaceholder.typicode.com");
            var restRequest = GetRequest("/posts");
            var response = restClient.Execute(restRequest);
            _apiUtil.ValidateResponseCode(response.StatusCode.ToString(), FrameworkConstants.Success);
        }

        [TestMethod]
        public void AddThePosts()
        {
            _apiUtil = new APIUtil();
            var restClient = SetUpRestClient("https://jsonplaceholder.typicode.com");
            var restRequest = GetRequest("/posts");
            var request = new[]
           {                 
                new
                {
                    parameterName = "@userId",                    
                    parameterValue = "1"
                },
                new
                {
                    parameterName = "@id",                    
                    parameterValue = "101"
                },
            new
            {
                parameterName = "@title",
                parameterValue = "Testing the API"
            },
            new
            {
                parameterName = "@body",
                parameterValue = "Testing the API. API is working fine"
            }
            }.ToList();
            restRequest.AddJsonBody(request);
            var response = restClient.Execute(restRequest);           
            var content = response.Content;
            var code = response.StatusCode;             
            _apiUtil.ValidateResponseCode(response.StatusCode.ToString(), FrameworkConstants.Success);
        }

        [TestMethod]
        public void DeleteThePosts()
        {
            var id = "100";
            _apiUtil = new APIUtil();
            var restClient = SetUpRestClient("https://jsonplaceholder.typicode.com");
            var restRequest = DeleteRequest("/posts/"+id+"");
            var response = restClient.Execute(restRequest);
            _apiUtil.ValidateResponseCode(response.StatusCode.ToString(), FrameworkConstants.Success);
        }

    }
}
