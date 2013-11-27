using System;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using SubSpec;
using WebDriverModels.Tests.Configuration;
using WebDriverModels.Tests.Models;
using Xunit;

namespace WebDriverModels.Tests
{
    public class BasicModelTests
    {
        [Specification]
        public void ShouldFindModelOnPage()
        {
            IWebDriver _driver = null;
            BasicModel _model = null;
            var exception = default(Exception);

            "Given a browser pointed at the basic test page"
                .ContextFixture(() =>
                    {
						_driver = new PhantomJSDriver();
						_driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Basic.html");

                        return _driver;
                    });

            "When the basic model is loaded"
                .Do(() => exception = Record.Exception(() => _model = new BasicModel(_driver)));

            "Then no exceptions should be thrown"
                .Observation(() => Assert.Null(exception));

            "The correct property value should be returned"
                .Observation(() => Assert.Equal("Item #1 Text", _model.ItemOne.Text));
        }
    }
}
