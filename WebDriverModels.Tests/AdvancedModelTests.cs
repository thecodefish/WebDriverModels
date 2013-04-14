using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using SubSpec;
using WebDriverModels.Tests.Models;
using Xunit;

namespace WebDriverModels.Tests
{
	public class AdvancedModelTests
	{
		[Specification]
		public void ShouldFindModelOnPage()
		{
			IWebDriver driver = null;
			AdvancedModel model = null;
			var exception = default(Exception);

			"Given a browser pointed at the basic test page"
				.ContextFixture(() =>
				{
					string htmlPath = ConfigurationManager.AppSettings["HtmlBasePath"];

					driver = CurrentDriver.Driver = new FirefoxDriver();

					driver.Navigate().GoToUrl("file://" + htmlPath + "Basic.html");

					return driver;
				});

			"When the advanced model is loaded"
				.Do(() => exception = Record.Exception(() => model = ModelFinder.FindModel<AdvancedModel>(driver)));

			"Then no exceptions should be thrown"
				.Observation(() => Assert.Null(exception));

			"The correct property value should be returned"
				.Observation(() => Assert.Equal("Item #1 Text", model.ItemOne));
		}

		[Specification]
		public void ShouldThrowExceptionIfModelDoesNotExistOnPage()
		{
			IWebDriver driver = null;
			var exception = default(Exception);

			"Given a browser pointed at the empty test page"
				.ContextFixture(() =>
					{
						string htmlPath = ConfigurationManager.AppSettings["HtmlBasePath"];

						driver = CurrentDriver.Driver = new FirefoxDriver();

						driver.Navigate().GoToUrl("file://" + htmlPath + "Empty.html");

						return driver;
					});

			"When the advanced model is loaded"
				.Do(() => exception = Record.Exception(() => ModelFinder.FindModel<AdvancedModel>(driver)));

			"Then an exception should be thrown"
				.Assert(() => Assert.NotNull(exception));
		}
	}
}
