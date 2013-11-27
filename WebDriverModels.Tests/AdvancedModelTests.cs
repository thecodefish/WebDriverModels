using System;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using SubSpec;
using WebDriverModels.Tests.Configuration;
using WebDriverModels.Tests.Models;
using Xunit;

namespace WebDriverModels.Tests
{
	public class AdvancedModelTests
	{
		[Specification]
		public void LoadingSingleModelForReadAccess()
		{
			IWebDriver driver = null;
			AdvancedModel model = null;
			var exception = default(Exception);

			"Given a browser pointed at the basic test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Basic.html");

					return driver;
				});

			"When the advanced model is loaded"
				.Do(() => exception = Record.Exception(() => model = ModelFinder.FindModel<AdvancedModel>(driver)));

			"Then no exceptions should be thrown"
				.Assert(() => Assert.Null(exception));

			"The model should load a string property by ID by default"
				.Assert(() => Assert.Equal("Item #1 Text", model.ItemOne));

			"The model should load a string property by ID"
				.Assert(() => Assert.Equal("Item #2 Text", model.ItemTwo));

			"The model should load a string property by class name"
				.Assert(() => Assert.Equal("Item #3 Text", model.ItemThree));

			"The model should load a string property by CSS selector"
				.Assert(() => Assert.Equal("Item #4 Text", model.ItemFour));

			"The model should load a string property from an input field"
				.Assert(() => Assert.Equal("Item #5 Text", model.ItemFive));
		}

		

		[Specification]
		public void LoadingModelThatCannotBeFound()
		{
			IWebDriver driver = null;
			var exception = default(Exception);

			"Given a browser pointed at the empty test page"
				.ContextFixture(() =>
					{
						driver = CurrentDriver.Driver = new PhantomJSDriver();
						driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Empty.html");

						return driver;
					});

			"When the advanced model is loaded"
				.Do(() => exception = Record.Exception(() => ModelFinder.FindModel<AdvancedModel>(driver)));

			"Then an exception should be thrown"
				.Assert(() => Assert.NotNull(exception));
		}

		[Specification]
		public void ModelExistsShouldReturnTrueIfTheModelCanBeFound()
		{
			IWebDriver driver = null;
			bool result = false;

			"Given a browser pointed at the basic test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Basic.html");

					return driver;
				});

			"When ModelExists is called"
				.Do(() => result = ModelFinder.ModelExists<AdvancedModel>(driver));

			"Then the result should be true"
				.Assert(() => Assert.True(result));
		}

		[Specification]
		public void ModelExistsShouldReturnFalseIfTheModelCannotBeFound()
		{
			IWebDriver driver = null;
			bool result = false;

			"Given a browser pointed at the empty test page"
				.ContextFixture(() =>
				{
					driver = CurrentDriver.Driver = new PhantomJSDriver();
					driver.Navigate().GoToUrl(TestConfiguration.BaseUrl + "Empty.html");

					return driver;
				});

			"When ModelExists is called"
				.Do(() => result = ModelFinder.ModelExists<AdvancedModel>(driver));

			"Then the result should be false"
				.Assert(() => Assert.False(result));
		}
	}
}
