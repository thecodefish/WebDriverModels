using System;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using SubSpec;
using WebDriverModels.Tests.Configuration;
using WebDriverModels.Tests.Models;
using Xunit;

namespace WebDriverModels.Tests.Specs
{
	public class ModelExistsWithTimeoutSpecs
	{
		[Specification]
		public void ModelExistsWithLongTimeoutWhenModelAppearsAfterShortDelay()
		{
			IWebDriver driver = null;
			bool foundModel = false;
			var exception = default(Exception);

			"Given a browser pointed at the delayed display test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Delayed.html");

					return driver;
				});

			"When the advanced model is loaded with a 5 second timeout"
				.Do(() => exception = Record.Exception(() => foundModel = ModelFinder.ModelExists<AdvancedModel>(driver, TimeSpan.FromSeconds(5))));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The model should be found"
				.Assert(() => Assert.True(foundModel));
		}

		[Specification]
		public void ModelExistsWithShortTimeoutWhenModelAppearsAfterLongDelay()
		{
			IWebDriver driver = null;
			bool foundModel = false;
			var exception = default(Exception);

			"Given a browser pointed at the delayed display test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Delayed.html");

					return driver;
				});

			"When the advanced model is loaded with a 1 second timeout"
				.Do(() => exception = Record.Exception(() => foundModel = ModelFinder.ModelExists<AdvancedModel>(driver, TimeSpan.FromSeconds(0.5))));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The model should not be found"
				.Assert(() => Assert.False(foundModel));
		}

		[Specification]
		public void ModelExistsWithTimeoutWhenModelNeverAppears()
		{
			IWebDriver driver = null;
			bool foundModel = false;
			var exception = default(Exception);

			"Given a browser pointed at the empty test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Empty.html");

					return driver;
				});

			"When the input model is loaded with a 2 second timeout"
				.Do(() => exception = Record.Exception(() => foundModel = ModelFinder.ModelExists<InputModel>(driver, TimeSpan.FromSeconds(1))));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The model should not be found"
				.Assert(() => Assert.False(foundModel));
		}

		[Specification]
		public void ModelExistsWithTimeoutWhenModelAlreadyExists()
		{
			IWebDriver driver = null;
			bool foundModel = false;
			var exception = default(Exception);

			"Given a browser pointed at the basic test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Basic.html");

					return driver;
				});

			"When the advanced model is loaded with a 2 second timeout"
				.Do(() => exception = Record.Exception(() => foundModel = ModelFinder.ModelExists<AdvancedModel>(driver, TimeSpan.FromSeconds(1))));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The model should be found"
				.Assert(() => Assert.True(foundModel));
		}
	}
}